using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using UserManagement.Shared;
using UserManagementRepository.Interfaces;

namespace UserManagementRepository.Implementations
{
    /// <summary>
    /// This repository is used by the user management service.  The db context must be passed in and have been configured outside of this class.
    /// It simply faciliates access to the TUser that has been defined and configured in the consuming aplication.
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class GenericUserRepository<TUser, TKey> : IUserRepository<TUser, TKey> where TUser : UserBase<TKey>, new() where TKey : IEquatable<TKey>
    {
        private readonly DbContext _context;

        /// <summary>
        /// pass in the pre configured dbcontext for the given TUser.
        /// </summary>
        /// <param name="context"></param>
        public GenericUserRepository(DbContext context)
        {
            _context = context;
        }

        public bool Exists(Expression<Func<TUser, bool>> exp)
        {
            return _context.Set<TUser>().Any(exp);
        }

        public TUser SingleOrDefault(Expression<Func<TUser, bool>> exp)
        {
            return _context.Set<TUser>().SingleOrDefault(exp);
        }

        public void UpdatePassword(TKey id, string newPassword, string newSecurityStamp)
        {
            var user = _context.Set<TUser>().Single(x => x.Id.Equals(id));
            user.PasswordHash = newPassword;
            user.SecurityStamp = newSecurityStamp;

            _context.SaveChanges();
        }

        public TKey Create(TUser repoUser)
        {
            if (repoUser == null) throw new ArgumentNullException("repoUser");

            _context.Set<TUser>().Add(repoUser);
            _context.SaveChanges();

            return repoUser.Id;
        }
    }
}
