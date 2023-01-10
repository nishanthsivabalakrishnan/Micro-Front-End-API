using System;
using System.Collections.Generic;

namespace MicroFrontendDal.DataModels
{
    public partial class MasterInformation
    {
        public int MasterId { get; set; }
        public string InformationName { get; set; } = null!;
        public string? Value { get; set; }
    }
}
