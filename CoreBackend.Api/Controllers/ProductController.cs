using CoreBackend.Api.Dto;
using CoreBackend.Api.Entities;
using CoreBackend.Api.Repositories;
using CoreBackend.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace CoreBackend.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {

        private readonly ILogger<ProductController> _logger; // interface 不是具体的实现类
        private readonly IMailService _localMailService;
        private readonly IProductRepository _productRepository;
        private readonly MyContext _context;
        public ProductController(ILogger<ProductController> logger, IMailService localMailService,     IProductRepository productRepository, MyContext context)
        {
            _logger = logger;
            _localMailService = localMailService;
            _productRepository = productRepository;
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            //   _logger.LogInformation("Index page says hello");
            //  _localMailService.Send("Product Deleted", $"Id为{id}的产品被删除了");
            ProductRepository repository = new ProductRepository(_context);
            
            return new JsonResult(repository.GetProducts().ToList());
        }

        [HttpGet("{productId}/materials")]
        public IActionResult GetMaterials(int productId)
        {
            var product = ProductService.Current.Products.SingleOrDefault(x => x.Id == productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product.Materials);
        }

        [HttpGet("{productId}/materials/{id}")]
        public IActionResult GetMaterial(int productId, int id)
        {
            var product = ProductService.Current.Products.SingleOrDefault(x => x.Id == productId);
            if (product == null)
            {
                return NotFound();
            }
            var material = product.Materials.SingleOrDefault(x => x.Id == id);
            if (material == null)
            {
                return NotFound();
            }
            return Ok(material);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProductCreation product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var maxId = ProductService.Current.Products.Max(x => x.Id);

            var newProduct = new Product
            {
                Id = ++maxId,
                Name = product.Name,
                Price = product.Price
            };
            ProductService.Current.Products.Add(newProduct);

            return CreatedAtRoute("GetProduct", new { id = newProduct.Id }, newProduct);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ProductModification product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            if (product.Name == "产品")
            {
                ModelState.AddModelError("Name", "产品的名称不可以是'产品'二字");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = ProductService.Current.Products.SingleOrDefault(x => x.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            model.Name = product.Name;
            model.Price = product.Price;
            model.Description = product.Description;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var model = ProductService.Current.Products.SingleOrDefault(t => t.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            return NoContent();

        }
    }
}
