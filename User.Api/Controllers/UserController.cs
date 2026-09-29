
using Common.ApiModels.Access.Request;
using Common.ApiModels.User;
using Common.Models.Token;
using Common.Services.Email;
using Common.Services.User;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Rabbit.Bilboard;
using System.Net.Mail;
using System.Threading.Tasks;
using User.Api.Services.Authentication;

namespace User.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : AuthroizedController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IEmailService _emailService;

        public UserController(IAuthenticationService authenticationService, IUserService userService, IPublishEndpoint publishEndpoint, IEmailService emailService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _publishEndpoint = publishEndpoint;
            _emailService = emailService;
        }

        /// <summary>
        /// Zalogowanie użytkownika
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Zalogowanie użytkownika</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<JwtTokenResponse>> Login([FromBody] LoginRequest request)
        {
            return Ok(await _authenticationService.Login(request));
        }

        /// <summary>
        /// Wylogowanie użytkownika
        /// </summary>
        /// <returns>Wylogowanie użytkownika</returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.Logout();
            return Ok("Pomyślnie wylogowano użytkownika");
        }

        /// <summary>
        /// Rejestracja użytkownika
        /// </summary>
        /// <returns>Rejestracja użytkownika</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
        {
            var userId = await _userService.Create(request);

            await _publishEndpoint.Publish(new CreateBilboardItemEvent()
            {
                UserId = userId
            });

            List<MailAddress> mailAddresses = [new MailAddress("dawid96.12mroczkowski@gmail.com")];

            //Dictionary<string, string> bodyParmas = new()
            //{
            //    { "UserName","emailll" },
            //    { "Password", "haslooo" }
            //};

            //Dictionary<string, string> subjectParmas = new()
            //{
            //    { "UserName","emailll" },
            //};

           // await _emailService.SendEmail(mailAddresses, "NewUser", true, Culture, new MailAddress("dawid96.12mroczkowski@gmail.com"));

            return Ok("Pomyślnie wylogowano użytkownika");
        }

        /// <summary>
        /// Wylogowanie użytkownika
        /// </summary>
        /// <returns></returns>
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            return Ok(AuthorizedUser);
        }
    }
}
