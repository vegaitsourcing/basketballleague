using System;
using System.Data.Entity.Validation;
using System.Text;

namespace LZRNS.DomainModels.Exceptions
{
    public class FormattedDbEntityValidationException : Exception
    {
        public FormattedDbEntityValidationException(DbEntityValidationException innerException) :
            base(null, innerException)
        {
        }

        public FormattedDbEntityValidationException()
        {
        }

        public FormattedDbEntityValidationException(string message) : base(message)
        {
        }

        public FormattedDbEntityValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public override string Message
        {
            get
            {
                if (InnerException is DbEntityValidationException innerException)
                {
                    var sb = new StringBuilder();

                    sb.AppendLine();
                    sb.AppendLine();
                    foreach (var eve in innerException.EntityValidationErrors)
                    {
                        sb.AppendFormat("- Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().FullName, eve.Entry.State).AppendLine();
                        foreach (var ve in eve.ValidationErrors)
                        {
                            sb.AppendFormat("-- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                ve.PropertyName,
                                eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                ve.ErrorMessage).AppendLine();
                        }
                    }
                    sb.AppendLine();

                    return sb.ToString();
                }

                return base.Message;
            }
        }
    }
}