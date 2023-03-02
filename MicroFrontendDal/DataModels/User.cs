using System;
using System.Collections.Generic;

namespace MicroFrontendDal.DataModels
{
    public partial class User
    {
        public int UserId { get; set; }
        public string AuthId { get; set; } = null!;
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Role { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsDelete { get; set; }
        public int? ReportsTo { get; set; }
    }
}
