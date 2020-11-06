using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WISEroster.Mvc.ImplementationSpecific
{
    public enum WiseRole
    {
        ClassroomAdmin,
        AllFunctions
    }
}
//APPLICATION_GROUP APPLICATION_ID       ROLE_ID ACTION_ID
//-------------------- -------------------- ----------------------------------- ----------------------
//WISEROSTER WISEROSTER               ClassroomAdmin EditClassroom
//WISEROSTER WISEROSTER                ClassroomAdmin ViewClassroom
//WISEROSTER WISEROSTER_DPI       AllFunctions ActAsAnyAgency
//WISEROSTER WISEROSTER_DPI       AllFunctions ViewClassroom
//WISEROSTER WISEROSTER_DPI       AllFunctions ViewAdmin
//WISEROSTER WISEROSTER_DPI       AllFunctions EditAdmin