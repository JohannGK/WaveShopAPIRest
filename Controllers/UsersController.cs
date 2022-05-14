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
    public List<User> GetUsers()
    {
        return DbContext.Users.ToList();
    }

    [HttpGet("{userName}")]
    public User? GetUser(string userName)
    {
        return DbContext.Users.FirstOrDefault(u => u.UserName == userName);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUserAsync(User user)
    {
        using (var transaction = DbContext.Database.BeginTransaction())
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
                    UserType = user.UserType,
                    Reputation = user.Reputation,
                    LastLogin = DateTime.Now,
                    LastUpdate = DateTime.Now
                });
                await DbContext.SaveChangesAsync();
                await DbContext.ShoppingCarts.AddAsync(new ShoppingCart() { productsQuantity = 0, subtotal = 0, LastUpdate = DateTime.Now, IdUser = newUser.Id });
                await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return newUser;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<User>> UpdateUserAsync(int id, User newUser)
    {
        if (DbContext.Users.Any(u => u.Id == id))
        {
            using (var transaction = DbContext.Database.BeginTransaction())
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
                    result.UserType = newUser.UserType;
                    result.Reputation = newUser.Reputation;
                    result.LastLogin = DateTime.Now;
                    result.LastUpdate = DateTime.Now;
                    await DbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return result;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(ex.Message);
                }
            }
        }
        else
        {
            return NotFound();
        }
    }

    private void CheckUserNameUnique(string userName, string currentUserName)
    {
        if (userName != currentUserName)
            if (DbContext.Users.Any(u => u.UserName.ToLower() == userName.ToLower()))
                throw new Exception("El nombre de usuario ya ha sido utilizado por otro usuario.");
    }

    private void CheckEmailUnique(string email, string currentEmail)
    {
        if (email != currentEmail)
            if (DbContext.Users.Any(u => u.Email.ToLower() == email.ToLower()))
                throw new Exception("El email ya ha sido utilizado por otro usuario.");
    }
}