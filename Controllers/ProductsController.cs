using Microsoft.AspNetCore.Mvc;
using WaveShopAPIRest.Models;

namespace WaveShopAPIRest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private WaveShopContext DbContext { get; set; }
    public ProductsController(WaveShopContext dbContext)
    {
        DbContext = dbContext;
    }

    [HttpGet("{category}/{name}")]
    public List<Product> GetProducts(int category, string name)
    {
        if (category == -1)
            return DbContext.Products.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();
        else
            return DbContext.Products.Where(p => p.IdCategory == category && p.Name == name).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<Product>> AddProductAsync(Product product)
    {
        using (var transaction = DbContext.Database.BeginTransaction())
        {
            try
            {
                Product newProduct;
                CheckProductNameUnique(product.Name, string.Empty);
                CheckExistVentor(product.IdVendor);
                await DbContext.Products.AddAsync(newProduct = new Product()
                {
                    Name = product.Name,
                    Description = product.Description,
                    PhotoAddress = product.PhotoAddress,
                    VideoAddress = product.VideoAddress,
                    StockQuantity = product.StockQuantity,
                    UnitPrice = product.UnitPrice,
                    Status = "Visible",
                    Published = DateTime.Now,
                    Country = product.Country,
                    Location = product.Location,
                    IdCategory = product.IdCategory,
                    IdVendor = product.IdVendor,
                    LikesNumber = 0,
                    DislikesNumber = 0,
                    ShoppedTimes = 0,
                    CommentsNumber = 0
                });
                await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return newProduct;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex.Message);
            }
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Product>> UpdateProductAsync(int id, Product product)
    {
        if (DbContext.Products.Any(p => p.Id == id))
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var result = await DbContext.Products.FindAsync(id);
                    CheckProductNameUnique(product.Name, result.Name);
                    result.Name = product.Name;
                    result.Description = product.Description;
                    result.PhotoAddress = product.PhotoAddress;
                    result.VideoAddress = product.VideoAddress;
                    result.StockQuantity = product.StockQuantity;
                    result.UnitPrice = product.UnitPrice;
                    result.Status = product.Status;
                    result.Country = product.Country;
                    result.Location = product.Location;
                    result.IdCategory = product.IdCategory;
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

    private void CheckProductNameUnique(string productName, string currentProductName)
    {
        if (productName != currentProductName)
            if (DbContext.Products.Any(p => p.Name.ToLower() == productName.ToLower()))
                throw new Exception("El nombre del producto ya ha sido utilizado.");
    }

    private void CheckExistVentor(int vendorId)
    {
        User? user;
        if ((user = DbContext.Users.Find(vendorId)) == null)
            throw new Exception("El usuario que vende el artículo no se encontró en el sistema");
        else
            if (user.Reputation == "Bad")
            throw new Exception("El usuario no puede vender un producto debido a su reputación");
    }
}