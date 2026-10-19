namespace ShopTARpe25.Models.Kindergarten
{
    public class KindergartenCreateUpdateViewModel
    {
        public Guid? Id { get; set; } // kui Id on olemas siis on Update
        public string GroupName { get; set; } = string.Empty;
        public int? ChildrenCount { get; set; }
        public string KindergartenName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;

        public List<IFormFile>? Files { get; set; } // uued failid mis kasutaja valib
        public List<ImageViewModel> Images { get; set; } 
            = new List<ImageViewModel>(); // olemasolevad pildid

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public DateTime? FoundedDate { get; set; } // lasteaia asutamise kuupäev
    }
}