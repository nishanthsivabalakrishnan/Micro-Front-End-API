using System;
using System.Collections.Generic;

namespace MicroFrontendDal.DataModels
{
    public partial class Task
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; } = null!;
        public string TaskDetails { get; set; } = null!;
        public int Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDelete { get; set; }
        public int? UserId { get; set; }
        public int? AssignedBy { get; set; }
    }
}
