using Leo.Model.Domains;

namespace Leo.Services.Interfaces
{
    public interface IImageService
    {
        Task<IList<Image>> AddList(IList<Image> img); 
        bool Delete(Image image);
        Task<bool> isImageExists(int imageId);
        Task<Image> GetImageBy(int imageId);
        public Task<IList<Image>> GetImageByProduct(int Id);
        public Task<IList<string>> GetAllImages();
        Task<IList<Image>> GetImageByCustomer(int Id);
    }
}
