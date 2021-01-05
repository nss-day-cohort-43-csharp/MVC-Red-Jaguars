using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;




namespace TabloidMVC.Repositories
{
    public class SubscriptionRepository : BaseRepository
    {
        public SubscriptionRepository(IConfiguration config) : base(config) { }

        

        
    }
}
