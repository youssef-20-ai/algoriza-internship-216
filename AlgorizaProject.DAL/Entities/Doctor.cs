

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlgorizaProject.DAL.Entities
{
    public class Doctor : AppUser
    {
        public Specialization? specialization { get; set; }
        public int Price { get; set; }
        public List<Appoitment> Appoitments { get; set; }
        public List<Request> DoctorRequests { get; set; }
    }
}
