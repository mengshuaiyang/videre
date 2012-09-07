﻿using System.Collections.Generic;
//using System.Web.Script.Serialization;
//using Newtonsoft.Json;
using CodeEndeavors.Extensions.Serialization;

namespace Videre.Core.Models
{
    public class User
    {
        public User()
        {
            Roles = new List<string>();
            Attributes = new Dictionary<string, object>();
        }

        public string Id { get; set; }
        public string PortalId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Locale { get; set; }

        //[ScriptIgnore, JsonIgnore]
        [SerializeIgnore(new string[] { "db", "client" })]
        public string Password { get; set; }
        
        //[ScriptIgnore]
        [SerializeIgnore(new string[] { "client" })]
        public string PasswordHash { get; set; }

        [SerializeIgnore(new string[] { "client" })]
        public string PasswordSalt { get; set; }

        public List<string> Roles { get; set; }
        public Dictionary<string, object> Attributes { get; set; }

    }
}