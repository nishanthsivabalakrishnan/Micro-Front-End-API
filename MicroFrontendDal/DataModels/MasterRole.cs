using System;
using System.Collections.Generic;

namespace MicroFrontendDal.DataModels
{
    public partial class MasterRole
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
    }
}
