using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Konata.Ruler
{
    [Serializable]
    public class ValidacionException : Exception
    {
        protected readonly List<ValidationError> validationErrors = new List<ValidationError>();
        public ValidacionException()
            : base()
        {
        }

        public ValidacionException(string message)
            : base(message)
        {
        }

        public ValidacionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public ValidacionException(string message, Exception inner, List<ValidationError> validationErrors)
            : base(message, inner)
        {
            this.validationErrors.AddRange(validationErrors);
        }

        public ValidacionException(string message, List<ValidationError> validationErrors)
            : base(message)
        {
            this.validationErrors.AddRange(validationErrors);
        }

        public List<ValidationError> GetValidationErrors()
        {
            return this.validationErrors;
        }

        protected ValidacionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            base.GetObjectData(info, context);
        }
    }
}
