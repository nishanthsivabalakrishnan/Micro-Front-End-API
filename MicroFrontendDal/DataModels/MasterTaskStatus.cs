using System;
using System.Collections.Generic;

namespace MicroFrontendDal.DataModels
{
    public partial class MasterTaskStatus
    {
        public int TaskStatusId { get; set; }
        public string TaskStatus { get; set; } = null!;
        public bool IsDelete { get; set; }
    }
}
