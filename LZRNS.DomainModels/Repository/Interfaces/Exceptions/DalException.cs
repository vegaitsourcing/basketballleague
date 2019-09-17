using System;

namespace LZRNS.DomainModels.Repository.Interfaces.Exceptions
{
    public enum DalExceptionCode { UNIQUE_LEAGUE_NAME, UNIQUE_SEASON_DATA, LENGTH_VIOLATION, DEFAULT };

    public class DalException : Exception
    {
        public DalExceptionCode ExceptionCode { get; set; }

        public DalException()
        {
            ExceptionCode = DalExceptionCode.DEFAULT;
        }

        public DalException(string message)
            : base(message)
        {
        }

        public DalException(DalExceptionCode exceptionCode)
        {
            ExceptionCode = exceptionCode;
        }

        public DalException(string message, DalExceptionCode exceptionCode) : base(message)
        {
            ExceptionCode = exceptionCode;
        }

        public DalException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}