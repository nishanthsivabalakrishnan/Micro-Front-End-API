using System;
using System.Collections.Generic;

namespace MicroFrontendDal.DataModels
{
    public partial class UserDetail
    {
        public int UserDetailsId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string DateOfBirth { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string CollegeName { get; set; } = null!;
        public string Percentage { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string WorkLocation { get; set; } = null!;
        public string? About { get; set; }
    }
}
