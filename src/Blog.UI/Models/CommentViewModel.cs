using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Xango.Mvc.ViewModel;

namespace Blog.UI.Models
{
    public class CommentViewModel : ViewModelBase
    {
        [Display(Name = "name")]
        [Required]
        [StringLength(255)]
        public string Author { get; set; }

        [Display(Name = "email")]
        [RegularExpression(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$")]
        public string Mail { get; set; }

        [Display(Name = "comment")]
        [Required]
        [StringLength(2000)]
        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
}