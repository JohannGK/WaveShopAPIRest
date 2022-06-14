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
    public ActionResult GetProducts(int category, string name)
    {
        List<Product> value = new List<Product>();
        var products = DbContext.Products.Where(p => p.Name.ToLower().Contains(name.ToLower()));
        if (products != null)
        {
            value = products.ToList();
            if (category != -1)
                value = products.ToList().Where(p => p.IdCategory == category).ToList();
        }
        value.ForEach(p => p.Product_Images = DbContext.Product_Images.Where(i => i.IdProduct == p.Id).ToArray());

        return new JsonResult(value);
    }

    [HttpGet("{id}")]
    public ActionResult GetProduct(int id)
    {
        var product = DbContext.Products.Find(id);
        product.Product_Images = DbContext.Product_Images.Where(i => i.IdProduct == product.Id).ToArray();
        if (product == null)
            return NotFound();
        else
            return new JsonResult(product);
    }

    [HttpPost]
    public async Task<ActionResult> AddProductAsync(Product product)
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
                    CommentsNumber = 0,
                    LastUpdate = DateTime.Now,
                    VendorUsername = DbContext.Users.Find(product.IdVendor).UserName
                });
                await DbContext.SaveChangesAsync();

                product.Product_Images.ToList().ForEach(i =>
                {
                    DbContext.Product_Images.AddAsync(new Product_Image()
                    {
                        Url = i.Url,
                        LastUpdate = DateTime.Now,
                        IdProduct = newProduct.Id
                    });
                });

                await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new JsonResult(newProduct);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new JsonResult(new { error = ex.Message }));
            }
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateProductAsync(int id, Product product)
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
                    result.VideoAddress = product.VideoAddress;
                    result.StockQuantity = product.StockQuantity;
                    result.UnitPrice = product.UnitPrice;
                    result.Status = product.Status;
                    result.Country = product.Country;
                    result.Location = product.Location;
                    result.IdCategory = product.IdCategory;
                    result.LastUpdate = DateTime.Now;
                    await DbContext.SaveChangesAsync();

                    DbContext.Product_Images.RemoveRange(DbContext.Product_Images.Where(i => i.IdProduct == id));
                    await DbContext.SaveChangesAsync();

                    product.Product_Images.ToList().ForEach(i =>
                    {
                        DbContext.Product_Images.AddAsync(new Product_Image()
                        {
                            Url = i.Url,
                            LastUpdate = DateTime.Now,
                            IdProduct = id
                        });
                    });

                    await DbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new JsonResult(result);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new JsonResult(new { error = ex.Message }));
                }
            }
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost("{idUser}/{idProduct}")]
    public async Task<ActionResult> AddFavoriteProductAsync(int idUser, int idProduct)
    {
        using (var transaction = DbContext.Database.BeginTransaction())
        {
            try
            {
                Favorite? result;
                await DbContext.Favorites.AddAsync(result = new Favorite()
                {
                    IdProduct = idProduct,
                    IdUser = idUser,
                    Creation = DateTime.Now
                });
                await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new JsonResult(new { error = ex.Message }));
            }
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