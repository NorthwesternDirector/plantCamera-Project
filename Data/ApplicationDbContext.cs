using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using plantCamera.Models;
using plantCamera.Models.DataViewModels;

namespace plantCamera.Data
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
        }

        public DbSet<AlbumA> AlbumA { get; set; }

        public DbSet<AlbumB> AlbumB { get; set; }

        public DbSet<AnalysisAirticle> AnalysisAirticle { get; set; }

        public DbSet<IdentifyPhoto> IdentifyPhoto { get; set; }

        public DbSet<StarAirticle> StarAirticle { get; set; }

        public DbSet<PhotoInformation> PhotoInformation { get; set; }

        public DbSet<allpoint> allpoint { get; set; }

        public DbSet<stationsLocation> stationsLocation { get; set; }

        public DbSet<DataQuality> DataQuality { get; set; }
    }
}
