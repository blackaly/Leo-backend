namespace Leo.Model.DTOs
{
    public class ProductsDTO
    {
        public int CustomerId { get; set; }
        public IList<FormFile> ImageUrl { get; set; }
    }
}
