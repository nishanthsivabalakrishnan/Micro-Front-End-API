using System;
using System.Collections.Generic;

namespace MicroFrontendDal.DataModels
{
    public partial class MasterStatus
    {
        public int StatusId { get; set; }
        public string Status { get; set; } = null!;
    }
}
