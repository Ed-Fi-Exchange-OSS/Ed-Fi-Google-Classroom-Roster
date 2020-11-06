using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WISEroster.Mvc.Models
{
    public class GcPreferenceViewModel
    {
        public bool AllowExternalDomains { get; set; } //teacher/student emails must match GcUserEmail domain
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string GcUserEmail { get; set; }
        public string CredentialMessage { get; set; }

    }
}