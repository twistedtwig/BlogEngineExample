using System;
using System.Security.Claims;
using General.Requests;
using UserManagement.Shared;

namespace UserManagementService.Interfaces
{
    public interface IUserService<TServiceUser, TRepositoryUser, in TKey> : IDisposable
        where TServiceUser : UserBase<TKey>, new()
        where TRepositoryUser : UserBase<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        TServiceUser FindById(TKey key);
        TServiceUser Find(string userName, string password);
        TServiceUser FindByName(string userName);

        ServiceResult<bool> ChangePassword(TKey userId, string currentPassword, string newPassword);
        ServiceResult<bool> AddPassword(TKey userId, string password);
        ServiceResult<bool>  AddLogin(TKey userId, string loginProvider, string providerKey);
//        ServiceResult<bool> RemovePassword(TKey key);
        ServiceResult<bool> RemoveLogin(TKey key, string loginProvider, string providerKey);

//        ServiceResult<bool> Create(TServiceUser user);
        ServiceResult<bool> Create(TServiceUser user, string password);
        ClaimsIdentity CreateIdentity(TServiceUser user, string authenticationType);        
    }
}
