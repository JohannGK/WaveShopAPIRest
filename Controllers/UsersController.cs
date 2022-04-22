using Microsoft.AspNetCore.Mvc;
using WaveShopAPIRest.Models;

namespace WaveShopAPIRest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private WaveShopContext DbContext { get; set; }
    public UsersController(WaveShopContext dbContext)
    {
        DbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        return DbContext.Users.OrderBy(u => u.UserName).ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserAsync(int id)
    {
        var result = await DbContext.Users.FindAsync(id);
        return result == null ? NotFound() : result;
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUserAsync(User user)
    {
        User newUser;
        try
        {
            CheckUserNameUnique(user.UserName, string.Empty);
            CheckEmailUnique(user.Email, string.Empty);
            await DbContext.Users.AddAsync(newUser = new Models.User()
            {
                Email = user.Email,
                UserName = user.UserName,
                Password = user.Password,
                Phone = user.Phone,
                Description = user.Description,
                Status = "Offline",
                BirthDay = user.BirthDay,
                Age = user.Age,
                UerType = user.UerType,
                Reputation = user.Reputation,
                LastLogin = DateTime.Now,
                LastUpdate = DateTime.Now
            });
            var result = await DbContext.SaveChangesAsync();
            return newUser;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<User>> UpdateTodoItem(int id, User newUser)
    {
        if (DbContext.Users.Any(u => u.Id == id))
        {
            try
            {
                var result = await DbContext.Users.FindAsync(id);
                CheckUserNameUnique(newUser.UserName, result.UserName);
                CheckEmailUnique(newUser.Email, result.Email);
                result.Email = newUser.Email;
                result.UserName = newUser.UserName;
                result.Password = newUser.Password;
                result.Phone = newUser.Phone;
                result.Description = newUser.Description;
                result.Status = newUser.Status;
                result.BirthDay = newUser.BirthDay;
                result.Age = newUser.Age;
                result.UerType = newUser.UerType;
                result.Reputation = newUser.Reputation;
                result.LastLogin = DateTime.Now;
                result.LastUpdate = DateTime.Now;
                var resultQuery = await DbContext.SaveChangesAsync();
                return resultQuery >= 0 ? result : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        else
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(int id)
    {
        try
        {
            var result = await DbContext.Users.FindAsync(id);
            if (result == null)
                return NotFound();
            result.Status = "Banned";
            await DbContext.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    private void CheckUserNameUnique(string userName, string currentUserName)
    {
        if (userName != currentUserName)
            if (DbContext.Users.Any(u => u.UserName == userName))
                throw new Exception("El nombre de usuario ya ha sido utilizado por otro usuario.");
    }

    private void CheckEmailUnique(string email, string currentEmail)
    {
        if (email != currentEmail)
            if (DbContext.Users.Any(u => u.Email == email))
                throw new Exception("El email ya ha sido utilizado por otro usuario.");
    }
}