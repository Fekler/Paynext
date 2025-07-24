namespace Paynext.Domain.Entities._bases
{
    public class Enums
    {
        public enum UserRole
        {
            Client,
            Admin
        }
        public enum InstallmentStatus
        {
            Open = 0,
            Paid = 1,
            Cancelled = 2
        }
        public enum AntecipationStatus
        {
            None = 0,
            Pending = 1,
            Approved = 2,
            Rejected = 3
        }
    }
}
