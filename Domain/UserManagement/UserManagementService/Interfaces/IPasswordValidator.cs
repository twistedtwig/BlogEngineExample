using UserManagementService.Models;

namespace UserManagementService.Interfaces
{
    public interface IPasswordValidator
    {
        ValidationResult Validate(string password);
    }
}
