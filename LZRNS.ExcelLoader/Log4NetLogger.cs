﻿namespace LZRNS.ExcelLoader
{
    public static class Log4NetLogger
    {
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}