using System.ComponentModel.DataAnnotations;
using System;
using Flutter_API.Data;

namespace Flutter_API.Controllers.Validations
{
    public class EmailUserUniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var _context = (Flutter_APIContext)validationContext.GetService(typeof(Flutter_APIContext));
            var entity = _context.User.SingleOrDefault(e => e.Email == value.ToString());

            if (entity != null)
            {
                return new ValidationResult(GetErrorMessage(value.ToString()));
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage(string email)
        {
            return $"Email {email} is already in use.";
        }
    }
}
