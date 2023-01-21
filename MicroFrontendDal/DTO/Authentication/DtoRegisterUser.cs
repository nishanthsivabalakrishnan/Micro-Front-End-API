﻿using System;
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
}
