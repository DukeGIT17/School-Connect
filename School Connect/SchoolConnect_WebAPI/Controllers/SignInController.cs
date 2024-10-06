using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_ServiceLayer.IServerSideServices;

namespace SchoolConnect_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        private readonly ISignInService _signInService;
        private Dictionary<string, object> _returnDictionary;

        public SignInController(ISignInService signIn)
        {
            _signInService = signIn;
            _returnDictionary = [];
        }

        [HttpPost("SignIn")]
        public IActionResult SignInWithEmailAndPassword(LoginModel model)
        {
            try
            {
                _returnDictionary = _signInService.SignInAsync(model).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(nameof(SetNewPassword))]
        public IActionResult SetNewPassword(LoginModel model)
        {
            try
            {
                _returnDictionary = _signInService.SetNewPasswordAsync(model).Result;
                if (!(bool)_returnDictionary["Success"]) return BadRequest(_returnDictionary["ErrorMessage"]);
                return Ok(_returnDictionary);
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
