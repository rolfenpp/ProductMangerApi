using System.ComponentModel.DataAnnotations;
using ECSProductWebAPI.Data;
using ECSProductWebAPI.Data.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ECSProductWebAPI.Controllers;


// GET POST PUT DELETE Product -> ProductsController

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase 
{
    private readonly ApplicationDbContext context;
    public ProductController(ApplicationDbContext context)
    {
        this.context = context;
    }

   [HttpPost] //POST /products

    public ActionResult<ProductDto> CreateProduct(CreateProductRequest createProductRequest)
    {
        // 1 - Skapa ett objekt av typ Product och kopiera över värden från createProductRequest
        var product = new Product
        {
            Namn = createProductRequest.Namn,
            Beskrivning = createProductRequest.Beskrivning,
            Sku = createProductRequest.Sku,
            ImgUrl = createProductRequest.ImgUrl,
            Pris = createProductRequest.Pris
        };

        var skuExist = context.Product.FirstOrDefault(x => x.Sku == product.Sku);
        if(skuExist is not null) {
            return BadRequest();
        }

        context.Product.Add(product);
        context.SaveChanges();

        // 3 - För över information från entitet till DTO och returnera till klienten
        var productDto = new ProductDto
        {
            Id = product.Id,
            Namn = product.Namn,
            Beskrivning = product.Beskrivning,
            Sku = product.Sku,
            ImgUrl = product.ImgUrl,
            Pris = product.Pris
        };

        return Created("", productDto); // 201 Created
    }

[HttpGet] //GET /products?name={name}
public IEnumerable<ProductDto> GetProducts([FromQuery] string? name)
{
    // Ternary-operatorn istället för if-else
    IEnumerable<Product> products = string.IsNullOrEmpty(name)
        ? context.Product.ToList()
        : context.Product.Where(x => x.Namn == name).ToList();

    IEnumerable<ProductDto> productDto = products.Select(x => new ProductDto
    {
        Id = x.Id,
        Namn = x.Namn,
        Sku = x.Sku,
        Beskrivning = x.Beskrivning,
        ImgUrl = x.ImgUrl,
        Pris = x.Pris
    });

    return productDto;
}

    [HttpGet("{sku}")]
    /* [Authorize] */
    public ActionResult<ProductDto> GetProductSku(string sku)
    {
        var product = context.Product.FirstOrDefault(x => x.Sku == sku);

        if (product is null)
            return NotFound(); // 404 Not Found

        var productDto = new ProductDto
        {
            Id = product.Id,
            Namn = product.Namn,
            Beskrivning = product.Beskrivning,
            Sku = product.Sku,
            ImgUrl = product.ImgUrl,
            Pris = product.Pris
        };

        return productDto; // 200 OK
    }

    // DELETE /students/{id}
    [HttpDelete("{sku}")]
    public ActionResult DeleteProduct(string sku)
    {
        var product = context.Product.FirstOrDefault(x => x.Sku == sku);

        if (product is null)
            return NotFound(); // 404 Not Found        

        context.Product.Remove(product);

        // SQL DELETE skickas till databasen för att radera den studerande
        context.SaveChanges();

        return NoContent(); // 204 No Content
    }

}

public class ProductDto 
{
    public int Id {get; set;}
    public string Namn { get; set;}
    public string Beskrivning {get; set;}
    public string Sku   {get; set;}
    public string ImgUrl {get; set;}
    public int Pris {get; set;}
  
}

public class CreateProductRequest 
{

    [Required]
    public string Namn { get; set;}
    
    [Required]
    public string Beskrivning {get; set;}

    [Required]
    public string Sku   {get; set;}

    [Required]
    public string ImgUrl {get; set;}

    [Required]
    public int Pris {get; set;}

}

