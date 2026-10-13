namespace ShopTARpe25.Models.Kindergarten
{
    public class ImageViewModel // uus klass see hoiab pildi infot
    {
        public Guid ImageId { get; set; } 
        public string? FilePath { get; set; } 
        public Guid? KindergartenId { get; set; } 
    }
}