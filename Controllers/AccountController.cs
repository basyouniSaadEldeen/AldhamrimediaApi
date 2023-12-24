using AldhamrimediaApi.Dto.UserDto;
using AldhamrimediaApi.Dtos.AcountDto;
using AldhamrimediaApi.Dtos.UserDto;
using AldhamrimediaApi.Models;
using AldhamrimediaApi.Service.interfaces;
using HealthCare.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AldhamrimediaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        //private readonly IEmailSender _emailSender;

        private readonly IAccountRepository AccountRepository;
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<User> userManager;
        public AccountController( IAccountRepository accountRepository
            , RoleManager<Role> roleManager, UserManager<User> userManager )
        {
          
            AccountRepository = accountRepository;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser( [FromForm] RegistraionUserDto model, string roleName = "User")
        {
            try
            {
                var result = await AccountRepository.RegesterUserAsync(model, roleName);
                if (!result.IsAuthenticated)
                {
                    return BadRequest(result);
                }

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            };


        }


        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser(LoginUserDto model)
        {
            try
            {
                var result = await AccountRepository.LoginUserAsync(model);
                if (!result.IsAuthenticated)
                {
                    return BadRequest(result);
                }

                return Ok(result);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            };


        }

        //[HttpGet("LoginUserUsingGoogle")]
        //[Authorize]
        //public ActionResult<string> LoginUserUsingGoogle(int id)
        //{
        //    var user = this.User;
        //    return "value";


        //}
        [HttpGet("UserProfile")]
        public async Task<IActionResult> UserProfile()
        {
            var userEmail = userManager.GetUserId(HttpContext.User);
            if (userEmail == null)
            {
                return BadRequest("you should login");
            }
            var userId = userManager.Users.Where(x => x.Email == userEmail).Select(x => x.Id).FirstOrDefault();
            var result = await AccountRepository.GetProfileAsync(userId);
            if (result == null)
            {
                return BadRequest("no User found");

            }
            return Ok(result);
        }
        [HttpGet("User_Records")]
        public async Task<IActionResult> User_Records()
        {
            var userEmail = userManager.GetUserId(HttpContext.User);
            if (userEmail == null)
            {
                return BadRequest("you should login");
            }
            var userId = userManager.Users.Where(x => x.Email == userEmail).Select(x => x.Id).FirstOrDefault();
            var result = await AccountRepository.My_Records(userId);
            if (result == null)
            {
                return BadRequest("no User found");

            }
            return Ok(result);
        }
        [HttpPost("buy_Service")]
        public async Task <IActionResult> Buy_service(Buy_Service_Dto model)
        {
            var userEmail = userManager.GetUserId(HttpContext.User);
            if (userEmail == null)
            {
                return BadRequest("you should login");
            }
            var userId = userManager.Users.Where(x => x.Email == userEmail).Select(x => x.Id).FirstOrDefault();
            var result = await AccountRepository.buyService(model,userId);
            if (result == null)
            {
                return BadRequest("no User found");

            }
            return Ok(result);
        }
        [HttpPost("Recharge_wallet")]
        public async Task<IActionResult> Recharge_Wallet(decimal amount)
        {
            var userEmail = userManager.GetUserId(HttpContext.User);
            if (userEmail == null)
            {
                return BadRequest("you should login");
            }
            var userId = userManager.Users.Where(x => x.Email == userEmail).Select(x => x.Id).FirstOrDefault();
            var result = await AccountRepository.Recharge_Wallet( userId, amount);
            if (result == null)
            {
                return BadRequest("no User found");

            }
            return Ok(result);
        }

        [HttpGet("UserNotification")]
        public async Task<IActionResult> GetNotificationAsync()
        {
            var userEmail = userManager.GetUserId(HttpContext.User);
            if (userEmail == null)
            {
                return BadRequest("you should login");
            }
            var userId = userManager.Users.Where(x => x.Email == userEmail).Select(x => x.Id).FirstOrDefault();
            var result = await AccountRepository.GetNotificationAsync(userId);
            if (result == null)
            {
                return BadRequest("not found");

            }
            return Ok(result);
        }


        //[HttpGet]
        //public IActionResult SendEmail()
        //{
        //   _emailSender.SendEmail("rodger.thiel@ethereal.email", "A7A Regitered");
        //    //await  _emailSender.SendEmailAsync("basyounisaad8@gmail.com", "Test Email", "this is email test");
        //    return Ok();
        //}
    }
}
