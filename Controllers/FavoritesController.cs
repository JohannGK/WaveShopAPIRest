using Microsoft.AspNetCore.Mvc;
using WaveShopAPIRest.Models;

namespace WaveShopAPIRest.Controllers;

[ApiController]
[Route("api/[controller]")]

public class FavoritesController : ControllerBase
{
    private WaveShopContext DbContext { get; set; }

    public FavoritesController(WaveShopContext dbContext)
    {
        DbContext = dbContext;
    }

    [HttpGet("{IdUser}")]
    public ActionResult GetFavoriteProducts(int IdUser)
    {
        List<Product> value = new List<Product>();
        foreach(var f in DbContext.Favorites.Where(p => p.IdProduct == IdUser).ToList()){
            value.Add(DbContext.Products.Find(f.IdProduct));
        }
        value.ForEach(p => p.Product_Images = DbContext.Product_Images.Where(i => i.IdProduct == p.Id).ToArray());        
        return new JsonResult(value);
    }


    [HttpPost("{idUser}/{idProduct}")]
    public async Task<ActionResult> AddFavoriteProductAsync(int idUser, int idProduct)
    {
        using (var transaction = DbContext.Database.BeginTransaction())
        {
            try
            {
                CheckFavorites(idUser, idProduct);
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

    [HttpDelete("{idUser}/{idProduct}")]
    public async Task<ActionResult> RemoveFromFavorite(int idUser, int idProduct)
    {
        using (var transaction = DbContext.Database.BeginTransaction())
        {
            try
            {
                var product = DbContext.Favorites.FirstOrDefault(c => c.IdUser == idUser && c.IdProduct == idProduct);
                DbContext.Favorites.Remove(product);
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

    private void CheckFavorites(int idUser, int idProduct)
    {
        try
        {
            DbContext.Favorites.Where(f => f.IdUser == idUser && f.IdProduct == idProduct).ToList().First();
            throw new Exception("Este producto ya está agregado a tu lista de favoritos");
        }
        catch (Exception){}
    }
}