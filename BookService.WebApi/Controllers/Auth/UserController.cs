using BookService.Infrastructure.Persistence.UnitOfWorks.Custom;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using BookService.JwtAuth;
using System.Linq;
using BookService.WebApi.Infrastructure.Extensions;
using BookService.Domain.Entiies.Users.DTOs;
using BookService.Domain.Entiies.Users.Entity;
using BookService.Domain.Entiies.Users.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookService.WebApi.Controllers.Auth;

public class UserController : Controller
{
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly IUserUnitOfWork _unitOfWork;
    private readonly IJwtGeneratorService _jwtGenerator;
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly IJwtUserManager _userManager;
    public UserController(IUserUnitOfWork unitOfWork, IJwtUserManager userManager, JsonSerializerOptions jsonOptions, IJwtGeneratorService jwtGenerator, JwtConfiguration jwtConfiguration)
    {
        _unitOfWork = unitOfWork;
        _jsonOptions = jsonOptions;
        _jwtGenerator = jwtGenerator;
        _jwtConfiguration = jwtConfiguration;
        _userManager = userManager;
    }

    #region Login
    /// <summary>
    /// Вход в аккаунт, возвращает JWT токен
    /// </summary>
    /// <response code="404">Пользователь не найден</response>
    /// <response code="200">Запрос успешно выполнен</response>
    [HttpPost, Route("/api/net/User/Login")]
    [ProducesResponseType(statusCode: 200, Type = typeof(string))]
    [DTO(typeof(UserLoginModel))]
    public async Task<IActionResult> Login([FromBody] JsonObject data)
    {
        UserLoginModel? model = data.Deserialize<UserLoginModel>(_jsonOptions);
        if (model == null)
        {
            return BadRequest();
        }
        TryValidateModel(model);
        if (ModelState.ErrorCount > 0)
        {
            return BadRequest(ModelState);
        }
        UserEntity? found = await _unitOfWork.FindAsync(model);
        if (found == null)
        {
            ModelState.AddModelError("Password", "Неверный логин или пароль!");
            return Ok(ModelState.ToDictionary(x => x.Key, x => string.Join("\n", x.Value.Errors.Select(x => x.ErrorMessage))));
        }
        found.RefreshToken = Guid.NewGuid().ToString();
        found.RefreshExpiryTime = DateTime.Now.AddMinutes(_jwtConfiguration.RefreshMinutes);
        _unitOfWork.Update(found);
        await _unitOfWork.SaveAsync();
        var jwt = _jwtGenerator.Generate(found.GetGeneratorClaims());
        Response.Cookies.Append("token", jwt, new CookieOptions()
        {
            Expires = found.RefreshExpiryTime
        });
        Response.Cookies.Append("refresh-token", found.RefreshToken, new CookieOptions()
        {
            Expires = found.RefreshExpiryTime,
            HttpOnly = true
        });
        return Ok(new object[]{ jwt, found.RefreshExpiryTime});
    }
    #endregion

    #region List
    /// <summary>
    /// Список пользователей
    /// </summary>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="200">Запрос успешно выполнен</response>
    [HttpGet, Route("/api/net/User/List")]
    [AuthorizeJWT(UserRole.Admin)]
    public async Task<List<UserEntity>> List()
    {
        var list = await _unitOfWork.GetListAsync();
        return list;
    }
    #endregion

    #region Index
    /// <summary>
    /// Конкретный пользователь
    /// </summary>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="401">Ошибка авторизации</response>
    /// <response code="200">Запрос успешно выполнен</response>
    [HttpGet, Route("/User/Index/{id}")]
    [AuthorizeJWT(roles: UserRole.Admin)]
    [ProducesResponseType(statusCode: 200, Type = typeof(UserEntity))]
    public async Task<IActionResult> Index(int id)
    {
        UserEntity? found = await _unitOfWork.FindAsync(id, tracking: false);
        if (found == null)
        {
            return NotFound();
        }
        return Content(JsonSerializer.Serialize(found, _jsonOptions));
    }
    #endregion

    #region Register
    /// <summary>
    /// Список пользователей
    /// </summary>
    /// <response code="400">Ошибка при регистрации</response>
    /// <response code="200">Запрос успешно выполнен</response>
    [HttpPost, Route("/User/Register")]
    [AuthorizeJWT(redirect: true, roles: UserRole.Admin)]
    public async Task<IActionResult> Register([FromForm] UserCreateModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (await _unitOfWork.UserRepository.FindWithLoginAsync(model.Login) != null)
        {
            ModelState.AddModelError("Login", "Пользователь с такой почтой уже существует!");
            return BadRequest(ModelState);
        }
        UserEntity user = new UserEntity(model);
        await _unitOfWork.AddAsync(user);
        await _unitOfWork.SaveAsync();
        return Ok();
    }
    #endregion

    /// <summary>
    /// Изменение пользователя
    /// </summary>
    /// <response code="400">Ошибка при именении</response>
    /// <response code="200">Запрос успешно выполнен</response>
    [HttpPost, Route("User/Edit")]
    [AuthorizeJWT(roles: UserRole.Admin)]
    public async Task<IActionResult> Edit([FromBody] UserEditModel updatedUser)
    {
        var user = await _unitOfWork.UserRepository.FindAsync(updatedUser.Id);

        if (user == null)
            return NotFound("Пользователь не найден.");
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        user.FirstName = updatedUser.FirstName;
        user.SecondName = updatedUser.SecondName;
        user.ThirdName = updatedUser.ThirdName;

        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.SaveAsync();

        return Ok("Данные пользователя обновлены.");
    }




    #region Default
    /// <summary>
    /// СОЗДАНИЕ ДЕФОЛТНОГО ПОЛЬЗОВАТЕЛЯ. ТОЛЬКО ДЛЯ ТЕСТИРОВАНИЯ
    /// Создаёт администратора admin@admin.ru с паролем admin
    /// </summary>
    /// <response code="400">Неприменимо</response>
    /// <response code="200">Запрос успешно выполнен</response>
    [HttpGet, Route("/User/Default")]
    public async Task<IActionResult> Default()
    {
        if ((await _unitOfWork.GetListAsync()).Count > 0)
        {
            return StatusCode(400);
        }
        UserEntity user = new UserEntity()
        {
            Login = "admin@admin.ru",
            PasswordHash = HashingExtensions.ComputeHash("admin@admin.ru", "admin"),
            FirstName = "Тест",
            SecondName = "Тестов",
            ThirdName = "Тестович",
            Role = UserRole.Admin,
        };
        await _unitOfWork.AddAsync(user);
        await _unitOfWork.SaveAsync();
        return Ok();
    }
    #endregion
}
