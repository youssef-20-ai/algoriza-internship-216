using AlgorizaProject.BLL.RequestsSpecifications;
using AlgorizaProject.DAL.Entities;
using AlgorizaProject.Dtos;
using APIDemo.BLL.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Security.Claims;

namespace AlgorizaProject.Controllers
{

    public class DoctorController : BaseController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public DoctorController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager,
            ITokenService tokenService , IUnitOfWork unitOfWork , IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> login([FromBody] LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                return BadRequest();

            var result = await signInManager.CheckPasswordSignInAsync(user , loginDto.Password , false);

            if (result.Succeeded)
            {
                var token = await tokenService.CreateToken(user, userManager);
                return Ok(new
                {
                    email = user.Email,
                    userName = user.UserName,
                    teken = token
                }) ;  
            } 

            return Unauthorized();
                
        }

        [Authorize]
        [HttpPost("AddAppoitment")]
        public async Task<ActionResult<bool>> addAppoitment([FromBody] AppoitmentDto appoitmentDto)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(Email);

            if (user == null) return Unauthorized();

            var repo = unitOfWork.Reposatory<Appoitment>();

            var times = new List<AppoitmentTime>();

            foreach(var time in appoitmentDto.Times)
            {
                var appoitmentTime = new AppoitmentTime()
                {
                    Time = time.Time,
                };
                times.Add(appoitmentTime);
            }

            var appoitment = new Appoitment()
            {
                AppoitmentEnum = appoitmentDto.AppoitmentEnum,
                Times = times,
                DoctorId = user.Id
            };


            repo.Add(appoitment);

            var result = await unitOfWork.Complete();

            if (result != 0) return Ok("appoitment added successfully");

            return BadRequest();

        }

        [Authorize]
        [HttpPut("EditAppoitment")]
        public async Task<ActionResult<bool>> editAppoitment([FromQuery] int appoitmentId , string time ,
            [FromBody] RequestsParameters requestsParameters)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(Email);

            if (user == null) return Unauthorized();

            var spec = new RequestsSpecification(requestsParameters);

            var requests = await unitOfWork.Reposatory<Request>().GetAllWithSpecAsync(spec);

            if (requests.Any()) return BadRequest(new { error = "There is requests for this time" });

            var updatedTime = await unitOfWork.Reposatory<AppoitmentTime>().GetAsync(requestsParameters.TimeId);

            updatedTime.Time = time;

            unitOfWork.Reposatory<AppoitmentTime>().Update(updatedTime);

            var result = await unitOfWork.Complete();

            if (result != 0) return Ok(true);

            return BadRequest();    
        }

        [HttpDelete("DeleteAppoitment")]
        public async Task<ActionResult<bool>> deleteAppoitment([FromQuery] int timeId)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(Email);

            if (user == null) return Unauthorized();

            var requestsParameters = new RequestsParameters()
            {
                TimeId = timeId,
            };

            var spec = new RequestsSpecification(requestsParameters);

            var requests = await unitOfWork.Reposatory<Request>().GetAllWithSpecAsync(spec);

            if (requests.Any()) return BadRequest(new { error = "There is requests for this time" });

            var time = await unitOfWork.Reposatory<AppoitmentTime>().GetAsync(requestsParameters.TimeId);

            unitOfWork.Reposatory<AppoitmentTime>().Delete(time);

            var result = await unitOfWork.Complete();

            if (result != 0) return Ok(true);

            return BadRequest();
        }

        [Authorize]
        [HttpGet("GetAllBooking")]
        public async Task<ActionResult<BookingDto>> getAllBooking()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(Email);

            if (user == null) return Unauthorized();

            var repo = unitOfWork.Reposatory<Request>();

            var requestParameters = new RequestsParameters
            {
                DoctorId = user.Id,
            };

            var spec = new RequestsSpecification(requestParameters);

            var requests = await repo.GetAllWithSpecAsync(spec);

            var bookingDto = mapper.Map<List<BookingDto>>(requests);

            return Ok(bookingDto);    
        }
    }
}
