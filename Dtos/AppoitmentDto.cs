using AlgorizaProject.DAL.Entities;

namespace AlgorizaProject.Dtos
{
    public class AppoitmentDto
    {
        public AppoitmentEnum AppoitmentEnum { get; set; }
        public List<TimeDto> Times { get; set; }
    }

}
