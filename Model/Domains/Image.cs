namespace Leo.Model.Domains
{
    public class Image
    {
        public int ImageId { get; set; }
        public string ImageURL { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

    }
}
