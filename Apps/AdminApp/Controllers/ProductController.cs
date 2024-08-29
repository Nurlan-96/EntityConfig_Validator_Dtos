using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAppAPI.Apps.AdminApp.Dtos.ProductDto;
using ShopAppAPI.Data;
using ShopAppAPI.Entities;

namespace ShopAppAPI.Apps.AdminApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ShopAppContext _shopAppContext;

        public ProductController(ShopAppContext shopAppContext)
        {
            _shopAppContext = shopAppContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string search,int page = 1)
        {
            var query = _shopAppContext.Products
                .Where(p => !p.IsDeleted);
            if (!string.IsNullOrEmpty(search))
                query = query.Where(p=>p.Name.ToLower().Contains(search.ToLower()));

            ProductListDto plDto = new ();
            plDto.Page = page;
            plDto.TotalCount = query.Count();
            plDto.Items = await query.Skip((page - 1) * 2).Take(2)
                .Select(p=> new ProductItemListDto
                {
                Id = p.Id,
                Name = p.Name,
                SalePrice = p.SalePrice,
                CostPrice = p.CostPrice,
                CreatedDate = p.CreatedDate,
                UpdatedDate = p.UpdatedDate,
                }).ToListAsync();
            return Ok(plDto);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int? id)
        {
            if (id == null) return BadRequest();
            var existProduct = await _shopAppContext.Products
                .Include(p=>p.Category)
                .Where(p=>!p.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (existProduct == null) return NotFound();
            ProductReturnDto prDto = new ();
            prDto.Name = existProduct.Name;
            prDto.SalePrice = existProduct.SalePrice;
            prDto.CostPrice = existProduct.CostPrice;
            prDto.Id = existProduct.Id;
            prDto.CreatedDate = existProduct.CreatedDate;
            prDto.UpdatedDate = existProduct.UpdatedDate;
            prDto.CategoryName = existProduct.Category.Name;
            return Ok(prDto);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDto pcd)
        {
            if (!await _shopAppContext.Categories.AnyAsync(c => !c.IsDeleted && c.Id == pcd.CategoryId)) return StatusCode(409);
            Product product = new ();
            product.Name = pcd.Name;
            product.SalePrice = pcd.SalePrice;
            product.CostPrice = pcd.CostPrice;
            product.CategoryId=pcd.CategoryId;
            await _shopAppContext.Products.AddAsync(product);
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int? id, ProductUpdateDto puDto)
        {
            if (id == null) return StatusCode(400);
            var existProduct = await _shopAppContext.Products
                .Where(x=>!x.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id ==id);
            if (existProduct == null) return NotFound();
            if (!await _shopAppContext.Categories.AnyAsync(c => !c.IsDeleted && c.Id == puDto.CategoryId)) return StatusCode(409);

            existProduct.Name = puDto.Name;
            existProduct.SalePrice = puDto.SalePrice;
            existProduct.CostPrice = puDto.CostPrice;
            existProduct.CategoryId= puDto.CategoryId;
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, bool status)
        {
            var existProduct = await _shopAppContext.Products.Where(x=>!x.IsDeleted).FirstOrDefaultAsync(x => x.Id == id);
            if (existProduct == null) return NotFound();
            existProduct.IsDeleted = status;
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existProduct = await _shopAppContext.Products.Where(x => !x.IsDeleted).FirstOrDefaultAsync(x => x.Id == id);
            if (existProduct == null) return NotFound();
            existProduct.IsDeleted = true;
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
