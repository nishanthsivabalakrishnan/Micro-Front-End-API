using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroFrontendDal.DTO.Authentication
{
    public class DtoRegisterUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class DtoUserRegistrationResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }
    public class DtoAdminRegisterNewUser
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ReportsTo { get; set; }
    }
    public class DtoAdminRegisterNewUserResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }
}
