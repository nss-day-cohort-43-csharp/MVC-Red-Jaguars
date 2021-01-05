using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class SubscriptionRepository : BaseRepository, ISubscriptionRepository
    {
        public SubscriptionRepository(IConfiguration config) : base(config) { }

        public void Add(Subscription subscription)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Subscription
                    (SubscriberUserProfileId, ProviderUserProfileId, BeginDateTime, EndDateTime)
                    OUTPUT INSERTED.ID
                    VALUES (@subUserId, @provUserId, @beginTime, @endTime)";
                    cmd.Parameters.AddWithValue("@subUserId", subscription.SubscriberUserProfileId);
                    cmd.Parameters.AddWithValue("@provUserId", subscription.ProviderUserProfileId);
                    cmd.Parameters.AddWithValue("@beginTime", subscription.BeginDateTime);
                    cmd.Parameters.AddWithValue("@endTime", 12/31/9999);
                    subscription.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        
    }
}
