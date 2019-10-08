using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebHelsi.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebHelsi.Entities;

namespace WebHelsi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        //Підключення до бази даних
        private readonly EFDbContext _context;
        //Створює нових User, оперує User
        private readonly UserManager<DbUser> _userManager;
        //Авторизація і аутентифікація User
        private readonly SignInManager<DbUser> _signInManager;

        //Конструктор контролера
        public AccountController(EFDbContext context,
          UserManager<DbUser> userManager,
          SignInManager<DbUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }
        //Запит авторизації
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    var errrors = CustomValidator.GetErrorsByModel(ModelState);
            //    return BadRequest(errrors);
            //}

            //Запит і перевірка на правильність введення даних
            var result = await _signInManager
                .PasswordSignInAsync(model.Email, model.Password,
                false, false);
            //якщо запит невдалий, повертає BadRequest
            if (!result.Succeeded)
            {
                return BadRequest(new { invalid = "Не правильно введені дані!" });
            }

            //Шукає User з таким Email

            var user = await _userManager.FindByEmailAsync(model.Email);
            //авторизація юзера
            await _signInManager.SignInAsync(user, isPersistent: false);

            //при авторизації створює новий токен
            return Ok(
            new
            {
                token = CreateTokenJwt(user)
            });
        }
        //токен аутентифікації

        private string CreateTokenJwt(DbUser user)
        {
            //показує роль нового юзера
            var roles = _userManager.GetRolesAsync(user).Result;

            //Створили Claim, в яких зберігається id та name User
            var claims = new List<Claim>()
            {
                //new Claim(JwtRegisteredClaimNames.Sub, user.Id)
                new Claim("id", user.Id.ToString()),
                new Claim("name", user.UserName)
            };
            //В claims записуємо ролі
            foreach (var role in roles)
            {
                claims.Add(new Claim("roles", role));
            }
            //Задаємо і налаштовуємо ключ для шифрування Jwt токена
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("11-sdfasdf-22233222222"));
            //Credentials - це дані юзера (ключ і алгоритм шифрування цим ключем HmacSha256)
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // робимо  Jwt токен
            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,//Credentials 
                claims: claims,//загальнодоступні дані
                expires: DateTime.Now.AddHours(1));//час життя токена
            //повертаємо токен у вигляді строки
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}