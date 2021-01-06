using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class PostReactionRepository: BaseRepository, IPostReactionRepository
    {
        public PostReactionRepository(IConfiguration config) : base(config) { }
        public List<PostReaction> GetPostReactionsByPostId(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT r.Id, r.PostId, r.ReactionId, r.UserProfileId
                                        FROM PostReaction r
                                        LEFT JOIN Post p ON p.Id = r.PostId
                                        LEFT JOIN Reaction e ON e.Id = r.ReactionId
                                        LEFT JOIN UserProfile u ON u.Id = r.UserProfileId
                                        WHERE r.PostId = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<PostReaction> reactions = new List<PostReaction>();
                    while (reader.Read())
                    {
                        PostReaction reaction = new PostReaction
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            ReactionId = reader.GetInt32(reader.GetOrdinal("ReactionId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId"))
                        };
                        reactions.Add(reaction);
                    }

                    reader.Close();

                    return reactions;
                }
            }
        }

        public void AddPostReaction(PostReaction postReaction)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO PostReaction (
                            PostId, ReactionId, UserProfileId )
                        OUTPUT INSERTED.ID
                        VALUES (
                            @PostId, @ReactionId, @UserProfileId )";
                    cmd.Parameters.AddWithValue("@PostId", postReaction.PostId);
                    cmd.Parameters.AddWithValue("@ReactionId", postReaction.ReactionId);
                    cmd.Parameters.AddWithValue("@UserProfileId", postReaction.UserProfileId);

                    postReaction.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}
