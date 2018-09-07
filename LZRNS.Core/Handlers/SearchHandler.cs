using Examine;
using Examine.Providers;
using Examine.SearchCriteria;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Highlight;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LZRNS.Common.Extensions;
using Version = Lucene.Net.Util.Version;

namespace LZRNS.Core.Handlers
{
	public static class SearchHandler
	{
		/// <summary>
		/// Gets the search results.
		/// </summary>
		/// <param name="searchTerm">The search text. Search string separated whitespace.</param>
		/// <param name="searchFields">The search fields.</param>
		/// <param name="searchProviderName">Name of the search provider.</param>
		/// <param name="useAndOperation">Search with wildcard.</param>
		/// <returns>Collection of <see cref="Examine.SearchResult"/>.</returns>
		public static IEnumerable<SearchResult> GetSearchResults(string searchTerm, string[] searchFields, string searchProviderName, bool useAndOperation)
		{
			if (searchTerm == null) throw new ArgumentNullException(nameof(searchTerm));
			if (searchFields == null) throw new ArgumentNullException(nameof(searchFields));
			if (searchProviderName == null) throw new ArgumentNullException(nameof(searchProviderName));

			BaseSearchProvider searchProvider = ExamineManager.Instance.SearchProviderCollection[searchProviderName];
			ISearchCriteria criteria = searchProvider.CreateSearchCriteria(BooleanOperation.Or);
			string[] terms = searchTerm.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			IEnumerable<string> rawQueries = GetRawQueries(searchFields, useAndOperation, searchFields.Length, terms);
			ISearchCriteria filter = criteria.RawQuery(rawQueries.ToStringWithBrackets());

			return searchProvider.Search(filter).DistinctBy(sr => sr.Id);
		}

		/// <summary>
		/// Gets the highlight.
		/// </summary>
		/// <param name="indexField">The index field value.</param>
		/// <param name="searchQuery">The search query.</param>
		/// <param name="highlightField">The highlight field name.</param>
		/// <param name="examineIndexSetName">Name of the examine index set.</param>
		/// <param name="maxNumFragments">Maximum number of fragments to retrieve.</param>
		/// <param name="preTag">Highlight pre tag.</param>
		/// <param name="postTag">Highlight post tag.</param>
		/// <returns></returns>
		public static string GetHighlight(string indexField, string searchQuery, string highlightField, string examineIndexSetName, int maxNumFragments, string preTag, string postTag)
		{
			if (indexField == null) throw new ArgumentNullException(nameof(indexField));
			if (searchQuery == null) throw new ArgumentNullException(nameof(searchQuery));
			if (highlightField == null) throw new ArgumentNullException(nameof(highlightField));
			if (examineIndexSetName == null) throw new ArgumentNullException(nameof(examineIndexSetName));

			string indexFieldStrippedHtmlValue = indexField.StripHtml();
			SimpleHTMLFormatter formatter = new SimpleHTMLFormatter(preTag, postTag);
			Highlighter highlighter = new Highlighter(formatter, FragmentScorer(searchQuery, highlightField, examineIndexSetName));
			TokenStream tokenStream = new StandardAnalyzer(Version.LUCENE_29).TokenStream(highlightField, new StringReader(indexFieldStrippedHtmlValue));

			return highlighter.GetBestFragments(tokenStream, indexFieldStrippedHtmlValue, maxNumFragments, "...");
		}

		/// <summary>
		/// Gets the highlighted wildcard text.
		/// </summary>
		/// <param name="indexField">The index field value.</param>
		/// <param name="searchQuery">The search query.</param>
		/// <param name="highlightField">The highlight field name.</param>
		/// <param name="examineIndexSetName">Name of the examine index set.</param>
		/// <param name="maxNumFragments">Maximum number of fragments to retrieve.</param>
		/// <param name="preTag">Highlight pre tag.</param>
		/// <param name="postTag">Highlight post tag.</param>
		/// <returns></returns>
		public static string GetHighlightWithWildcards(string indexField, string searchQuery, string highlightField, string examineIndexSetName, int maxNumFragments, string preTag, string postTag)
		{
			if (indexField == null) throw new ArgumentNullException(nameof(indexField));
			if (searchQuery == null) throw new ArgumentNullException(nameof(searchQuery));
			if (highlightField == null) throw new ArgumentNullException(nameof(highlightField));
			if (examineIndexSetName == null) throw new ArgumentNullException(nameof(examineIndexSetName));

			BooleanQuery finalQuery = new BooleanQuery();

			foreach (string term in searchQuery.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
			{
				FuzzyQuery fuzzyQuery = new FuzzyQuery(new Lucene.Net.Index.Term(highlightField, term), 0.5f, 0);
				finalQuery.Add(new BooleanClause(fuzzyQuery, BooleanClause.Occur.SHOULD));
			}

			string indexFieldStrippedHtmlValue = indexField.StripHtml();
			SimpleHTMLFormatter formatter = new SimpleHTMLFormatter(preTag, postTag);
			QueryScorer fragmentScorer = new QueryScorer(finalQuery.Rewrite(GetIndexSearcher(examineIndexSetName).GetIndexReader()));
			Highlighter highlighter = new Highlighter(formatter, fragmentScorer);
			TokenStream tokenStream = new StandardAnalyzer(Version.LUCENE_29).TokenStream(highlightField, new StringReader(indexFieldStrippedHtmlValue));

			return highlighter.GetBestFragments(tokenStream, indexFieldStrippedHtmlValue, maxNumFragments, "...");
		}

		/// <summary>
		/// Fragments the scorer.
		/// </summary>
		/// <param name="searchQuery">The search query.</param>
		/// <param name="highlightField">The highlight field.</param>
		/// <param name="indexName">Name of the index.</param>
		/// <returns></returns>
		private static QueryScorer FragmentScorer(string searchQuery, string highlightField, string indexName)
		{
			return new QueryScorer(GetLuceneQueryObject(searchQuery, highlightField).Rewrite(GetIndexSearcher(indexName).GetIndexReader()));
		}

		/// <summary>
		/// Gets the lucene query object.
		/// </summary>
		/// <param name="q">The q.</param>
		/// <param name="field">The field.</param>
		/// <returns></returns>
		private static Query GetLuceneQueryObject(string q, string field)
		{
			return new QueryParser(Version.LUCENE_29, field, new StandardAnalyzer(Version.LUCENE_29)).Parse(q);
		}

		/// <summary>
		/// Gets the index searcher.
		/// </summary>
		/// <param name="indexName">Name of the index.</param>
		/// <returns></returns>
		private static IndexSearcher GetIndexSearcher(string indexName)
		{
			string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_data", "TEMP", "ExamineIndexes", indexName, "Index");
			FSDirectory directory = new MMapDirectory(new DirectoryInfo(path));
			return new IndexSearcher(directory, true);
		}

		private static IEnumerable<string> GetRawQueries(string[] searchFields, bool useAndOperation, int searchFieldsCount, string[] terms)
		{
			for (int i = 0; i < searchFieldsCount; i++)
			{
				string escapedTerms = string.Join(" ", terms.Select(term => QueryParser.Escape(term) + GetOperationSymbol(useAndOperation)));
				string luceneQuery = string.Format("(+__IndexType:content && {0}:({1}){2})",
					searchFields[i], escapedTerms, GetBoostExpression(useAndOperation, searchFieldsCount - i));

				yield return luceneQuery;
			}
		}

		private static string GetBoostExpression(bool useAndOperation, int position = 0)
		{
			return useAndOperation ? string.Empty : "^" + Math.Pow(2, position);
		}

		private static string GetOperationSymbol(bool useAndOperation)
		{
			return useAndOperation ? "*" : string.Empty;
		}
	}
}
