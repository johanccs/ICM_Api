﻿using AECI.ICM.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AECI.ICM.Data.Context
{
    public class ICMDbContext: DbContext
    {
        #region Constructor

        public ICMDbContext(DbContextOptions options):base(options)
        {
        }

        #endregion

        #region Properties

        public DbSet<Entities.ICM> ICM { get; set; }
        public DbSet<Entities.SectionDetail> SectionDetail{ get; set; }      

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder builder)
        {
        //    builder.Entity<ICMSectionDetail>()
        //        .HasKey(icm => new { icm.ICMId, icm.SectionId });
        //    builder.Entity<ICMSectionDetail>()
        //        .HasOne(icm => icm.ICM)
        //        .WithMany(b => b.ICMSectionDetails)
        //        .HasForeignKey(b => b.ICMId);
        //    builder.Entity<ICMSectionDetail>()
        //        .HasOne(b => b.SectionDetail)
        //        .WithMany(b => b.ICMSectionDetails)
        //        .HasForeignKey(b => b.SectionId);
        }

        #endregion
    }
}