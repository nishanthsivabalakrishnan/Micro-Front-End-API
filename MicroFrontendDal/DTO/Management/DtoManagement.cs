/*
 * <Your-Product-Name>
 * Copyright (c) <Year-From>-<Year-To> <Your-Company-Name>
 *
 * Please configure this header in your SonarCloud/SonarQube quality profile.
 * You can also set it in SonarLint.xml additional file for SonarLint or standalone NuGet analyzer.
 */

using System.ComponentModel.DataAnnotations;

namespace MicroFrontendDal.DTO.Management
{
    public class DtoManagement
    {
    }
    public class DtoSpGetAllUsers
    {
        [Key]
        public int UserId { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public string RoleName { get; set; }
        public string? Name { get; set; }
        public string? ReportsTo { get; set; }
    }
    public class DtoSpGetUserById
    {
        [Key]
        public int UserId { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public string RoleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? ReportsTo { get; set; }
        public int Role { get; set; }
    }
    public class DtoReportingPersonList
    {
        public int UserId { get; set; }
        public string Name { get; set; }
    }
    public class DtoGetuserByIdresponse
    {
        public DtoSpGetUserById UserDetails { get; set; }
        public List<DtoReportingPersonList> ReportingList { get; set; }
    }
    public class DtoSpGetTeamList
    {
        [Key]
        public int UserId { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public string RoleName { get; set; }
        public string? Name { get; set; }
        public string? ReportsTo { get; set; }
    }
    public class DtoPostTask
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDetails { get; set; }
        public int TaskStatus { get; set; }
        public int UserId { get; set; }
        public int AssignedBy { get; set; }
    }
    public class DtoSpGetTaskBoard
    {
        [Key]
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDetails { get; set; }
        public int Status { get; set; }
        public string TaskStatus { get; set; }
        public string Name { get; set; }
    }
    public class DtoChangeTaskStatus
    {
        public int TaskId { get; set; }
        public int TaskStatus { get; set; }
    }
    public class DtoPostUserDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string CollegeName { get; set; }
        public string Percentage { get; set; }
        public string Designation { get; set; }
        public string WorkLocation { get; set; }
        public string About { get; set; }
        public int Projects { get; set; }
        public int Experience { get; set; }
        public int UserId { get; set; }
    }
}
