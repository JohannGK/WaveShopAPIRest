using Microsoft.AspNetCore.Mvc;
using WaveShopAPIRest.Models;

namespace WaveShopAPIRest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private WaveShopContext DbContext { get; set; }
    public CategoriesController(WaveShopContext dbContext)
    {
        DbContext = dbContext;
    }

    [HttpGet]
    public List<Category> GetCategoriesAsync()
    {
        return DbContext.Categories.ToList();
    }

    [HttpGet("detail/{id}")]
    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await DbContext.Categories.FindAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> CreateCategoryAsync(Category category)
    {
        using (var transaction = DbContext.Database.BeginTransaction())
        {
            try
            {
                Category result;
                CheckNameUnique(category.Name, string.Empty);
                await DbContext.Categories.AddAsync(result = new Category()
                {
                    Name = category.Name,
                    Description = category.Description,
                    Status = "Visible"
                });
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

    [HttpPut("{id}")]
    public async Task<ActionResult<Category>> UpdateCategoryAsync(int id, Category category)
    {
        if (DbContext.Categories.Any(c => c.Id == id))
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var result = await DbContext.Categories.FindAsync(id);
                    CheckNameUnique(category.Name, result.Name);
                    result.Description = category.Description;
                    result.Status = category.Status;
                    result.Name = category.Name;
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



    private void CheckNameUnique(string name, string currentName)
    {
        if (name != currentName)
            if (DbContext.Categories.Any(c => c.Name.ToLower() == name.ToLower()))
                throw new Exception("El nombre de la categoría ya ha sido usada.");
    }
}