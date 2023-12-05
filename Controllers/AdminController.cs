using AlgorizaProject.BLL.RequestsSpecifications;
using AlgorizaProject.DAL.Entities;
using AlgorizaProject.Dtos;
using APIDemo.BLL.Interface;
using APIDemo.DAL.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.WebSockets;

namespace AlgorizaProject.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<AppUser> userManager;

        public AdminController(IUnitOfWork unitOfWork , UserManager<AppUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        [HttpGet("NumberOfDoctors")]
        public async Task<ActionResult<int>> getNumberODoctors()
        {
            var result = await unitOfWork.Reposatory<Doctor>().GetAllAsync();
            return Ok(result.Count());
        }

        [HttpGet("NumberOfPatients")]
        public async Task<ActionResult<int>> getNumberOfPatients()
        {
            var result = await unitOfWork.Reposatory<Patient>().GetAllAsync();
            return Ok(result.Count());
        }

        [HttpPost("NumberOfRequests")]
        public async Task<ActionResult<int>> getNumberOfRequests(RequestsParameters requestsParametersDto)
        {
            var spec = new RequestsSpecification(requestsParametersDto);
            var result = await unitOfWork.Reposatory<Request>().GetAllWithSpecAsync(spec);
            return Ok(result.Count());
        }

        [HttpPost("AddDoctor")]
        public async Task<ActionResult<bool>> addDoctor([FromBody] DoctorDto doctorDto)
        {
            var user = await userManager.FindByEmailAsync(doctorDto.Email);
            if (user != null)
                return new BadRequestResult();
            var appUser = new Doctor()
            {
                Email = doctorDto.Email,
                UserName = doctorDto.Email.Split("@")[0],
                PhoneNumber = doctorDto.PhoneNumber,
                Gender = doctorDto.Gender,
                DateOfBirth = doctorDto.DOB,
                FirstName = doctorDto.FirstName,    
                LastName = doctorDto.LastName,
            };

            var result = await userManager.CreateAsync(appUser, doctorDto.Password);

            if (!result.Succeeded)
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

            return Ok(true);
        }

        [HttpPut("EditDoctor")]
        public async Task<ActionResult<bool>> editDoctor([FromBody] DoctorDto doctorDto)
        {
            var doctor = await userManager.FindByIdAsync(doctorDto.DoctorId);

            if (doctor == null)
                return BadRequest(new { errors = "doctor not found" });

            doctor.PhoneNumber = doctorDto.PhoneNumber; 
            doctor.Email = doctorDto.Email; 
            doctor.Gender = doctorDto.Gender;
            doctor.FirstName = doctorDto.FirstName;
            doctor.LastName = doctorDto.LastName;
            doctor.DateOfBirth = doctorDto.DOB;

            var result =  await userManager.UpdateAsync(doctor);

            if(result.Succeeded)
                return Ok(result);

            return BadRequest();

        }

        [HttpDelete("DeleteDoctor")]
        public async Task<ActionResult<bool>> deleteDoctor([FromQuery] string Id)
        {
            var doctor = await userManager.FindByIdAsync(Id);

            if (doctor == null)
                return BadRequest(new { error = "Doctor not found" });

            var result = await unitOfWork.Reposatory<Request>().GetAllAsync();

            var numOfRequests = result.Where(r => r.DoctorId == doctor.Id).Any();

            if (numOfRequests)
                return BadRequest(new { error = "Doctor have requests" });

            var deletionResult = await userManager.DeleteAsync(doctor);

            if (deletionResult.Succeeded)
                return Ok(result);

            return BadRequest();    
        }

        [HttpGet("GetAllDoctors")]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> getDoctors()
        {
            var doctors = await unitOfWork.Reposatory<Doctor>().GetAllAsync();

            return Ok(doctors); 
        }


        [HttpGet("GetDoctorById")]
        public async Task<ActionResult<DoctorDto>> getDoctor([FromQuery] string Id)
        {
            var doctor = await userManager.FindByIdAsync(Id);

            if (doctor == null) return BadRequest(new {error = "Doctor not found"});  

            return Ok(doctor);
        }

        /*[HttpGet("GetAllPatients")]
        public async Task<ActionResult<IEnumerable<AppUser>>> getAllPatient()
        {
            var patients = userManager.Users.Where(u => u.UserEnum == UserEnum.Patient); 
            
            return Ok(patients);
        }*/

        /*[HttpGet("GetPatientById")]
        public async Task<ActionResult<AppUser>> getPatient([FromQuery] string Id)
        {
            var patient = userManager.Users.Where(u => u.UserEnum == UserEnum.Patient && u.Id == Id);

            return Ok(patient);
        }*/

        [HttpPost("AddDiscountCode")]
        public async Task<ActionResult<bool>> addDiscountCode([FromBody] DiscountDto discountDto)
        {
            var discount = new Discount()
            {
                DiscountCode = discountDto.Code,
                RequestsCompleted = discountDto.NumberOfRequests,
                Value = discountDto.Value
            };

            unitOfWork.Reposatory<Discount>().Add(discount);

            var result = await unitOfWork.Complete();

            if(result == 1)
                return Ok(true);

            return BadRequest();
        }

        [HttpPut("EditDiscountCode")]
        public async Task<ActionResult<bool>> editDiscount([FromBody] DiscountDto discountDto) 
        {
            var discount = await unitOfWork.Reposatory<Discount>().GetAsync(discountDto.Id);

            if(discount == null) return BadRequest();   

            discount.Value = discountDto.Value;
            discount.DiscountCode = discountDto.Code;
            discount.DiscountEnum = discountDto.DiscountEnum;
            discount.RequestsCompleted = discountDto.NumberOfRequests;

            unitOfWork.Reposatory<Discount>().Update(discount);

            var result = await unitOfWork.Complete();

            if (result == 1) return Ok(true);

            return BadRequest();
        }

        [HttpDelete("DeleteDiscountCode")]
        public async Task<ActionResult<bool>> deleteDiscountCode([FromQuery] int Id)
        {
            var discount = await unitOfWork.Reposatory<Discount>().GetAsync(Id);

            if(discount == null) return BadRequest(new { error = "the discount code not found" });   
            
            unitOfWork.Reposatory<Discount>().Delete(discount);

            var result = await unitOfWork.Complete();

            if (result == 1) return Ok(true);

            return BadRequest();

        }

        [HttpPut("ChangeStatusOfDiscountCode")]
        public async Task<ActionResult<bool>> changeStatusOfDiscountCode(
            [FromQuery] DiscountAcivation discountAcivation , int Id)
        {
            var discount = await unitOfWork.Reposatory<Discount>().GetAsync(Id);

            if (discount == null) return BadRequest(new {error = "discount code not found"});

            discount.DiscountStatus = discountAcivation;

            unitOfWork.Reposatory<Discount>().Update(discount);

            var result = await unitOfWork.Complete();

            if (result == 1) return Ok(true);

            return BadRequest();    
        }
    }
}
