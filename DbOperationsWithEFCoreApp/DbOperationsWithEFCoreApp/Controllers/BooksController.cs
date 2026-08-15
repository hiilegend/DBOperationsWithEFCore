using DbOperationsWithEFCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEFCoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(AppDbContext appDbContext) : ControllerBase
    {
        [HttpPost("")]
        public async Task<IActionResult> AddNewBook([FromBody] Book book )
        {
            //var author = new Author()
            //{
            //    Name = "Author 1",
            //    Email = "test@gmail.com"
            //};
            //book.Author = author;
            appDbContext.Books.Add(book);
            await appDbContext.SaveChangesAsync();

            return Ok(book);
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> AddBooks([FromBody] List<Book> book)
        {
            appDbContext.Books.AddRange(book);
            await appDbContext.SaveChangesAsync();
            return Ok(book);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook([FromRoute] int bookId, [FromBody] Book model)
        {
            var book = appDbContext.Books.FirstOrDefault(x => x.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }
            book.Title = model.Title;
            book.Description = model.Description;

            await appDbContext.SaveChangesAsync();
            return Ok(book);
        }
        [HttpPut("")]
        public async Task<IActionResult> UpdateBookWithSingleQuery([FromRoute] int bookId, [FromBody] Book model)
        {
            // update book in single Query
            //appDbContext.Books.Update(model);

            appDbContext.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;


            await appDbContext.SaveChangesAsync();
            return Ok(model);
        }
        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateBookInBulk()
        {
            //var books = appDbContext.Books.ToList();
            //foreach (var item in books)
            //{
            //    item.Title = "Updated";
            //} //<- not good approach hr baar database fetch krega

            //await appDbContext.Books.ExecuteUpdateAsync(
            //    x => x.SetProperty(p => p.Description, "This is book description")
            //    .SetProperty(p=>p.Title, p=>p.Title + "Updated")
            //    );

            await appDbContext.Books
                .Where(x => x.NumberOfPages == 100)
                .ExecuteUpdateAsync(
                x => x.SetProperty(p => p.Description, "This is book description")
                .SetProperty(p => p.Title, p => p.Title + "Updated")
                );
            return Ok();
        }
        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBookByIdAsync([FromRoute] int bookId)
        {
            var book = new Book { Id = bookId };
            appDbContext.Entry(book).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

            //var book = appDbContext.Books.FirstOrDefault(x => x.Id == bookId);
            //if(book == null)
            //{
            //    return NotFound();
            //}
            //appDbContext.Books.Remove(book);
            //await appDbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteBookinBulkAsync()
        {
            //var book = new Book { Id = bookId };
            //appDbContext.Entry(book).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

            //var book = await appDbContext.Books.ToListAsync();
            //if (book == null)
            //{
            //    return NotFound();
            //}
            //appDbContext.Books.RemoveRange(book);
            //await appDbContext.SaveChangesAsync();
            var books = await appDbContext.Books.Where(x => x.Id < 8).ExecuteDeleteAsync();
            return Ok();
        }
    }
}
