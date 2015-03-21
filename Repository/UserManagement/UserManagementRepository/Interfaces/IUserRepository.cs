using System;
using System.Linq.Expressions;
using UserManagement.Shared;

namespace UserManagementRepository.Interfaces
{
    public interface IUserRepository<TUser, TKey> where TUser : UserBase<TKey>, new() where TKey : IEquatable<TKey>
    {
        bool Exists(Expression<Func<TUser, bool>> exp);
        TUser SingleOrDefault(Expression<Func<TUser, bool>> exp);
        void UpdatePassword(TKey id, string newPassword, string newSecurityStamp);
        TKey Create(TUser repoUser);
    }
}
