using UserManagement.Shared;
using UserManagementService.Models;

namespace UserManagementService.Interfaces
{
    public interface IUserValidator<in TUser, TKey> where TUser : UserBase<TKey>
    {
        bool AllowOnlyAlphanumericUserNames { get; set; }
        ValidationResult Validate(TUser user);
    }
}
