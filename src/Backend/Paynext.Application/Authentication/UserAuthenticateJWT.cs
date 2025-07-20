namespace Paynext.Application.Authentication
{
    public class UserAuthenticateJWT
    {
        public Guid UserUuid { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserRole { get; set; }

    }
}
