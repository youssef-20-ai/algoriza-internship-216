using AlgorizaProject.DAL.Entities;

namespace AlgorizaProject.Dtos
{
    public class DiscountDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int NumberOfRequests { get; set; }
        public DiscountEnum DiscountEnum { get; set; }
        public int Value { get; set; }
    }
}
