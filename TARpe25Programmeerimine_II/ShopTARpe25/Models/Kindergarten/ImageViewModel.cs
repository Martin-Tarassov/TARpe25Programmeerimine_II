namespace ShopTARpe25.Models.Kindergarten
{
    public class ImageViewModel // uus klass see hoiab pildi infot
    {
        public Guid ImageId { get; set; } // pildi Id
        public string? FilePath { get; set; } // pildi faili nimi
        public Guid? KindergartenId { get; set; } // lasteaia Id      
    }
}