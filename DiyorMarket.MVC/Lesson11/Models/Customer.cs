namespace Lesson11.Models
{
    public class Customer
    {
        public int? Id { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public virtual ICollection<Sale>? Sales { get; set; }
    }
}
