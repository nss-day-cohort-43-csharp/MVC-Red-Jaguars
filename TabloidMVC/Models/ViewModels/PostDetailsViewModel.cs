using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class PostDetailsViewModel
    {
        public Post Post { get; set; }
        public Tag Tag { get; set; }
        public Boolean Subscribed { get; set; }
        public List<Reaction> AllReactions { get; set; }
        public List<PostReaction> AllPostReactions { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
