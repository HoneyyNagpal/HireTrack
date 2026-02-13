using HireTrack.Core.DTOs;
using HireTrack.Services;
using Microsoft.AspNetCore.Mvc;

namespace HireTrack.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var result = await _authService.LoginAsync(req);
            if (result == null)
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            var success = await _authService.RegisterAsync(req);
            if (!success)
                return Conflict(new { message = "Email already in use" });

            return StatusCode(201, new { message = "User registered successfully" });
        }
    }
}
```

---

### **COMMIT 7 — Candidate Service + Controller**
```
git commit -m "add candidate service and REST endpoints"
