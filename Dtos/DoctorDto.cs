using AlgorizaProject.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace AlgorizaProject.Dtos
{
    public class DoctorDto
    {
        public string? DoctorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Specialize { get; set; }
        public Gender Gender { get; set; }
        public string DOB { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
