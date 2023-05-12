using KartStatsV2.DAL.Interfaces;
using KartStatsV2.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartStatsV2.DAL.Repositories
{
    public class InviteRepository : IInviteRepository
    {
        private readonly string _connectionString;

        public InviteRepository(IConfiguration config)
        {
            _connectionString = config.ConnectionString;
        }

        public void CreateInvite(Invite invite)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO Invites(GroupId, FromUserId, ToUserId, Status) VALUES(@GroupId, @FromUserId, @ToUserId, 'Pending')";
                    cmd.Parameters.AddWithValue("@GroupId", invite.GroupId);
                    cmd.Parameters.AddWithValue("@FromUserId", invite.FromUserId);
                    cmd.Parameters.AddWithValue("@ToUserId", invite.ToUserId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Invite> GetInvitesByToUserId(int toUserId)
        {
            List<Invite> invites = new List<Invite>();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM Invites WHERE ToUserId = @toUserId";
                    cmd.Parameters.AddWithValue("@toUserId", toUserId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Invite invite = new Invite
                            {
                                InviteId = Convert.ToInt32(reader["InviteId"]),
                                GroupId = Convert.ToInt32(reader["GroupId"]),
                                FromUserId = Convert.ToInt32(reader["FromUserId"]),
                                ToUserId = Convert.ToInt32(reader["ToUserId"]),
                                Status = reader["Status"].ToString()
                            };

                            invites.Add(invite);
                        }
                    }
                }
            }

            return invites;
        }

        public void UpdateInviteStatus(int inviteId, string status)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE Invites SET Status = @status WHERE InviteId = @inviteId";
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@inviteId", inviteId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void InviteUserToGroup(int groupId, string fromUserId, string toUserId)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO Invites (GroupId, FromUserId, ToUserId, Status) VALUES (@GroupId, @FromUserId, @ToUserId, 'Invited')";
                    cmd.Parameters.AddWithValue("@GroupId", groupId);
                    cmd.Parameters.AddWithValue("@FromUserId", fromUserId);
                    cmd.Parameters.AddWithValue("@ToUserId", toUserId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AcceptInvite(int inviteId)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE Invites SET Status = 'Accepted' WHERE InviteId = @InviteId";
                    cmd.Parameters.AddWithValue("@InviteId", inviteId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeclineInvite(int inviteId)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE Invites SET Status = 'Declined' WHERE InviteId = @InviteId";
                    cmd.Parameters.AddWithValue("@InviteId", inviteId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Invite GetInvite(int inviteId)
        {
            Invite invite = null;

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM Invites WHERE InviteId = @inviteId";
                    cmd.Parameters.AddWithValue("@inviteId", inviteId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            invite = new Invite
                            {
                                InviteId = Convert.ToInt32(reader["InviteId"]),
                                GroupId = Convert.ToInt32(reader["GroupId"]),
                                FromUserId = Convert.ToInt32(reader["FromUserId"]),
                                ToUserId = Convert.ToInt32(reader["ToUserId"]),
                                Status = reader["Status"].ToString()
                            };
                        }
                    }
                }
            }

            return invite;
        }

        public bool UpdateInvite(Invite invite)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE Invites SET GroupId = @groupId, FromUserId = @fromUserId, ToUserId = @toUserId, Status = @status WHERE InviteId = @inviteId";
                    cmd.Parameters.AddWithValue("@inviteId", invite.InviteId);
                    cmd.Parameters.AddWithValue("@groupId", invite.GroupId);
                    cmd.Parameters.AddWithValue("@fromUserId", invite.FromUserId);
                    cmd.Parameters.AddWithValue("@toUserId", invite.ToUserId);
                    cmd.Parameters.AddWithValue("@status", invite.Status);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
