namespace Task9.Application.DTOs
{
    public class RegisterClientDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Pesel { get; set; } = string.Empty;
        public DateTime? PaymentDate { get; set; }
    }
}