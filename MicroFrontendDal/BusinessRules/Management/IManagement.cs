/*
 * <Your-Product-Name>
 * Copyright (c) <Year-From>-<Year-To> <Your-Company-Name>
 *
 * Please configure this header in your SonarCloud/SonarQube quality profile.
 * You can also set it in SonarLint.xml additional file for SonarLint or standalone NuGet analyzer.
 */

using MicroFrontendDal.DataModels;
using MicroFrontendDal.DTO.Common;
using MicroFrontendDal.DTO.Management;

namespace MicroFrontendDal.BusinessRules.Management
{
    public interface IManagement
    {
        List<DtoSpGetAllUsers> GetAllUsers();
        List<DtoReportingPersonList> GetReportingList(int roleId);
        DtoGetuserByIdresponse GetUserDetailsWithManagerList(int id);
        List<DtoSpGetTeamList> GetTeamList(int Id);
        DtoResponse PostTask(DtoPostTask taskDetails);
        List<DtoSpGetTaskBoard> GetTaskBoard(int Id);
        DtoResponse DeleteTaskById(int Id);
        DtoResponse ChangeTaskStatus(DtoChangeTaskStatus taskStatus);
        DtoResponse PostUserProfileDetails(DtoPostUserDetails dtoUserDetails);
        UserDetail GetUserDetailsById(int id);
    }
}
