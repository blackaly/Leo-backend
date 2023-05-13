using Leo.Model;
using Leo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Leo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!model.Password.Equals(model.ConfirmPassword)) return BadRequest("Password not matched");

            var res = await _authService.Register(model);

            if (!res.IsAuthenticated) { return BadRequest(res.Message); }

            return Ok(res);
        }

        [HttpPost("Login")]
        [AllowAnonymous]

        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var res = await _authService.Login(model);

            if (!res.IsAuthenticated) return BadRequest(res.Message);

            return Ok(res);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            bool done = await _authService.RemoveUser(id);

            if (done) return Ok();
            return BadRequest();
        }
    }
}
