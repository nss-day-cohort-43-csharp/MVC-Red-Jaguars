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
                    cmd.Parameters.AddWithValue("@endTime", DateTime.MaxValue);
                    subscription.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public List<Subscription> GetUserSubscriptions(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT Id, SubscriberUserProfileId, ProviderUserProfileId, BeginDateTime, EndDateTime
                    FROM Subscription
                    WHERE SubscriberUserProfileId = @subUserProfileId";

                    cmd.Parameters.AddWithValue("@subUserProfileId", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Subscription> subscriptions = new List<Subscription>();

                    
                        while (reader.Read())
                        {
                            Subscription subscription = new Subscription()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                SubscriberUserProfileId = reader.GetInt32(reader.GetOrdinal("SubscriberUserProfileId")),
                                ProviderUserProfileId = reader.GetInt32(reader.GetOrdinal("ProviderUserProfileId")),
                                BeginDateTime = reader.GetDateTime(reader.GetOrdinal("BeginDateTime")),
                                EndDateTime = reader.GetDateTime(reader.GetOrdinal("EndDateTime"))
                            };
                            subscriptions.Add(subscription);
                        }

                    List<Subscription> currentSubscriptions = new List<Subscription>();

                    foreach (Subscription sub in subscriptions)
                    {
                        if (sub.EndDateTime > DateTime.Now)
                        {
                            currentSubscriptions.Add(sub);
                        }
                    }

                    reader.Close();
                    return currentSubscriptions;                    
                }
            }            
        }

        public Subscription GetSubscriptionById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT Id, SubscriberUserProfileId, ProviderUserProfileId, BeginDateTime, EndDateTime
                    FROM Subscription
                    WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Subscription subscription = new Subscription
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            SubscriberUserProfileId = reader.GetInt32(reader.GetOrdinal("SubscriberUserProfileId")),
                            ProviderUserProfileId = reader.GetInt32(reader.GetOrdinal("ProviderUserProfileId")),
                            BeginDateTime = reader.GetDateTime(reader.GetOrdinal("BeginDateTime")),
                            EndDateTime = reader.GetDateTime(reader.GetOrdinal("EndDateTime"))
                        };
                        reader.Close();
                        return subscription;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }

        public void Edit(Subscription subscription)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    UPDATE Subscription
                    SET EndDateTime = @now
                    WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@now", DateTime.Now);
                    cmd.Parameters.AddWithValue("@id", subscription.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        
    }
}
