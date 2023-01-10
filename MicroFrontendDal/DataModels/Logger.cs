﻿using System;
using System.Collections.Generic;

namespace MicroFrontendDal.DataModels
{
    public partial class Logger
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? Level { get; set; }
        public string? Message { get; set; }
        public string? Exception { get; set; }
    }
}
