using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using plantCamera.Data;

namespace plantCamera.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("plantCamera.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("CreateTime");

                    b.Property<string>("CreateUserIP");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("InviteCode");

                    b.Property<string>("LastLoginTime");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<int>("LoginTimes");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("plantCamera.Models.DataViewModels.AlbumA", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("AlbumLink");

                    b.Property<int>("AlbumNumber");

                    b.Property<int>("PhotoNumber");

                    b.Property<string>("ProfileLink1");

                    b.Property<string>("ProfileLink2");

                    b.Property<string>("ProfileLink3");

                    b.Property<int>("StarNumber");

                    b.HasKey("Id");

                    b.ToTable("AlbumA");
                });

            modelBuilder.Entity("plantCamera.Models.DataViewModels.AlbumB", b =>
                {
                    b.Property<int>("AlbumBId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AlbumText");

                    b.Property<string>("AlbumTime");

                    b.Property<string>("AlbumsName");

                    b.Property<string>("CoverLink");

                    b.Property<string>("Id");

                    b.HasKey("AlbumBId");

                    b.HasIndex("Id");

                    b.ToTable("AlbumB");
                });

            modelBuilder.Entity("plantCamera.Models.DataViewModels.allpoint", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("年份");

                    b.Property<string>("站点");

                    b.Property<string>("注记");

                    b.Property<string>("植物名");

                    b.Property<string>("叶芽开始膨大期");

                    b.Property<string>("叶芽开放期");

                    b.Property<string>("花芽开始膨大期");

                    b.Property<string>("花芽开放期");

                    b.Property<string>("开始展叶期");

                    b.Property<string>("展叶盛期");

                    b.Property<string>("花序或花蕾出现期");

                    b.Property<string>("开花始期");

                    b.Property<string>("开花盛期");

                    b.Property<string>("开花末期");

                    b.Property<string>("第二次开花期");

                    b.Property<string>("果实成熟期");

                    b.Property<string>("果实脱落开始期");

                    b.Property<string>("果实脱落末期");

                    b.Property<string>("叶开始变色期");

                    b.Property<string>("叶全部变色期");

                    b.Property<string>("开始落叶期");

                    b.Property<string>("落叶末期");

                    b.HasKey("id");

                    b.ToTable("allpoint");
                });

            modelBuilder.Entity("plantCamera.Models.DataViewModels.AnalysisAirticle", b =>
                {
                    b.Property<int>("aid")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Airticle");

                    b.Property<string>("AirticleDoc");

                    b.Property<string>("AirticleDocLink");

                    b.Property<string>("AirticlePhoto");

                    b.Property<string>("AirticlePhotoLink");

                    b.Property<string>("AirticleTime");

                    b.Property<string>("AirticleTitle");

                    b.Property<string>("AirticleUser");

                    b.Property<string>("Author");

                    b.HasKey("aid");

                    b.ToTable("AnalysisAirticle");
                });

            modelBuilder.Entity("plantCamera.Models.DataViewModels.DataQuality", b =>
                {
                    b.Property<int>("DataId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("dataCount");

                    b.Property<string>("lastYear");

                    b.Property<float>("latDiff");

                    b.Property<float>("lonDiff");

                    b.Property<string>("originalYear");

                    b.Property<string>("plantName");

                    b.Property<int>("pointCount");

                    b.HasKey("DataId");

                    b.ToTable("DataQuality");
                });

            modelBuilder.Entity("plantCamera.Models.DataViewModels.IdentifyPhoto", b =>
                {
                    b.Property<int>("IdentifyPhotoId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Id");

                    b.Property<string>("IdentifyName");

                    b.Property<string>("IdentifyPhotoLink");

                    b.Property<string>("IdentifyRant");

                    b.Property<string>("IdentifyTime");

                    b.Property<bool>("sure");

                    b.HasKey("IdentifyPhotoId");

                    b.HasIndex("Id");

                    b.ToTable("IdentifyPhoto");
                });

            modelBuilder.Entity("plantCamera.Models.DataViewModels.PhotoInformation", b =>
                {
                    b.Property<int>("PhotoInformationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AlbumName");

                    b.Property<bool>("Cover");

                    b.Property<bool>("DeleteFlag");

                    b.Property<string>("Id");

                    b.Property<string>("LunarTime");

                    b.Property<int>("PhotoGrade");

                    b.Property<float>("PhotoHeight");

                    b.Property<decimal>("PhotoLatitude");

                    b.Property<string>("PhotoLink");

                    b.Property<decimal>("PhotoLongitude");

                    b.Property<string>("PhotoPhenology")
                        .IsRequired();

                    b.Property<string>("PhotoPlant")
                        .IsRequired();

                    b.Property<string>("PhotoSource");

                    b.Property<string>("PhotoTakeTime")
                        .IsRequired();

                    b.Property<string>("PhotoUploadTime");

                    b.Property<string>("PlantText");

                    b.Property<string>("Season");

                    b.Property<float>("Temperature");

                    b.Property<string>("Weather")
                        .IsRequired();

                    b.HasKey("PhotoInformationId");

                    b.HasIndex("Id");

                    b.ToTable("PhotoInformation");
                });

            modelBuilder.Entity("plantCamera.Models.DataViewModels.StarAirticle", b =>
                {
                    b.Property<int>("StarAirticleId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Id");

                    b.Property<string>("StarTime");

                    b.Property<int>("aid");

                    b.HasKey("StarAirticleId");

                    b.HasIndex("Id");

                    b.ToTable("StarAirticle");
                });

            modelBuilder.Entity("plantCamera.Models.DataViewModels.stationsLocation", b =>
                {
                    b.Property<string>("站点")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("坐标经度");

                    b.Property<float>("坐标纬度");

                    b.Property<string>("省份");

                    b.HasKey("站点");

                    b.ToTable("stationsLocation");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("plantCamera.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("plantCamera.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("plantCamera.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("plantCamera.Models.DataViewModels.AlbumA", b =>
                {
                    b.HasOne("plantCamera.Models.ApplicationUser", "ApplicationUser")
                        .WithOne("AlbumA")
                        .HasForeignKey("plantCamera.Models.DataViewModels.AlbumA", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("plantCamera.Models.DataViewModels.AlbumB", b =>
                {
                    b.HasOne("plantCamera.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("Id");
                });

            modelBuilder.Entity("plantCamera.Models.DataViewModels.IdentifyPhoto", b =>
                {
                    b.HasOne("plantCamera.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("Id");
                });

            modelBuilder.Entity("plantCamera.Models.DataViewModels.PhotoInformation", b =>
                {
                    b.HasOne("plantCamera.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("Id");
                });

            modelBuilder.Entity("plantCamera.Models.DataViewModels.StarAirticle", b =>
                {
                    b.HasOne("plantCamera.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("Id");
                });
        }
    }
}
