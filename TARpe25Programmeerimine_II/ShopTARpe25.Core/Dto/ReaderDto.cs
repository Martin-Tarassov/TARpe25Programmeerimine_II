namespace ShopTARpe25.Core.Dto
{
    // Dto viib lugeja andmeid kontrollerist teenusesse
    public class ReaderDto
    {
        public Guid? Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}