using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorizaProject.DAL.Entities
{
    public class Specialization : BaseEntity
    {
        public string Special { get; set; }
        public ICollection<Doctor> Doctors { get; set; }
    }
}
