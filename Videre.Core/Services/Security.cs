﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Videre.Core.Services
{
    public class Security
    {
        public static List<Models.SecureActivity> GetSecureActivities()
        {
            return Repository.Current.GetResources<Models.SecureActivity>("SecureActivity").Select(m => m.Data).ToList();
        }
        public static Models.SecureActivity GetSecureActivity(string portalId, string area, string name)
        {
            //todo: hack to allow portalid to be empty should be taken out!
            return GetSecureActivities().Where(r => (string.IsNullOrEmpty(r.PortalId) || r.PortalId.Equals(portalId, StringComparison.InvariantCultureIgnoreCase)) && area.Equals(r.Area, StringComparison.InvariantCultureIgnoreCase) && name.Equals(r.Name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }
        public static Models.SecureActivity GetSecureActivityById(string Id)
        {
            return GetSecureActivities().Where(r => r.Id == Id).FirstOrDefault();
        }
        public static string Import(string portalId, Models.SecureActivity activity, Dictionary<string, string> idMap, string userId = null)
        {
            userId = string.IsNullOrEmpty(userId) ? Account.AuditId : userId;
            var existing = GetSecureActivity(portalId, activity.Area, activity.Name);
            activity.Id = existing != null ? existing.Id : null;
            activity.PortalId = portalId;
            activity.Roles = GetNewRoleIds(activity.Roles, idMap);
            return Save(activity, userId);
        }
        public static string Save(Models.SecureActivity activity, string userId = null)
        {
            userId = string.IsNullOrEmpty(userId) ? Account.AuditId : userId;
            Validate(activity);
            var res = Repository.Current.StoreResource("SecureActivity", null, activity, userId);
            return res.Id;
        }
        public static void Validate(Models.SecureActivity activity)
        {
            if (string.IsNullOrEmpty(activity.PortalId) || string.IsNullOrEmpty(activity.Area) || string.IsNullOrEmpty(activity.Name))
                throw new Exception(Localization.GetExceptionText("InvalidResource.Error", "{0} is invalid.", "SecureActivity"));
            if (IsDuplicate(activity))
                throw new Exception(Localization.GetExceptionText("DuplicateResource.Error", "{0} already exists.   Duplicates Not Allowed.", "SecureActivity"));
        }
        public static bool IsDuplicate(Models.SecureActivity activity)
        {
            var e = GetSecureActivity(activity.PortalId, activity.Area, activity.Name);
            return (e != null && e.Id != activity.Id);
        }
        public static bool Exists(Models.SecureActivity activity)
        {
            var m = GetSecureActivity(activity.PortalId, activity.Area, activity.Name);
            return (m != null);
        }
        public static bool DeleteSecureActivity(string id, string userId = null)
        {
            userId = string.IsNullOrEmpty(userId) ? Account.AuditId : userId;
            var res = Repository.Current.GetResourceById<Models.SecureActivity>(id);
            if (res != null)
                Repository.Current.Delete(res);
            return res != null;
        }
        public static void VerifyActivityAuthorized(string area, string name, string portalId = null)
        {
            portalId = string.IsNullOrEmpty(portalId) ? Portal.CurrentPortalId : portalId;
            var activity = GetSecureActivity(portalId, area, name);
            if (activity != null)
                Services.Account.VerifyInRole(activity.Roles);
        }
        public static bool IsActivityAuthorized(string area, string name, string portalId = null)
        {
            portalId = string.IsNullOrEmpty(portalId) ? Portal.CurrentPortalId : portalId;
            var activity = GetSecureActivity(portalId, area, name);
            if (activity != null)
                return Services.Account.IsInRole(activity.Roles, false);
            return false;
        }

        public static List<string> GetNewRoleIds(List<string> roleIds, Dictionary<string, string> map)
        {
            var newRoleIds = new List<string>();
            if (roleIds != null)
            {
                foreach (var roleId in roleIds)
                {
                    var newRoleId = Portal.GetIdMap<Models.Role>(roleId, map);
                    if (!string.IsNullOrEmpty(newRoleId))
                        newRoleIds.Add(newRoleId);
                }
            }
            return newRoleIds;
        }

    }
}