using System;

namespace LegacyApp
{
    public class UserCreditServiceWrapper : IUserCreditService
    {
        public int GetCreditLimit(string lastName, DateTime dateOfBirth)
        {
            using var userCreditService = new UserCreditService();
            return userCreditService.GetCreditLimit(lastName, dateOfBirth);
        }
    }
}