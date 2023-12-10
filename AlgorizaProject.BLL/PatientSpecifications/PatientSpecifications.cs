using AlgorizaProject.BLL.Sepecifications;
using AlgorizaProject.DAL.Entities;
using APIDemo.BLL.Sepecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorizaProject.BLL.PatientSpecifications
{
    public class PatientSpecifications : BaseSpecification<Patient>
    {
        public PatientSpecifications(BaseSpecificationParams patientSpecificationParams) :
            base(P => string.IsNullOrEmpty(patientSpecificationParams.Search) || P.FirstName.ToLower().Contains(patientSpecificationParams.Search))
        {
            applyPaging(patientSpecificationParams.PageSize * (patientSpecificationParams.PageIndex - 1), patientSpecificationParams.PageSize);
        }
    }
}
