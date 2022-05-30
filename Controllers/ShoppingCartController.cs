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
        return new JsonResult(value);
    }

    [HttpDelete("{idUser}/{idProduct}")]
    public async Task<ActionResult> RemoveToShoppingCartAsync(int idUser, int idProduct)
    {
        using (var transaction = DbContext.Database.BeginTransaction())
        {
            try
            {
                List<Product> value = new List<Product>();
                var shoppingCart = DbContext.ShoppingCarts.Where(s => s.IdUser == idUser);
                var productSelectedCart = DbContext.ProductSelectedCarts.Where(p => p.IdShoppingCart == shoppingCart.First().id).ToList();
                productSelectedCart.Remove(DbContext.ProductSelectedCarts.Where(p => p.IdProduct == idProduct).First());
                await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new JsonResult(value);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new JsonResult(new { error = ex.Message }));
            }
        }
    }
}