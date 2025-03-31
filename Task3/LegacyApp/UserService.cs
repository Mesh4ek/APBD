using System;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ClientRepository _clientRepository;
        private readonly ICreditLimitCalculator _creditLimitCalculator;
        
        public UserService()
            : this(new UserRepository(), new ClientRepository(), new CreditLimitCalculator(() => new UserCreditService()))
        {
        }
        
        public UserService(IUserRepository userRepository, ClientRepository clientRepository, ICreditLimitCalculator creditLimitCalculator)
        {
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _creditLimitCalculator = creditLimitCalculator;
        }

        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                return false;

            if (!email.Contains("@") && !email.Contains("."))
                return false;

            int age = CalculateAge(dateOfBirth);
            if (age < 21)
                return false;

            var client = _clientRepository.GetById(clientId);

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                DateOfBirth = dateOfBirth,
                Client = client
            };

            _creditLimitCalculator.ApplyCreditLimit(user);

            if (user.HasCreditLimit && user.CreditLimit < 500)
                return false;

            _userRepository.AddUser(user);
            return true;
        }

        private int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age))
                age--;
            return age;
        }
    }
}
