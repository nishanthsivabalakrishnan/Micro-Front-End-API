/*
 * <Your-Product-Name>
 * Copyright (c) <Year-From>-<Year-To> <Your-Company-Name>
 *
 * Please configure this header in your SonarCloud/SonarQube quality profile.
 * You can also set it in SonarLint.xml additional file for SonarLint or standalone NuGet analyzer.
 */

namespace MicroFrontendDal.DTO.Common
{
    public class DtoFrontendData
    {
        public string ObjInputString { get; set; }
    }
    public class DtoFrontendGetData
    {
        public dynamic Data { get; set; }
    }
}
