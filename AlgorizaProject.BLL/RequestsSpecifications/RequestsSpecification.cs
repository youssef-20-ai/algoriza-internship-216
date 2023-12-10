using AlgorizaProject.DAL.Entities;
using AlgorizaProject.Dtos;
using APIDemo.BLL.Sepecifications;


namespace AlgorizaProject.BLL.RequestsSpecifications
{
    public class RequestsSpecification : BaseSpecification<Request>
    {
        public RequestsSpecification(RequestsParameters requestsParameters) :
            base(r => r.RequestStatus == requestsParameters.RequestStatus 
                 && (!String.IsNullOrEmpty(requestsParameters.DoctorId) || requestsParameters.DoctorId == r.DoctorId)
            && (!requestsParameters.Appoitment.HasValue || r.AppoitmentEnum == requestsParameters.Appoitment))
        {
            addInclude(r => r.Patient);
        }

        public RequestsSpecification()
        { }
    }
}
