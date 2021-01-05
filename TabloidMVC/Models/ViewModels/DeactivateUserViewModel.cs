using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class DeactivateUserViewModel
    {
        public UserProfile User { get; set; }
        public string ErrorMsg { get; set; }
    }
}
