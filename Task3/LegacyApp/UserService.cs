using System;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserCreditService _creditService;
        private readonly IUserRepository _userRepository;

        public UserService() : this(new ClientRepository(), new UserCreditServiceWrapper(), new UserRepository())
        {
        }

        private UserService(
            IClientRepository clientRepository, 
            IUserCreditService creditService,
            IUserRepository userRepository)
        {
            _clientRepository = clientRepository;
            _creditService = creditService;
            _userRepository = userRepository;
        }

        public bool AddUser(
            string firstName, 
            string lastName, 
            string email, 
            DateTime dateOfBirth, 
            int clientId)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return false;
            }

            if (!email.Contains("@") && !email.Contains("."))
            {
                return false;
            }
            
            int age = CalculateAge(dateOfBirth, DateTime.Now);
            if (age < 21)
            {
                return false;
            }
            
            var client = _clientRepository.GetById(clientId);
            
            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                int creditLimit = _creditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                user.CreditLimit = creditLimit * 2;
            }
            else
            {
                user.HasCreditLimit = true;
                int creditLimit = _creditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                user.CreditLimit = creditLimit;
            }
            
            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }
            
            _userRepository.AddUser(user);

            return true;
        }

        private int CalculateAge(DateTime birthDate, DateTime now)
        {
            int age = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month 
                || (now.Month == birthDate.Month && now.Day < birthDate.Day))
            {
                age--;
            }
            return age;
        }
    }
}
