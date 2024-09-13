using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolConnect_RepositoryLayer.Interfaces;

namespace SchoolConnect_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        private readonly ISignIn _signInRepo;

        public SignInController(ISignIn signIn)
        {
            _signInRepo = signIn;
        }

        [HttpPost("SignIn")]
        public IActionResult SignInWithEmailAndPassword(string email, string password)
        {
            try
            {
                var result = _signInRepo.SignInAsync(email, password).Result;
                var success = result.GetValueOrDefault("Success") ?? throw new Exception("Could not find the Success Key");

                if (!(bool)success)
                    return BadRequest(result.GetValueOrDefault("ErrorMessage") ?? "Operation failed. Error: Could not retrieve the ErrorMessage.");

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
                _signInRepo.SignOutAsync().Wait();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
