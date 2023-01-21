using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroFrontendDal.DTO.Authentication
{
    public class DtoVerifyEmail
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
    public class DtoVerifyEmailResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }
}
