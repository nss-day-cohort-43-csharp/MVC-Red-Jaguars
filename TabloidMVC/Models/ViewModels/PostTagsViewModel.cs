using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class PostTagsViewModel
    {
        public List<Tag> tags { get; set; }
        public Post post { get; set; }
        public Tag tag { get; set; }
    }
}
