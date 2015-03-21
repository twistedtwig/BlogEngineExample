
namespace UserManagementService.Models
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            Errors = new string[0];
        }

        public bool Valid { get; set; }
        public string[] Errors { get; set; }
    }
}
