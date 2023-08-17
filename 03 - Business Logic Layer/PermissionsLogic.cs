using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortsApi
{
    public class PermissionsLogic : BaseLogic
    {
        public FilePermission GetPermissions(string userName, int LayerID)
        {
            return DB.CommonMngFilesPermissions.Where(u => u.UserName == userName && u.LayerId == LayerID).Select(u => new FilePermission
            {
                ID = u.UserId,
                UserName = u.UserName,
                CanView = u.CanView,
                CanEdit = u.CanEdit,
                UserID = u.UserId,
                LayerID = u.LayerId
            }).SingleOrDefault();
        }
    }
}
