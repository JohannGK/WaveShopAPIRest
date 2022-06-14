using Microsoft.AspNetCore.Mvc;
using WaveShopAPIRest.Models;

namespace WaveShopAPIRest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private WaveShopContext DbContext { get; set; }
    public OrdersController(WaveShopContext dbContext)
    {
        DbContext = dbContext;
    }

    [HttpGet("{idUser}")]
    public ActionResult GetProductsShopped(int idUser)
    {
        List<Product> orders = new List<Product>();
        foreach (var order in DbContext.Orders.Where(o => o.IdUser == idUser).ToList())
        {
            DbContext.ProductSelectedOrders.Where(pso => pso.IdOrder == order.Id).ToList().ForEach(p =>
            {
                var product = DbContext.Products.Find(p.IdProduct);
                product.StockQuantity = p.Quantity;
                product.UnitPrice = p.Price;
                orders.Add(product);
            });
        }
        orders.ForEach(p => p.Product_Images = DbContext.Product_Images.Where(i => i.IdProduct == p.Id).ToArray());
        return new JsonResult(orders);
    }

    [HttpGet("sold/{idUser}")]
    public ActionResult GetProductsSold(int idUser)
    {
        List<Product> products = new List<Product>();
        DbContext.Products.Where(p => p.IdVendor == idUser).ToList().ForEach(product =>
        {
            product.Product_Images = DbContext.Product_Images.Where(i => i.IdProduct == product.Id).ToArray();
            products.Add(product);
        });
        return new JsonResult(products);
    }

    [HttpGet("sold/details/{idProduct}")]
    public async Task<ActionResult> GetProductSoldById(int idProduct)
    {
        List<Product> orders = new List<Product>();
        foreach (var pso in DbContext.ProductSelectedOrders.Where(ps => ps.IdProduct == idProduct).ToList())
        {
            var order = await DbContext.Orders.FindAsync(pso.IdOrder);
            var product = DbContext.Products.Find(idProduct);
            var clone = CloneProduct(product);
            clone.IdVendor = order.IdUser;
            clone.VendorUsername = DbContext.Users.Find(order.IdUser).UserName;
            clone.UnitPrice = pso.Price;
            clone.StockQuantity = pso.Quantity;
            clone.LastUpdate = order.Ordered;
            orders.Add(clone);
        }
        return new JsonResult(orders);
    }


    private Product CloneProduct(Product product)
    {
        var p = new Product()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            VideoAddress = product.VideoAddress,
            StockQuantity = product.StockQuantity,
            UnitPrice = product.UnitPrice,
            Status = product.Status,
            Published = product.Published,
            Country = product.Country,
            Location = product.Location,
            IdCategory = product.IdCategory,
            IdVendor = product.IdVendor,
            LikesNumber = product.LikesNumber,
            DislikesNumber = product.DislikesNumber,
            ShoppedTimes = product.ShoppedTimes,
            CommentsNumber = product.CommentsNumber,
            LastUpdate = product.LastUpdate,
            VendorUsername = product.VendorUsername
        };
        return p;
    }

    [HttpPost("buy/{idUser}")]
    public async Task<ActionResult> PurchaseProduct(int idUser, Product product)
    {
        using (var transaction = DbContext.Database.BeginTransaction())
        {
            try
            {
                Order order = await SaveOrder(idUser, -1);
                ProductSelectedOrder productSelected;
                var p = await DbContext.Products.FindAsync(product.Id);
                if (p.StockQuantity < product.StockQuantity)
                {
                    throw new Exception("La cantidad del producto no es suficiente");
                }
                else
                {
                    p.StockQuantity -= product.StockQuantity;
                    p.ShoppedTimes += product.StockQuantity;
                    p.LastUpdate = DateTime.Now;
                }
                await DbContext.SaveChangesAsync();
                await DbContext.ProductSelectedOrders.AddAsync(productSelected = new ProductSelectedOrder()
                {
                    Price = product.UnitPrice * product.StockQuantity,
                    Quantity = product.StockQuantity,
                    Status = "Shopped",
                    IdProduct = product.Id,
                    IdOrder = order.Id
                });
                await DbContext.SaveChangesAsync();
                order.Total = productSelected.Price;
                await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new JsonResult(order);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new JsonResult(new { error = ex.Message }));
            }
        }
    }

    [HttpPost("{idUser}")]
    public async Task<ActionResult> PurchaseShoppingCartAsync(int idUser)
    {
        using (var transaction = DbContext.Database.BeginTransaction())
        {
            try
            {
                int idShoppingCart = DbContext.ShoppingCarts.FirstOrDefault(i => i.IdUser == idUser).id;
                Order order = await SaveOrder(idUser, idShoppingCart);
                var products = DbContext.ProductSelectedCarts.Where(ps => ps.IdShoppingCart == idShoppingCart).ToList();
                foreach (var p in products)
                {
                    Product? product = await DbContext.Products.FindAsync(p.IdProduct);
                    if (p.Quantity > product.StockQuantity)
                        throw new Exception($"El producto '{product.Name}' no tiene la cantidad suficiente en el stock");
                    product.StockQuantity -= p.Quantity;
                    product.ShoppedTimes += p.Quantity;
                    product.LastUpdate = DateTime.Now;
                    order.Total += p.Price;
                    await SaveProductBought(p, order.Id);
                }
                await DbContext.SaveChangesAsync();
                products.ForEach(p => DbContext.ProductSelectedCarts.Remove(DbContext.ProductSelectedCarts.Find(p.Id)));
                await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new JsonResult(order);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new JsonResult(new { error = ex.Message }));
            }
        }
    }

    private async Task<Order> SaveOrder(int idUser, int idShoppingCart)
    {
        Order order;
        await DbContext.Orders.AddAsync(order = new Order()
        {
            IdUser = idUser,
            IdShoppingCart = idShoppingCart,
            Ordered = DateTime.Now,
            Shipped = DateTime.Now,
            Status = "Shopped",
            Total = 0
        });
        await DbContext.SaveChangesAsync();
        return order;
    }

    private async Task<ProductSelectedOrder> SaveProductBought(ProductSelectedCart product, int oderId)
    {
        ProductSelectedOrder productOrdered;
        await DbContext.ProductSelectedOrders.AddAsync(productOrdered = new ProductSelectedOrder()
        {
            Price = product.Price,
            Quantity = product.Quantity,
            Status = "Shopped",
            IdProduct = product.IdProduct,
            IdOrder = oderId
        });
        await DbContext.SaveChangesAsync();
        return productOrdered;
    }
}