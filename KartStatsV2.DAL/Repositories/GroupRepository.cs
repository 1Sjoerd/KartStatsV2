using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KartStatsV2.Models;
using KartStatsV2.DAL.Interfaces;
using System.Web;

namespace KartStatsV2.DAL.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly string _connectionString;

        public GroupRepository(IConfiguration config)
        {
            _connectionString = config.ConnectionString;
        }

        public void AddGroup(Group group)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO Groups(Name, AdminUserId) VALUES(@Name, @AdminUserId)";
                    cmd.Parameters.AddWithValue("@Name", group.Name);
                    cmd.Parameters.AddWithValue("@AdminUserId", HttpContext.Current.Session["Id"] as int?);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Group> GetAllGroups()
        {
            List<Group> groups = new List<Group>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM Groups";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Group group = new Group
                            {
                                GroupId = Convert.ToInt32(reader["GroupId"]),
                                Name = reader["Name"].ToString(),
                                AdminUserId = Convert.ToInt32(reader["AdminUserId"])
                            };

                            groups.Add(group);
                        }
                    }
                }
            }

            return groups;
        }

        public Group GetGroup(int id)
        {
            Group group = null;

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM Groups WHERE GroupId = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            group = new Group
                            {
                                GroupId = Convert.ToInt32(reader["GroupId"]),
                                Name = reader["Name"].ToString(),
                                AdminUserId = Convert.ToInt32(reader["AdminUserId"])
                            };
                        }
                    }
                }
            }

            return group;
        }

        public bool UpdateGroup(Group group)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("UPDATE Groups SET Name = @Name, AdminUserId = @AdminUserId WHERE GroupId = @GroupId", conn);
                    cmd.Parameters.AddWithValue("@Name", group.Name);
                    cmd.Parameters.AddWithValue("@AdminUserId", HttpContext.Current.Session["Id"] as int?);
                    cmd.Parameters.AddWithValue("@GroupId", group.GroupId);

                    int affectedRows = cmd.ExecuteNonQuery();

                    return affectedRows > 0;
                }
            }
            catch (Exception ex)
            {
                // Log de fout
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool DeleteGroup(int groupId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM Groups WHERE GroupId = @GroupId", conn);
                    cmd.Parameters.AddWithValue("@GroupId", groupId);

                    int affectedRows = cmd.ExecuteNonQuery();

                    return affectedRows > 0;
                }
            }
            catch (Exception ex)
            {
                // Log de fout
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public string GetGroupAdmin(int groupId)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT AdminUserId FROM Groups WHERE GroupId = @GroupId";
                    cmd.Parameters.AddWithValue("@GroupId", groupId);

                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        public List<string> GetGroupMembers(int groupId)
        {
            List<string> members = new List<string>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT UserId FROM GroupMembers WHERE GroupId = @GroupId";
                    cmd.Parameters.AddWithValue("@GroupId", groupId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            members.Add(reader["UserId"].ToString());
                        }
                    }
                }
            }

            return members;
        }

        public void AddGroupMember(int groupId, string userId)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO GroupMembers (GroupId, UserId) VALUES (@GroupId, @UserId)";
                    cmd.Parameters.AddWithValue("@GroupId", groupId);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemoveGroupMember(int groupId, string userId)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM GroupMembers WHERE GroupId = @GroupId AND UserId = @UserId";
                    cmd.Parameters.AddWithValue("@GroupId", groupId);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void LeaveGroup(int userId, int groupId)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM GroupMembers WHERE UserId = @UserId AND GroupId = @GroupId";
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@GroupId", groupId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemoveMember(int userId, int groupId)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM GroupMembers WHERE UserId = @UserId AND GroupId = @GroupId";
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@GroupId", groupId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool AddMember(int groupId, int userId)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO GroupMembers(GroupId, UserId) VALUES(@groupId, @userId)";
                    cmd.Parameters.AddWithValue("@groupId", groupId);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
