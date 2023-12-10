using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorizaProject.DAL.Entities
{
    public class Discount : BaseEntity
    {
        public string DiscountCode { get; set; }
        public int RequestsCompleted { get; set; }
        public DiscountEnum DiscountEnum { get; set; }
        public int Value { get; set; }
        public DiscountAcivation DiscountStatus { get; set; }
    }

    public enum DiscountAcivation
    {
        Active,
        Deactive
    }
}
