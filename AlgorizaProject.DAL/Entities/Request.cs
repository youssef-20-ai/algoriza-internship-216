using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorizaProject.DAL.Entities
{
    public class Request : BaseEntity
    {
        public string PatientId { get; set; }
        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }
        public string DoctorId {  get; set; }
        [ForeignKey(nameof(DoctorId))]
        public Doctor Doctor {  get; set; }   
        public AppoitmentEnum AppoitmentEnum { get; set; }
        public string Time { get; set; }
        public string DiscountCode { get; set; }
        public int FinalPrice { get; set; }
        public RequestStatus RequestStatus { get; set; }
    }
}
