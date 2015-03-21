using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using General;
using General.Requests;
using UserManagement.Shared;
using UserManagementRepository.Interfaces;
using UserManagementService.Interfaces;
using UserManagementService.Models;

namespace UserManagementService.Implementations
{
    public class GenericUserService<TServiceUser, TRepositoryUser, TKey> : IUserService<TServiceUser, TRepositoryUser, TKey>
        where TServiceUser : UserBase<TKey>, new()
        where TRepositoryUser : UserBase<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private IUserSettingsService _settingsService;
        private IUserRepository<TRepositoryUser, TKey> _userRepository;

        private readonly IPasswordHasher _passwordHasher;
        private readonly IPasswordValidator _passwordValidator;
        private readonly IUserValidator<TServiceUser, TKey> _userValidator;
        private readonly IClaimsIndentityService<TServiceUser, TKey> _claimsIndentityService;

        private bool _disposed;

        public GenericUserService(IUserSettingsService settingsService, IUserRepository<TRepositoryUser, TKey> userRepository, IPasswordHasher passwordHasher, IPasswordValidator passwordValidator, IUserValidator<TServiceUser, TKey> userValidator, IClaimsIndentityService<TServiceUser, TKey> claimsIndentityService)
        {
            _settingsService = settingsService;
            _userRepository = userRepository;

            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
            _userValidator = userValidator;
            _claimsIndentityService = claimsIndentityService;
            _userValidator.AllowOnlyAlphanumericUserNames = _settingsService.UsernameOnlyAlphaNumerics;
        }


        public TServiceUser FindById(TKey key)
        {
            ThrowIfDisposed();

            var repoUser = _userRepository.SingleOrDefault(x => x.Id.Equals(key));
            return GenericMapper<TRepositoryUser, TServiceUser>.ToModelWithSubTypes(repoUser);
        }

        public TServiceUser Find(string userName, string password)
        {
            ThrowIfDisposed();
            var repoUser = _userRepository.SingleOrDefault(x => x.UserName == userName);
            if (repoUser == null) return null;

            return VerifyPassword(repoUser, password) ? GenericMapper<TRepositoryUser, TServiceUser>.ToModelWithSubTypes(repoUser) : null;
        }

        public TServiceUser FindByName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("userName");
            ThrowIfDisposed();

            return GenericMapper<TRepositoryUser, TServiceUser>.ToModelWithSubTypes(_userRepository.SingleOrDefault(x => x.UserName == userName));
        }

        public ServiceResult<bool> ChangePassword(TKey userId, string currentPassword, string newPassword)
        {
            ThrowIfDisposed();
            var repoUser = _userRepository.SingleOrDefault(x => x.Id.Equals(userId));
            if (repoUser == null) throw new ArgumentNullException("userId", "unable to find user");

            if (!VerifyPassword(repoUser, currentPassword))
            {
                return ServiceResult<bool>.Error("user or password incorrect.");
            }

            return UpdatePasswordInternal(repoUser, newPassword);
        }

        public ServiceResult<bool> AddPassword(TKey userId, string password)
        {
            ThrowIfDisposed();
            var repoUser = _userRepository.SingleOrDefault(x => x.Id.Equals(userId));
            if (repoUser == null) throw new ArgumentNullException("userId", "unable to find user");

            return UpdatePasswordInternal(repoUser, password);
        }

        public ServiceResult<bool> AddLogin(TKey userId, string loginProvider, string providerKey)
        {
            throw new NotImplementedException();
        }

        public ServiceResult<bool> RemoveLogin(TKey key, string loginProvider, string providerKey)
        {
            throw new NotImplementedException();
        }
        
        public ServiceResult<bool> Create(TServiceUser user, string password)
        {
            ThrowIfDisposed();

            if (user == null) throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password");

            var errors = new List<string>();

            var userValidationResult = _userValidator.Validate(user);
            if (!userValidationResult.Valid)
            {
                errors.AddRange(userValidationResult.Errors);
            }

            var passwordValidation = _passwordValidator.Validate(password);
            if (!passwordValidation.Valid)
            {
                errors.AddRange(passwordValidation.Errors);
            }

            if(!string.IsNullOrWhiteSpace(user.Email) && _userRepository.Exists(u => u.Email.Trim().Equals(user.Email.Trim())))
            {
                errors.Add("Email address is already in use");
            }

            if (!string.IsNullOrWhiteSpace(user.UserName) && _userRepository.Exists(u => u.UserName.Trim().Equals(user.UserName.Trim())))
            {
                errors.Add("Username is already in use");
            }

            if (errors.Any())
            {
                return ServiceResult<bool>.Error(errors.ToArray());
            }

            var repoUser = GenericMapper<TRepositoryUser, TServiceUser>.ToEntityWithSubTypes(user);
            TKey id = _userRepository.Create(repoUser);
            repoUser.Id = id;

            UpdatePasswordInternal(repoUser, password);
            return new ServiceResult<bool> { Value = true, Succeeded = true };
        }

        public ClaimsIdentity CreateIdentity(TServiceUser user, string authenticationType)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
            	throw new ArgumentNullException("user");
            }
            return _claimsIndentityService.CreateAsync(user, authenticationType);
        }


        private bool VerifyPassword(TRepositoryUser repoUser, string password)
        {
            return _passwordHasher.VerifyHashedPassword(repoUser.PasswordHash, password) != PasswordVerificationResult.Failed;
        }

        /// <summary>
        /// Checks to if the given password meets the criteria.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private ValidationResult ValidatePassword(string password)
        {
            return _passwordValidator.Validate(password);
        }

        private ServiceResult<bool> UpdatePasswordInternal(TRepositoryUser repoUser, string password)
        {
            if (repoUser == null) throw new ArgumentNullException("repoUser");

            //ensure the password meets complexity rules.
            var result = ValidatePassword(password);
            if(!result.Valid)
            {
                return ServiceResult<bool>.Error(result.Errors);
            }

            var newPassword = _passwordHasher.HashPassword(password);
            var newSecurityStamp = Guid.NewGuid().ToString(); //not a salt.
            
            _userRepository.UpdatePassword(repoUser.Id, newPassword, newSecurityStamp);
            return ServiceResult<bool>.Success();
        }


        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     When disposing, actually dipose the store
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                //todo ensure the services are disposed / nulled.

                _settingsService = null;
                _userRepository = null;
                _disposed = true;
            }
        }
    }
}
