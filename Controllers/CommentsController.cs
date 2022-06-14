using Microsoft.AspNetCore.Mvc;
using WaveShopAPIRest.Models;

namespace WaveShopAPIRest.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CommentsController : ControllerBase
{
    private WaveShopContext DbContext { get; set; }

    public CommentsController(WaveShopContext dbContext)
    {
        DbContext = dbContext;
    }

    [HttpGet("{IdProduct}")]
    public ActionResult GetComments(int idProduct)
    {
        List<Comment> value = new List<Comment>();
        var comments = DbContext.Comments.Where(c => c.IdProduct == idProduct);
        if (comments != null)
        {
            value = comments.ToList().Where(p => p.IdProduct == idProduct).ToList();
        }
        return new JsonResult(value);
    }

    [HttpGet("comment/{IdComment}")]
    public ActionResult GetReplyComments(int idComment)
    {
        List<Comment> value = new List<Comment>();
        var comments = DbContext.Comments.Where(c => c.IdComment == idComment);
        if (comments != null)
        {
            value = comments.ToList().Where(p => p.IdComment == idComment).ToList();
        }
        return new JsonResult(value);
    }

    [HttpPost]
    public async Task<ActionResult> AnnotateAsync(Comment comment)
    {
        using (var transaction = DbContext.Database.BeginTransaction())
        {
            try
            {
                Comment newComment;
                await DbContext.Comments.AddAsync(newComment = new Comment()
                {
                    UserName = comment.UserName,
                    OpinionResume = comment.OpinionResume,
                    Content = comment.Content,
                    Visible = "V",
                    PhotoAddress = comment.PhotoAddress,
                    Likes = 0,
                    Dislikes = 0,
                    Published = DateTime.Now,
                    IdProduct = comment.IdProduct,
                    IdComment = comment.IdComment
                });
                await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new JsonResult(newComment);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new JsonResult(new { error = ex.InnerException }));
            }
        }
    }

    [HttpPost("comment")]
    public async Task<ActionResult> ReplyAsync(Comment comment)
    {
        using (var transaction = DbContext.Database.BeginTransaction())
        {
            try
            {
                Comment newComment;
                await DbContext.Comments.AddAsync(newComment = new Comment()
                {
                    UserName = comment.UserName,
                    OpinionResume = comment.OpinionResume,
                    Content = comment.Content,
                    Visible = "V",
                    PhotoAddress = comment.PhotoAddress,
                    Likes = 0,
                    Dislikes = 0,
                    Published = DateTime.Now,
                    IdProduct = comment.IdProduct,
                    IdComment = comment.IdComment
                });
                await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new JsonResult(newComment);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new JsonResult(new { error = ex.InnerException }));
            }
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> HideCommentAsync(int id, Comment comment)
    {
        if (DbContext.Comments.Any(p => p.Id == id))
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var result = await DbContext.Comments.FindAsync(id);
                    result.UserName = comment.UserName;
                    result.OpinionResume = comment.OpinionResume;
                    result.Content = comment.Content;
                    result.Visible = "H";
                    result.PhotoAddress = comment.PhotoAddress;
                    result.Likes = comment.Likes;
                    result.Dislikes = comment.Dislikes;
                    result.Published = comment.Published;
                    result.IdProduct = comment.IdProduct;
                    result.IdComment = comment.IdComment;
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

    [HttpPut("ActiveComment/{id}")]
    public async Task<ActionResult> ActiveCommentAsync(int id, Comment comment)
    {
        if (DbContext.Comments.Any(p => p.Id == id))
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var result = await DbContext.Comments.FindAsync(id);
                    result.UserName = comment.UserName;
                    result.OpinionResume = comment.OpinionResume;
                    result.Content = comment.Content;
                    result.Visible = "V";
                    result.PhotoAddress = comment.PhotoAddress;
                    result.Likes = comment.Likes;
                    result.Dislikes = comment.Dislikes;
                    result.Published = comment.Published;
                    result.IdProduct = comment.IdProduct;
                    result.IdComment = comment.IdComment;
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
}