using AlgorizaProject.DAL.Entities;

namespace AlgorizaProject.Dtos
{

    public class RequestsParameters
    {
        public RequestStatus? RequestStatus { get; set; }
        public string? DoctorId { get; set; }
        public AppoitmentEnum? Appoitment { get; set; }
        public int? TimeId { get; set; }
    }
}
