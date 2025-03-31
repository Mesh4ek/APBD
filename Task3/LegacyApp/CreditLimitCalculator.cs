using System;

namespace LegacyApp
{
    public class CreditLimitCalculator : ICreditLimitCalculator
    {
        private readonly Func<UserCreditService> _creditServiceFactory;

        public CreditLimitCalculator(Func<UserCreditService> creditServiceFactory)
        {
            _creditServiceFactory = creditServiceFactory;
        }

        public void ApplyCreditLimit(User user)
        {
            // Cast the client property from object to Client
            var client = user.Client as Client;
            if (client == null)
            {
                throw new InvalidOperationException("User.Client is not a valid Client object.");
            }

            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                using (var creditService = _creditServiceFactory())
                {
                    user.CreditLimit = creditService.GetCreditLimit(user.LastName, user.DateOfBirth) * 2;
                }
            }
            else
            {
                user.HasCreditLimit = true;
                using (var creditService = _creditServiceFactory())
                {
                    user.CreditLimit = creditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                }
            }
        }
    }
}