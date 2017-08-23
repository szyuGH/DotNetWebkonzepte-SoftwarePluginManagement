using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SPM.Models;
using SPM.Models.UserEntities;

namespace SPM.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            

            builder.Entity<LicenseKey>()
                .HasOne(l => l.Software)
                .WithMany(s => s.LicenseKeys)
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Cascade)
                .IsRequired();
            builder.Entity<LicenseKey>()
                .HasOne(l => l.User)
                .WithMany(u => u.LicenseKeys)
                .IsRequired();

            builder.Entity<EditorUser>()
                .HasOne(e => e.Company)
                .WithMany(c => c.Editors)
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Cascade)
                .IsRequired();
            
            builder.Entity<Plugin>()
                .HasOne(s => s.RelatedSoftware)
                .WithMany(c => c.Plugins)
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Cascade)
                .IsRequired();

            builder.Entity<UsersPlugins>()
                .HasOne(up => up.User)
                .WithMany(nu => nu.Plugins)
                .IsRequired();
            builder.Entity<UsersPlugins>()
                .HasOne(up => up.Plugin);
            builder.Entity<UsersPlugins>()
                .HasOne(up => up.Software);
        }

        public DbSet<NormalUser> NormalUser { get; set; }
        public DbSet<CompanyUser> CompanyUser { get; set; }
        public DbSet<EditorUser> EditorUser { get; set; }


        public DbSet<SPM.Models.Software> Software { get; set; }
        public DbSet<SPM.Models.Plugin> Plugin { get; set; }
        public DbSet<UsersPlugins> UserPlugin { get; set; }
        public DbSet<LicenseKey> LicenseKey { get; set; }
    }
}
