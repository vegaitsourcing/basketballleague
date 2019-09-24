namespace LZRNS.ExcelLoader
{
    public interface IExcelLoaderCorrector
    {
        void CorrectInvalidTeamNames(ExcelReader.ExcelLoader loader);
    }
}