using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class ReactionRepository: BaseRepository, IReactionRepository
    {
        public ReactionRepository(IConfiguration config) : base(config) { }

        public List<Reaction> GetReactions()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Name, ImageLocation FROM Reaction";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Reaction> reactions = new List<Reaction>();
                    while (reader.Read())
                    {
                        Reaction reaction = new Reaction
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageLocation = reader.GetString(reader.GetOrdinal("ImageLocation"))
                        };
                        reactions.Add(reaction);
                    }

                    reader.Close();

                    return reactions;
                }
            }
        }

    }
}
