/*
 * <Your-Product-Name>
 * Copyright (c) <Year-From>-<Year-To> <Your-Company-Name>
 *
 * Please configure this header in your SonarCloud/SonarQube quality profile.
 * You can also set it in SonarLint.xml additional file for SonarLint or standalone NuGet analyzer.
 */

using MicroFrontendDal.BusinessRules.Logger;
using MicroFrontendDal.DataModels;
using MicroFrontendDal.DTO.Common;
using MicroFrontendDal.DTO.Management;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static MicroFrontendDal.BusinessConstants.BusinessConstant;

namespace MicroFrontendDal.BusinessRules.Management
{
    public class Management : IManagement
    {
        #region Variable Region
        private readonly Log Logger;
        private MicroFrontEndDbContext DbContext { get; set; }
        private AppDbContext.AppDbContext Context { get; set; }
        private const string FileName = "Management";
        #endregion

        #region Constructor
        public Management(UserManager<IdentityUser> userManager, Log log)
        {
            DbContext = new MicroFrontEndDbContext();
            Logger = log;
            Context = new AppDbContext.AppDbContext();
        }
        #endregion

        public List<DtoSpGetAllUsers> GetAllUsers()
        {
            try
            {
                var users = Context.GetAllUsers.FromSqlRaw("USP_Admin_GetAllUsers").ToList();
                return users;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "GetAllUsers", ex);
                throw;
            }
        }
        public List<DtoReportingPersonList> GetReportingList(int roleId)
        {
            try
            {
                List<DtoReportingPersonList> reportingList = new();
                var repotingId = DbContext.MasterRoles.FirstOrDefault(x => x.RoleId == roleId).ReportingTo;
                var reportingPersons = DbContext.Users.Where(x => x.Role == repotingId && x.IsDelete != true).ToList();
                foreach (var person in reportingPersons)
                {
                    DtoReportingPersonList reporting = new()
                    {
                        Name = person.FirstName + ' ' + person.LastName,
                        UserId = person.UserId
                    };
                    reportingList.Add(reporting);
                }
                return reportingList;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "GetAllUsers", ex);
                throw;
            }
        }
        public DtoGetuserByIdresponse GetUserDetailsWithManagerList(int id)
        {
            try
            {
                var userDetails = Context.GetAllUserById.FromSqlRaw("USP_Admin_GetUserDetailById {0}", id).ToList();
                var reportingList = GetReportingList(userDetails.FirstOrDefault().Role).ToList();
                DtoGetuserByIdresponse response = new()
                {
                    UserDetails = userDetails.FirstOrDefault(),
                    ReportingList = reportingList
                };
                return response;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "GetUserDetailsWithManagerList", ex);
                throw;
            }
        }
        public List<DtoSpGetTeamList> GetTeamList(int Id)
        {
            try
            {
                var users = Context.GetTeamList.FromSqlRaw("USP_Lead_GetAllTeamList {0}", Id).ToList();
                return users;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "GetTeamList", ex);
                throw;
            }
        }
        public DtoResponse PostTask(DtoPostTask taskDetails)
        {
            try
            {
                if (taskDetails != null)
                {
                    if (taskDetails.TaskId != 0)
                    {
                        var task = DbContext.Tasks.FirstOrDefault(x => x.TaskId == taskDetails.TaskId);
                        task.TaskName = taskDetails.TaskName;
                        task.TaskDetails = taskDetails.TaskDetails;
                        task.UserId = taskDetails.UserId;
                        task.Status = taskDetails.TaskStatus;
                        task.AssignedBy = taskDetails.AssignedBy;
                        task.UpdatedOn = DateTime.Now;
                        DbContext.SaveChanges();
                        DtoResponse response = new()
                        {
                            Status = ResponseStatus.Success,
                            Message = CustomMessages.CM021
                        };
                        return response;
                    }
                    else
                    {
                        DataModels.Task task = new()
                        {
                            TaskName = taskDetails.TaskName,
                            TaskDetails = taskDetails.TaskDetails,
                            Status = taskDetails.TaskStatus,
                            CreatedOn = DateTime.Now,
                            IsDelete = false,
                            UserId = taskDetails.UserId,
                            AssignedBy = taskDetails.AssignedBy,
                        };
                        DbContext.Tasks.Add(task);
                        DbContext.SaveChanges();
                        DtoResponse response = new()
                        {
                            Status = ResponseStatus.Success,
                            Message = CustomMessages.CM020
                        };
                        return response;
                    }
                }
                else
                {
                    DtoResponse response = new()
                    {
                        Status = ResponseStatus.Failed,
                        Message = CustomMessages.CM022
                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "PostTask", ex);
                throw;
            }
        }
        public List<DtoSpGetTaskBoard> GetTaskBoard(int Id)
        {
            try
            {
                var taskList = Context.GetTaskBoards.FromSqlRaw("USP_Lead_GetTaskBoardById {0}", Id).ToList();
                return taskList;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "GetTaskBoard", ex);
                throw;
            }
        }
        public DtoResponse DeleteTaskById(int Id)
        {
            try
            {
                if (Id != 0)
                {
                    var task = DbContext.Tasks.FirstOrDefault(x => x.TaskId == Id);
                    task.IsDelete = true;
                    DbContext.SaveChanges();
                    DtoResponse response = new()
                    {
                        Status = ResponseStatus.Success,
                        Message = CustomMessages.CM023
                    };
                    return response;
                }
                else
                {
                    DtoResponse response = new()
                    {
                        Status = ResponseStatus.Failed,
                        Message = CustomMessages.CM024
                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "DeleteTaskById", ex);
                throw;
            }
        }
        public DtoResponse ChangeTaskStatus(DtoChangeTaskStatus taskStatus)
        {
            try
            {
                if (taskStatus.TaskId != 0)
                {
                    var task = DbContext.Tasks.FirstOrDefault(x => x.TaskId == taskStatus.TaskId);
                    task.Status = taskStatus.TaskStatus;
                    task.UpdatedOn = DateTime.Now;
                    DbContext.SaveChanges();
                    DtoResponse response = new()
                    {
                        Status = ResponseStatus.Success,
                        Message = CustomMessages.CM025
                    };
                    return response;
                }
                else
                {
                    DtoResponse response = new()
                    {
                        Status = ResponseStatus.Failed,
                        Message = CustomMessages.CM026
                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "ChangeTaskStatus", ex);
                throw;
            }
        }
        public DtoResponse PostUserProfileDetails(DtoPostUserDetails dtoUserDetails)
        {
            try
            {
                if (dtoUserDetails.UserId != 0)
                {
                    var userDetails = DbContext.UserDetails.FirstOrDefault(x => x.UserId == dtoUserDetails.UserId);
                    if (userDetails == null)
                    {
                        UserDetail userDetail = new UserDetail()
                        {
                            UserId = dtoUserDetails.UserId,
                            About = dtoUserDetails.About,
                            Address = dtoUserDetails.Address,
                            City = dtoUserDetails.City,
                            CollegeName = dtoUserDetails.CollegeName,
                            Country = dtoUserDetails.Country,
                            DateOfBirth = dtoUserDetails.DateOfBirth,
                            Designation = dtoUserDetails.Designation,
                            Email = dtoUserDetails.Email,
                            FirstName = dtoUserDetails.FirstName,
                            LastName = dtoUserDetails.LastName,
                            Percentage = dtoUserDetails.Percentage,
                            PostalCode = dtoUserDetails.PostalCode,
                            WorkLocation = dtoUserDetails.WorkLocation
                        };
                        DbContext.UserDetails.Add(userDetail);
                        DbContext.SaveChanges();
                    }
                    else
                    {
                        userDetails.About = dtoUserDetails.About;
                        userDetails.Address = dtoUserDetails.Address;
                        userDetails.City = dtoUserDetails.City;
                        userDetails.Country = dtoUserDetails.Country;
                        userDetails.Email = dtoUserDetails.Email;
                        userDetails.FirstName = dtoUserDetails.FirstName;
                        userDetails.LastName = dtoUserDetails.LastName;
                        userDetails.Percentage = dtoUserDetails.Percentage;
                        userDetails.WorkLocation = dtoUserDetails.WorkLocation;
                        userDetails.DateOfBirth = dtoUserDetails.DateOfBirth;
                        userDetails.CollegeName = dtoUserDetails.CollegeName;
                        userDetails.Designation = dtoUserDetails.Designation;
                        userDetails.PostalCode = dtoUserDetails.PostalCode;
                        DbContext.SaveChanges();
                    }

                    DtoResponse successfulResponse = new()
                    {
                        Status = ResponseStatus.Success,
                        Message = CustomMessages.CM029
                    };
                    return successfulResponse;
                }
                DtoResponse response = new()
                {
                    Status = ResponseStatus.Failed,
                    Message = CustomMessages.CM030
                };
                return response;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "ChangeTaskStatus", ex);
                throw;
            }
        }
        public UserDetail GetUserDetailsById(int id)
        {
            try
            {
                if(id!=0)
                {
                    UserDetail userDetail = DbContext.UserDetails.FirstOrDefault(x => x.UserId == id);
                    return userDetail;
                }
                return new UserDetail();
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "GetUserDetailsById", ex);
                throw;
            }
        }
    }
}
