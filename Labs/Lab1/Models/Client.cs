namespace DrDanielaVintu.Labs.Lab1.Models
{
    /// <summary>
    /// Reprezintă un client al clinicii.
    /// Demonstrează ÎNCAPSULAREA prin utilizarea proprietăților cu validări.
    /// </summary>
    public class Client
    {
        private string _email;

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                    throw new ArgumentException("Email invalid.");
                _email = value;
            }
        }

        public string PhoneNumber { get; set; }
    }
}
