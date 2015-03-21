using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UserManagementService.Interfaces;
using UserManagementService.Models;

namespace UserManagementService.Implementations
{
    /// <summary>
    /// Ensures that a password meets the given criteria
    /// </summary>
    public class PasswordValidator : IPasswordValidator
    {
        public int MinLength { get; set; }
        public bool MustHaveNumeric { get; set; }
        public bool MustHaveNonAlphaNumeric { get; set; }
        public bool MustHaveUpperAndLowerChars { get; set; }

        public PasswordValidator()
        {
            MinLength = 6;
        }

        public ValidationResult Validate(string password)
        {            
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add("Password is empty");
                return new ValidationResult {Valid =  false, Errors = errors.ToArray() };
            }

            var pass = password.Trim();

            if (pass.Length < MinLength)
            {
                errors.Add("Password is too short");
            }

            if (MustHaveNumeric && !AssertHasNumeric(pass))
            {
                errors.Add("Password must contain at least one number");
            }

            if (MustHaveUpperAndLowerChars && !AssertUpperAndLowerCharacers(pass))
            {
                errors.Add("Password must contain upper and lower case characters");
            }

            if (MustHaveNonAlphaNumeric && AssertNonAlphaNumeric(pass))
            {
                errors.Add("Password must contain at least one non alpha numeric character");                
            }

            return new ValidationResult {Valid = !errors.Any(), Errors = errors.ToArray()};
        }


        public bool AssertHasNumeric(string pass)
        {
            return Regex.Match(pass, @"\d+", RegexOptions.ECMAScript).Success;
        }

        public bool AssertUpperAndLowerCharacers(string pass)
        {
            return (Regex.Match(pass, @"[a-z]", RegexOptions.ECMAScript).Success &&
                    Regex.Match(pass, @"[A-Z]", RegexOptions.ECMAScript).Success);
        }
        
        public bool AssertNonAlphaNumeric(string pass)
        {
            return Regex.Match(pass, @"^[a-zA-Z0-9]*$", RegexOptions.ECMAScript).Success;
        }
    }
}
