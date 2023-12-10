using AlgorizaProject.BLL.Sepecifications;
using AlgorizaProject.DAL.Entities;
using APIDemo.BLL.Sepecifications;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorizaProject.BLL.DoctorSpecifications
{
    public class DoctorSpecification : BaseSpecification<Doctor>
    {
        public DoctorSpecification(BaseSpecificationParams doctorSpecificationParams) :
            base(p => p.FirstName == "Hello" && p.FirstName != "Hello")
        {
            applyPaging(doctorSpecificationParams.PageSize * (doctorSpecificationParams.PageIndex - 1), doctorSpecificationParams.PageSize);
        }
    }
}
