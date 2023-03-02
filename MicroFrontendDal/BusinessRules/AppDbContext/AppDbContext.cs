/*
 * <Your-Product-Name>
 * Copyright (c) <Year-From>-<Year-To> <Your-Company-Name>
 *
 * Please configure this header in your SonarCloud/SonarQube quality profile.
 * You can also set it in SonarLint.xml additional file for SonarLint or standalone NuGet analyzer.
 */

using MicroFrontendDal.DTO.Management;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MicroFrontendDal.BusinessRules.AppDbContext
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public virtual DbSet<DtoSpGetAllUsers> GetAllUsers { get; set; }
        public virtual DbSet<DtoSpGetUserById> GetAllUserById { get; set; }
        public virtual DbSet<DtoSpGetTeamList> GetTeamList { get; set; }
        public virtual DbSet<DtoSpGetTaskBoard> GetTaskBoards { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=MicroFrontEndDb;Integrated Security=True");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
