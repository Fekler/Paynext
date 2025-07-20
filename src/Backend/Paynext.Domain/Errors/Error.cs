namespace Paynext.Domain.Errors
{
    public static class Error
    {
        public const string INVALID_ID = "Invalid ID.";
        public const string INVALID_UUID = "Invalid UUID.";
        public const string INVALID_NAME = "Invalid Name.";
        public const string INVALID_DESCRIPTION = "Invalid Description.";
        public const string INVALID_DATE = "Invalid Date.";
        public const string INVALID_PASSWORD = "Invalid Password.";
        public const string INVALID_ADDRESS = "Invalid Address.";
        public const string INVALID_DOCUMENT = "Invalid Document.";
        public const string INVALID_PHONE = "Invalid Phone.";
        public const string INVALID_EMAIL = "Invalid E-mail.";

        public const string INVALID_LOGIN = "Invalid Login.";
        public const string INVALID_EMAIL_OR_PASSWORD = "Invalid email or password.";

        public const string INVALID_USER = "Invalid User.";
        public const string USER_NOT_FOUND = "User not found.";
        public const string USER_ALREADY_EXISTS = "User already exists.";
        public const string USER_ALREADY_EXISTS_EMAIL = "User with this email already exists.";
        public const string USER_ALREADY_EXISTS_PHONE = "User with this phone already exists.";
        public const string USER_ALREADY_EXISTS_DOCUMENT = "User with this document already exists.";
        public const string USER_NOT_AUTHORIZED = "User not authorized.";
        public const string USER_NOT_ACTIVE = "User not active.";


        public const string UNEXPECTED_ERROR = "An unexpected error occurred.";
        public const string UNAUTHORIZED = "Unauthorized.";
    }
}
