using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_ServiceLayer.ISystemAdminServices;

namespace SchoolConnect_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        private readonly ISignInService _signInService;

        public SignInController(ISignInService signIn)
        {
            _signInService = signIn;
        }

        [HttpPost("SignIn")]
        public IActionResult SignInWithEmailAndPassword(LoginModel model)
        {
            try
            {
                var result = _signInService.SignInAsync(model).Result;
                var success = result["Success"];

                if (!(bool)success)
                    return BadRequest(result["ErrorMessage"]);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(nameof(SignOut))]
        public new IActionResult SignOut()
        {
            try
            {
                _signInService.SignOutAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
