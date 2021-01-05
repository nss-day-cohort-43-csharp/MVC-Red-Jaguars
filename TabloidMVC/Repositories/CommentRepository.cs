using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(IConfiguration config) : base(config) { }
        public List<Comment> GetCommentByPostId(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT c.Id, c.PostId, c.UserProfileId, c.Subject,
                              c.Content, c.CreateDateTime, p.Id AS PostId,
                              p.Title, p.Content AS PostContent,     
                              p.ImageLocation AS HeaderImage,
                              p.CreateDateTime AS PostDateTime, p.PublishDateTime, p.IsApproved,
                              p.CategoryId,
                              u.FirstName, u.LastName, u.DisplayName, 
                              u.Email, u.CreateDateTime, u.ImageLocation AS AvatarImage,
                              u.UserTypeId, 
                              ut.[Name] AS UserTypeName
                         FROM Comment c
                         LEFT JOIN Post p ON c.PostId = p.Id
                         LEFT JOIN UserProfile u ON c.UserProfileId = u.id
                         LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE c.PostId = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    List<Comment> comments = new List<Comment>();


                    while (reader.Read())
                    {
                        comments.Add(NewCommentFromReader(reader));
                    }
                    reader.Close();

                    return comments;
                }
            }
        }
        public Comment GetCommentById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT c.Id, c.PostId, c.UserProfileId, c.Subject,
                              c.Content, c.CreateDateTime, p.Id AS PostId,
                              p.Title, p.Content AS PostContent,     
                              p.ImageLocation AS HeaderImage,
                              p.CreateDateTime AS PostDateTime, p.PublishDateTime, p.IsApproved,
                              p.CategoryId,
                              u.FirstName, u.LastName, u.DisplayName, 
                              u.Email, u.CreateDateTime, u.ImageLocation AS AvatarImage,
                              u.UserTypeId, 
                              ut.[Name] AS UserTypeName
                         FROM Comment c
                         LEFT JOIN Post p ON c.PostId = p.Id
                         LEFT JOIN UserProfile u ON c.UserProfileId = u.id
                         LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE c.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    Comment comments = null;


                    if (reader.Read())
                    {
                        comments = NewCommentFromReader(reader);
                    }
                    reader.Close();

                    return comments;
                }
            }
        }
        private Comment NewCommentFromReader(SqlDataReader reader)
        {
            Comment comment = new Comment()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                Subject = reader.GetString(reader.GetOrdinal("Subject")),
                Content = reader.GetString(reader.GetOrdinal("Content")),
                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                UserProfile = new UserProfile()
                {
                    Id = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    ImageLocation = DbUtils.GetNullableString(reader, "AvatarImage"),
                    UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                    UserType = new UserType()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                        Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                    }
                },
                Post = new Post()
                {
                    Id = reader.GetInt32(reader.GetOrdinal("PostId")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    Content = reader.GetString(reader.GetOrdinal("PostContent")),
                    ImageLocation = DbUtils.GetNullableString(reader, "HeaderImage"),
                    CreateDateTime = reader.GetDateTime(reader.GetOrdinal("PostDateTime")),
                    PublishDateTime = DbUtils.GetNullableDateTime(reader, "PublishDateTime"),
                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),

                }
            };
            return comment;
        }
        public void AddComment(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO Comment (PostId, UserProfileId, Subject, Content, CreateDateTime)
                                        OUTPUT INSERTED.ID
                                        VALUES (@PostId, @UserProfileId, @Subject, @Content, convert(datetime,@CreateDateTime,1))
";
                    cmd.Parameters.AddWithValue("@PostId", comment.PostId);
                    cmd.Parameters.AddWithValue("@UserProfileId", comment.UserProfileId);
                    cmd.Parameters.AddWithValue("@Subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@Content", comment.Content);
                    cmd.Parameters.AddWithValue("@CreateDateTime", comment.CreateDateTime);

                    comment.Id = (int)cmd.ExecuteScalar();
                }
            }

        }
        public void DeleteComment(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Comment
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }

        }
        public void UpdateComment(Comment comment, int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                UPDATE Comment
                                SET
                                Subject = @Subject,
                                Content = @Content
                                WHERE Id = @id
";
                    cmd.Parameters.AddWithValue("@Subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@Content", comment.Content);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }


    }
}
