using System.Text.RegularExpressions;

namespace Paynext.Domain.Validations
{
    public static partial class Rules
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return EmailRegex().IsMatch(email);
        }

        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            return PhoneNumberRegex().IsMatch(phone);
        }
        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            //string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$";
            //return Regex.IsMatch(password, passwordRegex);
            return PasswordRegex().IsMatch(password);
        }

        [GeneratedRegex(@"^(?=.* [a-z])(?=.* [A-Z])(?=.*\d)(?=.* [!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$")]
        private static partial Regex PasswordRegex();

        [GeneratedRegex(@"^(\d{2,3}|\(\d{2,3}\))?\s?\d{4,5}-?\d{4}$")]
        private static partial Regex PhoneNumberRegex();

        [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        private static partial Regex EmailRegex();


    }
}
