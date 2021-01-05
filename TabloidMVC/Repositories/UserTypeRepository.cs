using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using TabloidMVC.Models;
using TabloidMVC.Utils;
using System.Collections.Generic;

namespace TabloidMVC.Repositories
{
    public class UserTypeRepository: BaseRepository, IUserTypeRepository
    {
        public UserTypeRepository(IConfiguration config) : base(config) { }
        public List<UserType> GetUserTypes()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT Id, Name
                         FROM UserType";

                    UserType userType = null;
                    var userTypes = new List<UserType>();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        userType = new UserType()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                        userTypes.Add(userType);
                    }

                    reader.Close();

                    return userTypes;
                }
            }
        }
    }
}
