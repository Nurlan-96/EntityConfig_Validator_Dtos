using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAppAPI.Apps.AdminApp.Dtos.CategoryDto;
using ShopAppAPI.Data;
using ShopAppAPI.Entities;

namespace ShopAppAPI.Apps.AdminApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ShopAppContext _shopAppContext;

        public CategoryController(ShopAppContext shopAppContext)
        {
            _shopAppContext = shopAppContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            if (id == null) return BadRequest();
            var existCategory= await _shopAppContext.Categories.Where(c=>!c.IsDeleted).FirstOrDefaultAsync(c=>c.Id==id);
            if (existCategory == null) return NotFound();
            CategoryReturnDto crDto = new();
            crDto.Id = existCategory.Id;
            crDto.Name = existCategory.Name;
            crDto.CreatedDate = existCategory.CreatedDate;
            crDto.UpdateDate= existCategory.UpdatedDate;
            crDto.ImageUrl = "http://localhost:51012/images" + existCategory.Image;
            return Ok(crDto);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto ccDto)
        {
            var exists = await _shopAppContext.Categories.AnyAsync(c => !c.IsDeleted && c.Name.ToLower() == ccDto.Name.ToLower());
            if (exists) return StatusCode(409);
            Category category = new();
            if (ccDto.Photo is null) return BadRequest();
            if (!ccDto.Photo.ContentType.Contains("images/")) return BadRequest();
            if (ccDto.Photo.Length/1024 > 1000) return BadRequest();
            string fileName = Guid.NewGuid().ToString()+Path.GetExtension(ccDto.Photo.FileName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
            using FileStream fileStream = new(path, FileMode.Create);
            await ccDto.Photo.CopyToAsync(fileStream);
            category.Name = ccDto.Name;
            category.Image = fileName;
            await _shopAppContext.Categories.AddAsync(category);
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(201);
        }
    }
}
