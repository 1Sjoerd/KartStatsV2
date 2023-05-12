using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KartStatsV2.Models;

namespace KartStatsV2.BLL.Interfaces
{
    public interface IGroupService
    {
        List<Group> GetAllGroups();
        Group GetGroup(int id);
        void AddGroup(Group group);
        bool UpdateGroup(Group group);
        bool DeleteGroup(int groupId);
        void LeaveGroup(int userId, int groupId);
        void RemoveMember(int userId, int groupId);
        bool AddMember(int userId, int groupId);
    }
}