using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KartStatsV2.Models;

namespace KartStatsV2.DAL.Interfaces
{
    public interface IGroupRepository
    {
        List<Group> GetAllGroups();
        Group GetGroup(int id);
        void AddGroup(Group group);
        bool UpdateGroup(Group group);
        bool DeleteGroup(int groupId);
        string GetGroupAdmin(int groupId);
        List<string> GetGroupMembers(int groupId);
        void AddGroupMember(int groupId, string userId);
        void RemoveGroupMember(int groupId, string userId);
        void LeaveGroup(int userId, int groupId);
        void RemoveMember(int userId, int groupId);

        bool AddMember(int userId, int groupId);
    }
}
