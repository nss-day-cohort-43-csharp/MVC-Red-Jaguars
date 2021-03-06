﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class UserProfileTypeViewModel
    {
        public UserProfile user { get; set; }
        public UserProfile olduser { get; set; }
        public List<UserType> type { get; set; }
        public string ErrorMsg { get; set; }
    }
}
