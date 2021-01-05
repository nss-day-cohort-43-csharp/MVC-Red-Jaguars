using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class CommentCreateViewModel
    {
        public Post Post { get; set; }
        public Comment Comment { get; set; }
        public string ErrorMessage { get; set; }
    }
}
