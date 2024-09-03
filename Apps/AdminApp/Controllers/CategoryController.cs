using AutoMapper;
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
        private readonly IMapper _mapper;

        public CategoryController(ShopAppContext shopAppContext, IMapper mapper)
        {
            _shopAppContext = shopAppContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            if (id == null) return BadRequest();
            var existCategory= await _shopAppContext.Categories.Include(c=>c.Products).Where(c=>!c.IsDeleted).FirstOrDefaultAsync(c=>c.Id==id);
            if (existCategory == null) return NotFound();
            return Ok(_mapper.Map<CategoryReturnDto>(existCategory));
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto ccDto)
        {
            var exists = await _shopAppContext.Categories.AnyAsync(c => !c.IsDeleted && c.Name.ToLower() == ccDto.Name.ToLower());
            if (exists) return StatusCode(409);
            Category category = new();
            if (ccDto.Photo is null) return BadRequest();
            if (!ccDto.Photo.ContentType.Contains("image/")) return BadRequest();
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
