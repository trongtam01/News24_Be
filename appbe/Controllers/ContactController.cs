using appbe.Models.Post;
using appbe.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appbe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactModel>>> GetContacts()
        {
            return await _context.Contacts.ToListAsync();
        }

        // GET: api/TodoItems/5
        // <snippet_GetByID>
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactModel>> GetContact(int id)
        {
            var todoItem = await _context.Contacts.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        [HttpPost]
        public async Task<ActionResult<ContactModel>> PostContact([FromBody]ContactModel categoryModel)
        {
            var todoItem = new ContactModel
            {
                FullName = categoryModel.FullName,
                Email = categoryModel.Email,
                Message = categoryModel.Message,
                Subject = categoryModel.Subject,
                DateSent = DateTime.Now
            };

            _context.Contacts.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetContact), new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var todoItem = await _context.Contacts.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
