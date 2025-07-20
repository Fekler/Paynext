namespace Paynext.Domain.Validations
{
    public class RuleValidator
    {
        private List<string> _errorList;
        private string _message;

        private RuleValidator() => _errorList = [];

        internal static RuleValidator Build() => new();


        internal RuleValidator When(bool haserror, string error)
        {
            if (haserror)
            {
                _errorList.Add(error);
            }
            return this;
        }
        internal void ThrowExceptionIfExists()
        {
            if (_errorList.Count > 0)
            {
                _message = string.Join(", ", _errorList);
                _message += ".";

                throw new DomainExceptionValidation(_message);
            }
        }
    }
}