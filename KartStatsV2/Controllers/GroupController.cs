using KartStatsV2.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KartStatsV2.Models;
using KartStatsV2.BLL.Interfaces;
using Org.BouncyCastle.Bcpg;
using System.Net;
using System.Text.RegularExpressions;

namespace KartStatsV2.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly IInviteService _inviteService;
        private readonly UserService _userService;

        public GroupController(IGroupService groupService, IInviteService inviteService)
        {
            _groupService = groupService;
            _inviteService = inviteService;

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _userService = new UserService(connectionString);

        }

        public ActionResult Index()
        {
            var groups = _groupService.GetAllGroups();
            return View(groups);  // We geven de lijst met groepen aan de view
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Models.Group group)
        {
            if (ModelState.IsValid)
            {
                _groupService.AddGroup(group);
                return RedirectToAction("Index");
            }

            return View(group);
        }

        public ActionResult Details(int id)
        {
            var group = _groupService.GetGroup(id);
            return View(group);  // We geven de groep aan de view
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var group = _groupService.GetGroup(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        [HttpPost]
        public ActionResult Edit(Models.Group group)
        {
            if (ModelState.IsValid)
            {
                _groupService.UpdateGroup(group);
                return RedirectToAction("Index");
            }
            return View(group);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var group = _groupService.GetGroup(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _groupService.DeleteGroup(id);
            return RedirectToAction("Index");
        }

        public ActionResult Invite(int groupId)
        {
            var model = new InviteViewModel
            {
                GroupId = groupId
            };

            return View();
        }

        public ActionResult Invites()
        {
            var userId = _userService.GetIdByUsername(_userService.GetUsername());
            var invites = _inviteService.GetInvitesByToUserId(userId);
            return View(invites);
        }

        [HttpPost]
        public ActionResult LeaveGroup(int groupId)
        {
            var group = _groupService.GetGroup(groupId);
            var currentUser = _userService.GetIdByUsername(_userService.GetUsername());

            if (group == null)
            {
                // Groep niet gevonden
                return HttpNotFound();
            }

            if (group.AdminUserId == currentUser)
            {
                // Beheerders kunnen hun eigen groep niet verlaten
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Beheerders kunnen hun eigen groep niet verlaten");
            }

            _groupService.LeaveGroup(currentUser, groupId);

            // Terug naar de lijst met groepen na het verlaten van de groep
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveMember(int groupId, int userId)
        {
            var group = _groupService.GetGroup(groupId);
            var currentUser = _userService.GetIdByUsername(_userService.GetUsername());

            if (group == null)
            {
                // Groep niet gevonden
                return HttpNotFound();
            }

            if (group.AdminUserId != currentUser)
            {
                // Alleen de beheerder kan leden verwijderen
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Alleen de beheerder kan leden verwijderen");
            }

            _groupService.RemoveMember(userId, groupId);

            // Terug naar de groepspagina na het verwijderen van een lid
            return RedirectToAction("Details", new { id = groupId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InviteMember(InviteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                                .Where(x => x.Value.Errors.Count > 0)
                                .Select(x => new { x.Key, x.Value.Errors })
                                .ToArray();

                // Breakpoint zetten of loggen van de 'errors' variabele
            }

            if (ModelState.IsValid)
            {
                var toUser = _userService.GetIdByUsername(model.ToUserName);
                if (toUser != null)
                {
                    var fromUserId = _userService.GetIdByUsername(_userService.GetUsername());
                    var invite = new Invite
                    {
                        GroupId = model.GroupId,
                        FromUserId = fromUserId,
                        ToUserId = toUser,
                        Status = InviteStatus.Pending.ToString()
                    };
                    _inviteService.CreateInvite(invite);
                    return RedirectToAction("Index", "Home"); // of waar je ook naartoe wilt na het succesvol uitnodigen van een gebruiker
                }
                else
                {
                    ModelState.AddModelError("", "Gebruikersnaam niet gevonden.");
                }
            }

            // Als we hier zijn, is er iets mis gegaan, dus laten we het formulier opnieuw weergeven
            return View(model);
        }



        public ActionResult AcceptInvite(int inviteId)
        {
            var invite = _inviteService.GetInvite(inviteId);

            // Controleer of de uitnodiging bestaat en of de huidige gebruiker de ontvanger is
            if (invite == null || invite.ToUserId != _userService.GetIdByUsername(_userService.GetUsername()))
            {
                return RedirectToAction("Index");
            }

            // Update de status van de uitnodiging naar Geaccepteerd
            invite.Status = InviteStatus.Accepted.ToString();
            _inviteService.UpdateInvite(invite);

            // Voeg de gebruiker toe aan de groep
            _groupService.AddMember(invite.GroupId, invite.ToUserId);

            return RedirectToAction("Index");
        }

        public ActionResult DeclineInvite(int inviteId)
        {
            var invite = _inviteService.GetInvite(inviteId);

            // Controleer of de uitnodiging bestaat en of de huidige gebruiker de ontvanger is
            if (invite == null || invite.ToUserId != _userService.GetIdByUsername(_userService.GetUsername()))
            {
                return RedirectToAction("Index");
            }

            // Update de status van de uitnodiging naar Geweigerd
            invite.Status = InviteStatus.Declined.ToString();
            _inviteService.UpdateInvite(invite);

            return RedirectToAction("Index");
        }



    }
}