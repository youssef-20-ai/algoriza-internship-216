using AlgorizaProject.DAL.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace AlgorizaProject.Dtos
{
    public class BookingDto
    {
        public string PatientName { get; set; }
        public string Image { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
       // public AppoitmentEnum AppoitmentEnum { get; set; }
        //public string Time { get; set; }
    }
}
