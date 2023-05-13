using Leo.Model.Domains;
using Leo.Model.DTOs;
using Leo.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;

namespace Leo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly IImageService _imageService;
        private IWebHostEnvironment _webHostEnvironment;

        public ProductController
            (
            ICustomerService customerService, 
            IProductService productService, 
            IImageService imageService,
            IWebHostEnvironment webHostEnvironment
            )
                {
                    _customerService = customerService;
                    _productService = productService;
                    _imageService = imageService;
                    _webHostEnvironment = webHostEnvironment;
                }



        [HttpGet("ByCustomer")]
        public async Task<IActionResult> GetProductsBy(int customerId)
        {
            if(await _customerService.GetCustomerBy(customerId) is not null)
            {
                try
                {
                    List<ProductImageDTO> imagesURL = new List<ProductImageDTO>();
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads\\Products\\");
                    var obj = await _imageService.GetImageByCustomer(customerId);

                    List<ProductImageDTO> imgs = new List<ProductImageDTO>();

                    foreach(var i in obj)
                    {
                        string realPath = string.Concat(path, i.ImageURL);
                        if (System.IO.File.Exists(realPath))
                        {

                            MemoryStream m = new MemoryStream();
                            FileStream f = new FileStream(realPath, FileMode.Open);
                            Console.WriteLine(f.Length);
                            f.CopyTo(m);
                            imagesURL.Add(new ProductImageDTO { Data = Convert.ToBase64String(m.ToArray()) });
                            m.Flush();
                        }
                    }


                    return Ok(imagesURL);
                }catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return BadRequest();
        }

        [HttpGet("Images")]
        public async Task<IActionResult> GetProductWithImages()
        {
            List<ProductImageDTO> imagesURL = new List<ProductImageDTO>();
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads\\Products\\");
            var images = await _imageService.GetAllImages();

            foreach(var i in images)
            {
                string realPath = string.Concat(path, i);
                if (System.IO.File.Exists(realPath))
                {

                    MemoryStream m = new MemoryStream();
                    FileStream f = new FileStream(realPath, FileMode.Open);
                    Console.WriteLine(f.Length);
                    f.CopyTo(m);
                    imagesURL.Add(new ProductImageDTO { Data = Convert.ToBase64String(m.ToArray()) });
                    m.Flush();
                }
            }

            return Ok(imagesURL);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return Ok(await _productService.GetProducts());
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] ProductsDTO dto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            else if (await _customerService.GetCustomerBy(dto.CustomerId) is null) return BadRequest();

            Product p = new Product()
            {
                CustomerId = dto.CustomerId,
            };

            List<Image> m = new List<Image>();

            try
            {
                var obj = await _productService.Add(p);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            foreach(var v in dto.ImageUrl)
            {
                var fakeName = UploadFile(v);

                Image mm = new Image()
                {
                    ImageURL = fakeName,
                    Product = p,
                    ProductId = p.ProductId
                };

                m.Add(mm);
            }

            try
            {
                await _imageService.AddList(m);
                return Ok(p);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut]
        public async Task<IActionResult> EditProduct(int id, ProductsDTO dto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            else if(!await _productService.isProductExists(id)) return BadRequest();
            else if(await _customerService.GetCustomerBy(dto.CustomerId) is null) return BadRequest();

            var product = await _productService.GetProductBy(id);

            product.CustomerId = dto.CustomerId;
            List<Image> m = new List<Image>();

            foreach (var v in dto.ImageUrl)
            {
                var fakeName = Path.GetRandomFileName();
                Image mm = new Image()
                {
                    ImageURL = fakeName,
                    Product = product,
                    ProductId = product.ProductId
                };

                m.Add(mm);
            }

            try
            {
                 await _productService.Update(product);
                
                if(m.Count > 0)
                {
                    await _imageService.AddList(m);
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            if(!await _imageService.isImageExists(imageId)) return BadRequest();

            var image = _imageService.GetImageBy(imageId).Result;

            try
            {
                if (_imageService.Delete(image)) return Ok();
                return BadRequest();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private string UploadFile(IFormFile file)
        {
            string fakeName = string.Empty;

            if(file != null)
            {
                fakeName = Path.GetRandomFileName();
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads\\Products", fakeName);
                using FileStream f = new FileStream(path, FileMode.Create);
                file.CopyTo(f);
            }

            return fakeName;
        }
        
    }
}
