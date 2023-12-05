using AlgorizaProject.DAL.Entities;
using AlgorizaProject.Dtos;
using APIDemo.BLL.Interface;
using APIDemo.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace AlgorizaProject.Controllers
{

    public class PatientController : BaseController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IUnitOfWork unitOfWork;

        public PatientController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager , 
            ITokenService tokenService , IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var user = await userManager.FindByEmailAsync(registerDto.Email);

            if (user != null)
                return BadRequest(new { erorr = "email adress ia already used"});

            var patient = new Patient()
            {
                Email = registerDto.Email,
                UserName = registerDto.Email.Split("@")[0],
                PhoneNumber = registerDto.PhoneNumber,
                DateOfBirth = registerDto.DOB,
                FirstName = registerDto.FirstName,  
                LastName = registerDto.LastName,
            };

            var result = await userManager.CreateAsync(patient, registerDto.Password);

            if (!result.Succeeded)
                return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });

            var userDto = new 
            {
                Email = registerDto.Email,
                DisplayName = $"{registerDto.DisplayName}",
                token = await tokenService.CreateToken(patient, userManager)
            };
            return Ok(userDto);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> login([FromBody] LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                return BadRequest();

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (result.Succeeded)
            {
                var token = await tokenService.CreateToken(user, userManager);
                return Ok(new
                {
                    email = user.Email,
                    userName = user.UserName,
                    teken = token
                });
            }

            return Unauthorized();

        }

        [HttpGet("GetDoctors")]
        public async Task<ActionResult> getDoctors()
        {
            var doctors = await unitOfWork.Reposatory<Doctor>().GetAllAsync();

            return Ok(doctors);
        }

        [Authorize]
        [HttpPost("BookAppoitment")]
        public async Task<ActionResult<bool>> bookAppoitment([FromBody] BookAppoitmentDto bookAppoitmentDto)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(Email);

            if (user == null) return Unauthorized();

            var repo = unitOfWork.Reposatory<Request>();

            var time = await unitOfWork.Reposatory<AppoitmentTime>().GetAsync(bookAppoitmentDto.TimeId);

            var day = await unitOfWork.Reposatory<Appoitment>().GetAsync(bookAppoitmentDto.AppoitmentId);

            var request = new Request
            {
                DoctorId = bookAppoitmentDto.DoctorId,
                PatientId = user.Id,
                DiscountCode = bookAppoitmentDto.DiscountCode,
                RequestStatus = RequestStatus.Pending,
                Time = time.Time,
                AppoitmentEnum = day.AppoitmentEnum
            };

            repo.Add(request);

            var result = await unitOfWork.Complete();

            if(result != 0) return Ok(result);

            return BadRequest();
        }
    }
}
