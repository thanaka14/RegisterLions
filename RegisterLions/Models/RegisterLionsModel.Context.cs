﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RegisterLions.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class RegisterLionsEntities : DbContext
    {
        public RegisterLionsEntities()
            : base("name=RegisterLionsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<ClubStatus> ClubStatus { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<MemberMovement> MemberMovements { get; set; }
        public virtual DbSet<MembershipType> MembershipTypes { get; set; }
        public virtual DbSet<Movement> Movements { get; set; }
        public virtual DbSet<MultipleDistrict> MultipleDistricts { get; set; }
        public virtual DbSet<Officer> Officers { get; set; }
        public virtual DbSet<TUser> TUsers { get; set; }
        public virtual DbSet<Committee> Committees { get; set; }
        public virtual DbSet<CommitteeDetail> CommitteeDetails { get; set; }
        public virtual DbSet<CommitteeOfficer> CommitteeOfficesr { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseDetail> CourseDetails { get; set; }
        public virtual DbSet<ZoneClub> ZoneClubs { get; set; }
        public virtual DbSet<ZoneOfficer> ZoneOfficers { get; set; }
        public virtual DbSet<ClubOfficer> ClubOfficers { get; set; }
        public virtual DbSet<RegionOfficer> RegionOfficers { get; set; }
        public virtual DbSet<Club> Clubs { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<ApplicationLog> ApplicationLogs { get; set; }
        public virtual DbSet<OfficerGroup> OfficerGroups { get; set; }
    }
}
