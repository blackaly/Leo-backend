using Leo.Model;
using Leo.Model.Domains;
using Leo.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Leo.Services.Implementation
{
    public class ImageService : IImageService
    {

        private readonly ApplicationDbContext _context;

        public ImageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Image>> AddList(IList<Image> img)
        {
            await _context.Images.AddRangeAsync(img);
            await _context.SaveChangesAsync();
            return img;
        }

        public bool Delete(Image image)
        {
            _context.Images.Remove(image);
            bool deleted = true;
            var entry = _context.Images.Entry(image);
            if (entry.State == EntityState.Deleted) return deleted;
            return !deleted;
        }

        public async Task<Image> GetImageBy(int imageId)
        {
            return await _context.Images.FindAsync(imageId);
        }

        public async Task<bool> isImageExists(int imageId)
        {
            return await _context.Images.AnyAsync(x => x.ImageId == imageId);
        }

        public async Task<IList<Image>> GetImageByProduct(int productId)
        {
            return await _context.Images.Where(x => x.ProductId == productId).ToListAsync();
        }

        public async Task<IList<Image>> GetImageByCustomer(int Id)
        {
            return await _context.Images.Where(x => x.Product.CustomerId == Id).ToListAsync();
        }

        public async Task<IList<string>> GetAllImages()
        {
            return await _context.Images.Select(x => x.ImageURL).ToListAsync();
        }
    }
}
