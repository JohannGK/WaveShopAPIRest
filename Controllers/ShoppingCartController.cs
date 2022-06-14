using Microsoft.AspNetCore.Mvc;
using WaveShopAPIRest.Models;

namespace WaveShopAPIRest.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ShoppingCartController : ControllerBase
{
    private WaveShopContext DbContext { get; set; }

    public ShoppingCartController(WaveShopContext dbContext)
    {
        DbContext = dbContext;
    }

    [HttpGet("{IdUser}")]
    public ActionResult GetShoppingCart(int idUser)
    {
        List<Product> value = new List<Product>();
        var shoppingCart = DbContext.ShoppingCarts.Where(s => s.IdUser == idUser);
        var productSelectedCart = DbContext.ProductSelectedCarts.Where(p => p.IdShoppingCart == shoppingCart.First().id).ToList();
        foreach (var p in productSelectedCart)
        {
            Product? product = DbContext.Products.Find(p.IdProduct);
            if (product != null)
            {
                product.StockQuantity = p.Quantity;
                product.UnitPrice *= p.Quantity;
                value.Add(product);
            }
        }
        value.ForEach(p => p.Product_Images = DbContext.Product_Images.Where(i => i.IdProduct == p.Id).ToArray());
        return new JsonResult(value);
    }

    [HttpPost("{idClient}")]
    public async Task<ActionResult> AddToShoppingCartAsync(int idClient, Product product)
    {
        using (var transaction = DbContext.Database.BeginTransaction())
        {
            try
            {
                await CheckProductQuantity(product.Id, product.StockQuantity);
                ProductSelectedCart result;
                await DbContext.ProductSelectedCarts.AddAsync(result = new ProductSelectedCart()
                {
                    Price = product.UnitPrice * product.StockQuantity,
                    Quantity = product.StockQuantity,
                    Status = "Solicited",
                    IdProduct = product.Id,
                    IdShoppingCart = DbContext.ShoppingCarts.FirstOrDefault(c => c.IdUser == idClient).id
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

    [HttpDelete("{idUser}/{idProduct}")]
    public async Task<ActionResult> RemoveToShoppingCartAsync(int idUser, int idProduct)
    {
        using (var transaction = DbContext.Database.BeginTransaction())
        {
            try
            {
                var cart = DbContext.ShoppingCarts.FirstOrDefault(c => c.IdUser == idUser);
                var productSelected = DbContext.ProductSelectedCarts.FirstOrDefault(c => c.IdShoppingCart == cart.id && c.IdProduct == idProduct );
                DbContext.ProductSelectedCarts.Remove(DbContext.ProductSelectedCarts.Find(productSelected.Id));
                await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new JsonResult("Ok");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new JsonResult(new { error = ex.Message }));
            }
        }
    }

    private async Task<ActionResult<bool>> CheckProductQuantity(int idProduct, int cantidad)
    {
        Product? product = await DbContext.Products.FindAsync(idProduct);
        if (product != null)
        {
            if (product.StockQuantity < cantidad)
                throw new Exception("No hay suficiente cantidad del producto seleccionado.");
        }
        else
        {
            throw new Exception("No se encontró el producto seleccionado");
        }
        return true;
    }
}