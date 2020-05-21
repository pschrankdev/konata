using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Konata.Ruler
{
    public abstract class BaseRule
    {

        protected readonly List<BaseRule> _rules = new List<BaseRule>();
        protected readonly List<ValidationError> _validationErrors = new List<ValidationError>();
        protected string _validationMessage;
        protected string _propertyName;
        public Exception Exception
        {
            get
            {
                return new ValidacionException(SerializedValidationErrors, InnerException, ValidationErrors);
            }
        }

        public Exception InnerException { get; private set; }
        public bool BreaksValidation { get; }

        protected bool _returnValue;

        protected BaseRule(bool breaksValidation, string propertyName, string validationMessage)
        {
            BreaksValidation = breaksValidation;
            _validationMessage = validationMessage;
            _propertyName = propertyName;
        }
        public void AddRule(BaseRule rule)
        {
            this._rules.Add(rule);
        }

        protected abstract bool InnerMechanism();

        public bool GetIsValid()
        {
            this._returnValue = InnerMechanism();
            if (this._returnValue)
            {
                foreach (var rule in this._rules)
                {
                    if (!rule.GetIsValid())
                    {
                        _returnValue = false;
                        _validationErrors.AddRange(rule.ValidationErrors);
                        if (rule.BreaksValidation)
                        {
                            InnerException = rule.Exception;
                            break;
                        }
                    }
                }
            }
            return this._returnValue;
        }

        public List<ValidationError> ValidationErrors
        {
            get
            {
                var auxiliar = new List<ValidationError>
                    {
                        new ValidationError()
                            {
                                Property = _propertyName,
                                Message = _validationMessage
                            }
                    };
                auxiliar.AddRange(_validationErrors);
                return auxiliar;
            }
        }


        public string SerializedValidationErrors
        {
            get
            {
                return JsonConvert.SerializeObject(ValidationErrors);
            }
        }

    }
}
