using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xango.Mvc.ViewModel;

namespace Blog.UI.Models
{
    public class PostViewModel : ViewModelBase
    {
        public PostViewModel()
        {
            Comments = new List<CommentViewModel>();
        }

        [Display(Name = "title")]
        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Display(Name = "author")]
        [Required]
        [StringLength(255)]
        public string Author { get; set; }

        [Display(Name = "body")]
        [Required]
        public string Body { get; set; }

        [Display(Name = "create at")]
        public string CreatedAt { get; set; }

        public string Slug { get; set; }

        public string Day { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}