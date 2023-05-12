using KartStatsV2.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KartStatsV2.Models;
using KartStatsV2.BLL.Interfaces;
using KartStatsV2.DAL.Interfaces;

namespace KartStatsV2.BLL
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;

        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public void AddGroup(Group group)
        {
            _groupRepository.AddGroup(group);
        }

        public List<Group> GetAllGroups()
        {
            return _groupRepository.GetAllGroups();
        }

        public Group GetGroup(int id)
        {
            return _groupRepository.GetGroup(id);
        }

        public bool UpdateGroup(Group group)
        {
            return _groupRepository.UpdateGroup(group);
        }

        public bool DeleteGroup(int groupId)
        {
            return _groupRepository.DeleteGroup(groupId);
        }

        public void LeaveGroup(int userId, int groupId)
        {
            _groupRepository.LeaveGroup(userId, groupId);
        }

        public void RemoveMember(int userId, int groupId)
        {
            _groupRepository.RemoveMember(userId, groupId);
        }

        public bool AddMember(int groupId, int userId)
        {
            return _groupRepository.AddMember(groupId, userId);
        }
    }
}