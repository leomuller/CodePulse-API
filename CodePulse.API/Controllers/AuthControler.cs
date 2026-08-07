using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace CodePulse.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	public class AuthControler : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly ITokenRepository _tokenRepository;

		public AuthControler(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
		{
			this._userManager = userManager;
			this._tokenRepository = tokenRepository;
		}

		// POST: {apibaseurl}/api/auth/register
		[HttpPost]
		[Route("register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
		{
			//create identityuser object
			var user = new IdentityUser
			{
				UserName = request.Email?.Trim(),
				Email = request.Email?.Trim()

			};

			//create user
			var identityResult = await _userManager.CreateAsync(user, request.Password);

			if (identityResult.Succeeded) {
				//assign roles:
				var addRoleResult = await _userManager.AddToRoleAsync(user, "Reader");
				if (addRoleResult.Succeeded)
				{
					return Ok();
				}
				else
				{
					if (addRoleResult.Errors.Any())
					{
						foreach (var error in addRoleResult.Errors)
						{
							ModelState.AddModelError("", error.Description);
						}
					}
				}
			}
			else
			{
				if (identityResult.Errors.Any())
				{
					foreach (var error in identityResult.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
			}

			return ValidationProblem(ModelState);
		}

		// POST: {apibaseurl}/api/auth/login
		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
		{
			var identityUser = await _userManager.FindByEmailAsync(request.Email);
			if(identityUser != null)
			{
				bool checkPasswordResult = await _userManager.CheckPasswordAsync(identityUser, request.Password);

				if(checkPasswordResult == true)
				{
					//create token.
					var roles = await _userManager.GetRolesAsync(identityUser);
					var jwtToken = _tokenRepository.CreateJwtToken(identityUser, roles.ToList());

					var response = new LoginResponseDto()
					{
						Email = request.Email,
						Token = jwtToken,
						Roles = roles.ToList(),
					};


					return Ok(response);
				}
				
			}

			ModelState.AddModelError("", "Email or password is incorrect");
			return ValidationProblem(ModelState);
		}

	}
}
