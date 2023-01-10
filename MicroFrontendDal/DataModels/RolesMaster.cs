using System;
using System.Collections.Generic;

namespace MicroFrontendDal.DataModels
{
    public partial class RolesMaster
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public string CreatedOn { get; set; } = null!;
        public string UpdatedOn { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
