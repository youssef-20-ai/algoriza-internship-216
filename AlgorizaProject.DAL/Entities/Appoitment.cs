using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AlgorizaProject.DAL.Entities
{
    public class Appoitment : BaseEntity
    {
        public AppoitmentEnum AppoitmentEnum { get; set; }
        public List<AppoitmentTime> Times { get; set; }
        public string DoctorId { get; set; }
        [ForeignKey(nameof(DoctorId))]
        public Doctor Doctor { get; set; }
    }
}
