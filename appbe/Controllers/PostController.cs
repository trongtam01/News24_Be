using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using appbe.Models;
using appbe.Models.Post;
using Microsoft.AspNetCore.Identity;
using appbe.Utilities;
using appbe.Models.DTOs.Responses;

namespace appbe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PostController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: api/Post
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostModel>>> GetPosts()
        {
            return await _context.Posts.Include(x => x.Category).Select(x => new PostModel()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Content = x.Content,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated,
                ImageName = x.ImageName,
                Published = x.Published,
                CategoryId = x.CategoryId,
                ImageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.ImageName),
            }).ToListAsync();
        }

        // GET: api/Post/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostModel>> GetPostModel(int id)
        {
            var postModel = await _context.Posts.FindAsync(id);

            if (postModel == null)
            {
                return NotFound();
            }

            postModel.ImageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, postModel.ImageName);

            return postModel;
        }

        // PUT: api/Post/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id,[FromForm] PostModel postModel)
        {
            if (id != postModel.Id)
            {
                return BadRequest();
            }

            if (postModel.ImageFile != null)
            {
                DeleteImage(postModel.ImageName);
                postModel.ImageName = await SaveImage(postModel.ImageFile);
            }

            _context.Entry(postModel).State = EntityState.Modified;

            try
            {
                var productUpdate = await _context.Posts.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
                if (productUpdate == null)
                {
                    return NotFound();
                }

                productUpdate.Title = postModel.Title;
                productUpdate.Description = postModel.Description;
                productUpdate.Content = postModel.Content;
                productUpdate.Published = postModel.Published;
                productUpdate.DateUpdated = DateTime.Now;
                productUpdate.CategoryId = postModel.CategoryId;

                _context.Update(productUpdate);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Post
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PostModel>> Post([FromForm]PostModel postModel)
        {
            postModel.ImageName = await SaveImage(postModel.ImageFile);
            postModel.DateCreated = postModel.DateUpdated = DateTime.Now;
            postModel.Category = await _context.Categories.Where(p => p.Id == postModel.CategoryId).FirstOrDefaultAsync();
            

            _context.Posts.Add(postModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPostModel), new { id = postModel.Id }, postModel);
        }

        // DELETE: api/Post/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var postModel = await _context.Posts.FindAsync(id);
            if (postModel == null)
            {
                return NotFound();
            }

            DeleteImage(postModel.ImageName);
            _context.Posts.Remove(postModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostModelExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            using (var filestream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(filestream);
            }
            return imageName;
        }

        [NonAction]
        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }
    }
}
