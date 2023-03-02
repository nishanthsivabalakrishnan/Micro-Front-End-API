using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroFrontendDal.DTO.Authentication
{
    public class DtoLoginUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class DtoTokenResponse
    {
        public int Status { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public DateTime ValidTill { get; set; }
        public string Route { get; set; }
    }
}
