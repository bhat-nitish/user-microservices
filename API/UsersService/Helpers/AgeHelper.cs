using System;

namespace UsersService.Helpers
{
    public static class AgeHelper
    {
        public static string GetAge(DateTime dateOfBirth)
        {
            if (dateOfBirth == null)
                return "Invalid Date Of Birth";
            DateTime now = DateTime.Today;
            int age = now.Year - dateOfBirth.Year;
            if (now < dateOfBirth.AddYears(age)) age--;
            return age.ToString();
        }

        public static string GetFullAge(DateTime dateOfBirth)
        {
            if (dateOfBirth == null)
                return "Invalid Date Of Birth";

            DateTime Today = DateTime.Now;
            TimeSpan ts = Today - dateOfBirth;
            DateTime Age = DateTime.MinValue + ts;

            int years = Age.Year - 1;
            int months = Age.Month - 1;
            int days = Age.Day - 1;

            return $"{years} years {months} months {days} days";
        }
    }
}
