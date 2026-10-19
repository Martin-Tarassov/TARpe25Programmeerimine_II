namespace ShopTARpe25.Models.Kindergarten
{
    public class KindergartenDeleteViewModel
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public int? ChildrenCount { get; set; }
        public string KindergartenName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public List<ImageViewModel> Images { get; set; } 
            = new List<ImageViewModel>();
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public DateTime? FoundedDate { get; set; } // lasteaia asutamise kuupäev
    }
}