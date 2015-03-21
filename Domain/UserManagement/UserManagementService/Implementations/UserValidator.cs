using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using UserManagement.Shared;
using UserManagementService.Interfaces;
using UserManagementService.Models;

namespace UserManagementService.Implementations
{
    /// <summary>
    /// Validates if a user meets the given criteria, such as email and username validation and complexity.
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class UserValidator<TUser, TKey> : IUserValidator<TUser, TKey> where TUser : UserBase<TKey>
    {
    
        public bool AllowOnlyAlphanumericUserNames { get; set; }
       
        public ValidationResult Validate(TUser user)
        {
            var errors = new List<string>();

            ValidateUserEmail(user.Email, errors);
            ValidateUserName(user.UserName, errors);
            ValidateDsiplayName(user.DisplayName, errors);

            if (errors.Any())
            {
                return new ValidationResult
                {
                    Valid = false,
                    Errors = errors.ToArray()
                };
            }

            return new ValidationResult {Valid = true};
        }


        private bool ValidateUserEmail(string email, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                errors.Add("Email is empty");
                return false;
            }

            try
            {
                new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                errors.Add("Invalid Email");
            }

            return false;
        }

        private bool ValidateUserName(string username, List<string> errors)
        {
            var error = false;
            if (string.IsNullOrWhiteSpace(username))
            {
                errors.Add("Username is empty");
                return false;
            }

            if (username.Trim().Contains(" "))
            {
                errors.Add("username can not contain spaces");
                error = true;
            }

            if (AllowOnlyAlphanumericUserNames && !Regex.IsMatch(username, "^[A-Za-z0-9@_\\.]+$"))
            {
                errors.Add("Uername can only contain alpha numerics");
                error = true;
            }

            return error;
        }

        private bool ValidateDsiplayName(string name, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                errors.Add("Display Name is empty");
                return false;
            }

            return true;
        }
    }
}
