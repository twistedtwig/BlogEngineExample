using System;
using System.Security.Claims;
using UserManagement.Shared;

namespace UserManagementService.Interfaces
{
    public interface IClaimsIndentityService<TUser, TKey> 
        where TUser : UserBase<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        ClaimsIdentity CreateAsync(TUser user, string authenticationType);
    }
}
