using System;
using System.Collections.Generic;

namespace MicroFrontendDal.DataModels
{
    public partial class MasterPageRouting
    {
        public int MasterPageId { get; set; }
        public int RoleId { get; set; }
        public string MasterPageRoute { get; set; } = null!;
        public bool? IsDelete { get; set; }
    }
}
