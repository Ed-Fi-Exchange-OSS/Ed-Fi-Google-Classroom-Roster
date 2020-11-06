using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Microsoft.ApplicationInsights;

namespace WISEroster.Mvc.ImplementationSpecific
{
    public interface ISessionInfo
    {
        void SetCurrentAgency(int edOrgId);
        int? CurrentAgencyId { get; }
        WamsPrincipal User { get; set; }
    }

    public class SessionInfo : ISessionInfo
    {
        HttpSessionStateWrapper _session;
        public SessionInfo(HttpSessionStateWrapper session)
        {
            _session = session;
        }

        public void SetCurrentAgency(int edOrgId)
        {
            CurrentAgencyId = edOrgId;
        }

        public int? CurrentAgencyId
        {
            get { return (int?) _session["CurrentAgencyId"];}
            set { _session["CurrentAgencyId"] = value; }
        }
        public WamsPrincipal User { get; set; }
    }
}