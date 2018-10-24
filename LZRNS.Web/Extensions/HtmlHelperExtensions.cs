using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace LZRNS.Web.Extensions
{
	public static class HtmlHelperExtensions
	{
		public static void RenderAction<TController>(this HtmlHelper source, Expression<Action<TController>> expression, params object[] methodArguments)
			where TController : Controller
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (expression == null) throw new ArgumentNullException(nameof(expression));
			if (methodArguments == null) throw new ArgumentNullException(nameof(methodArguments));

			MethodInfo methodInfo = GetMethodInfo(expression);
			string controllerName = typeof(TController).Name.RemoveControllerSuffix();

			RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
			MatchArgumentsWithMethodParameters(methodInfo, (key, value) => routeValueDictionary.Add(key, value),  methodArguments);

			source.RenderAction(methodInfo.Name, controllerName, routeValueDictionary);
		}

		public static string Action<TController>(this UrlHelper source, Expression<Action<TController>> expression, object routeValues = null)
			where TController : Controller
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (expression == null) throw new ArgumentNullException(nameof(expression));

			MethodInfo methodInfo = GetMethodInfo(expression);
			string controllerName = typeof(TController).Name.RemoveControllerSuffix();

			return source.Action(methodInfo.Name, controllerName, routeValues);
		}

		private static MethodInfo GetMethodInfo<T>(Expression<T> expression)
		{
			var body = expression.Body as MethodCallExpression;
			if (body == null) throw new ArgumentException("Expression has to specify an existing method on the type T.", nameof(expression));

			return body.Method;
		}

		private static void MatchArgumentsWithMethodParameters(MethodInfo methodInfo, Action<string, object> action, params object[] methodArguments)
		{
			ParameterInfo[] parameters = methodInfo.GetParameters();
			if (parameters.Length != methodArguments.Length)
			{
				throw new ArgumentException("Number of provided arguments doesn't match with number of method parameters.", nameof(methodArguments));
			}

			IEnumerator parametersEnumerator = parameters.GetEnumerator();
			IEnumerator argumentsEnumerator = methodArguments.GetEnumerator();

			while (parametersEnumerator.MoveNext() && argumentsEnumerator.MoveNext())
			{
				action(((ParameterInfo)parametersEnumerator.Current).Name, argumentsEnumerator.Current);
			}
		}

        public static string IsSelected(this HtmlHelper html, string controllers = "", string actions = "", string cssClass = "active")
        {
            ViewContext viewContext = html.ViewContext;
            bool isChildAction = viewContext.Controller.ControllerContext.IsChildAction;

            if (isChildAction)
                viewContext = html.ViewContext.ParentActionViewContext;

            RouteValueDictionary routeValues = viewContext.RouteData.Values;
            string currentAction = routeValues["action"].ToString();
            string currentController = routeValues["controller"].ToString();

            if (String.IsNullOrEmpty(actions))
                actions = currentAction;

            if (String.IsNullOrEmpty(controllers))
                controllers = currentController;

            string[] acceptedActions = actions.Trim().Split(',').Distinct().ToArray();
            string[] acceptedControllers = controllers.Trim().Split(',').Distinct().ToArray();

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
                cssClass : String.Empty;
        }

        public static string Controller(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("controller"))
                return (string)routeValues["controller"];

            return string.Empty;
        }

        public static string Action(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("action"))
                return (string)routeValues["action"];

            return string.Empty;
        }


    }
}
