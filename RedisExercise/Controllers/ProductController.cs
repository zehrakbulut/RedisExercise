using Microsoft.AspNetCore.Mvc;
using RedisExercise.Context;
using RedisExercise.Services;
using RedisExercise.Tables;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
	private readonly ICacheService _cacheService;
	private readonly DbRedis _context;

	public ProductController(ICacheService cacheService, DbRedis context)
	{
		_cacheService = cacheService;
		_context = context;
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetProduct(int id)
	{
		string cacheKey = $"product:{id}";

		var product = await _cacheService.GetOrAddAsync(cacheKey,
			async () => await _context.Products.FindAsync(id));

		if (product == null)
			return NotFound();

		return Ok(product);
	}

	[HttpPost]
	public async Task<IActionResult> CreateProduct([FromBody] Product product)
	{
		if (product == null)
			return BadRequest();

		_context.Products.Add(product);
		await _context.SaveChangesAsync();

		await _cacheService.SetStringAsync($"product:{product.Id}", product);

		return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
	{
		if (product == null || product.Id != id)
		{
			return BadRequest();
		}

		var existingProduct = await _context.Products.FindAsync(id);
		if (existingProduct == null)
			return NotFound();

		existingProduct.Name = product.Name;
		existingProduct.Price = product.Price;
		existingProduct.Stok = product.Stok;

		await _context.SaveChangesAsync();

		await _cacheService.SetStringAsync($"product:{id}", existingProduct);

		return Ok(existingProduct);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteProduct(int id)
	{
		var existingProduct = await _context.Products.FindAsync(id);
		if (existingProduct == null)
			return NotFound();

		_context.Products.Remove(existingProduct);
		await _context.SaveChangesAsync();

		await _cacheService.RemoveAsync($"product:{id}");

		return NoContent(); 
	}
}
