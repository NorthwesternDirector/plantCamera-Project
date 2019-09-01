using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace plantCamera.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    CreateTime = table.Column<string>(nullable: true),
                    CreateUserIP = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    InviteCode = table.Column<string>(nullable: true),
                    LastLoginTime = table.Column<string>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LoginTimes = table.Column<int>(nullable: false),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "allpoint",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    年份 = table.Column<string>(nullable: true),
                    站点 = table.Column<string>(nullable: true),
                    注记 = table.Column<string>(nullable: true),
                    植物名 = table.Column<string>(nullable: true),
                    叶芽开始膨大期 = table.Column<string>(nullable: true),
                    叶芽开放期 = table.Column<string>(nullable: true),
                    花芽开始膨大期 = table.Column<string>(nullable: true),
                    花芽开放期 = table.Column<string>(nullable: true),
                    开始展叶期 = table.Column<string>(nullable: true),
                    展叶盛期 = table.Column<string>(nullable: true),
                    花序或花蕾出现期 = table.Column<string>(nullable: true),
                    开花始期 = table.Column<string>(nullable: true),
                    开花盛期 = table.Column<string>(nullable: true),
                    开花末期 = table.Column<string>(nullable: true),
                    第二次开花期 = table.Column<string>(nullable: true),
                    果实成熟期 = table.Column<string>(nullable: true),
                    果实脱落开始期 = table.Column<string>(nullable: true),
                    果实脱落末期 = table.Column<string>(nullable: true),
                    叶开始变色期 = table.Column<string>(nullable: true),
                    叶全部变色期 = table.Column<string>(nullable: true),
                    开始落叶期 = table.Column<string>(nullable: true),
                    落叶末期 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_allpoint", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AnalysisAirticle",
                columns: table => new
                {
                    aid = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Airticle = table.Column<string>(nullable: true),
                    AirticleDoc = table.Column<string>(nullable: true),
                    AirticleDocLink = table.Column<string>(nullable: true),
                    AirticlePhoto = table.Column<string>(nullable: true),
                    AirticlePhotoLink = table.Column<string>(nullable: true),
                    AirticleTime = table.Column<string>(nullable: true),
                    AirticleTitle = table.Column<string>(nullable: true),
                    AirticleUser = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisAirticle", x => x.aid);
                });

            migrationBuilder.CreateTable(
                name: "DataQuality",
                columns: table => new
                {
                    DataId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    dataCount = table.Column<int>(nullable: false),
                    lastYear = table.Column<string>(nullable: true),
                    latDiff = table.Column<float>(nullable: false),
                    lonDiff = table.Column<float>(nullable: false),
                    originalYear = table.Column<string>(nullable: true),
                    plantName = table.Column<string>(nullable: true),
                    pointCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataQuality", x => x.DataId);
                });

            migrationBuilder.CreateTable(
                name: "stationsLocation",
                columns: table => new
                {
                    站点 = table.Column<string>(nullable: false),
                    坐标经度 = table.Column<float>(nullable: false),
                    坐标纬度 = table.Column<float>(nullable: false),
                    省份 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stationsLocation", x => x.站点);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlbumA",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AlbumLink = table.Column<string>(nullable: true),
                    AlbumNumber = table.Column<int>(nullable: false),
                    PhotoNumber = table.Column<int>(nullable: false),
                    ProfileLink1 = table.Column<string>(nullable: true),
                    ProfileLink2 = table.Column<string>(nullable: true),
                    ProfileLink3 = table.Column<string>(nullable: true),
                    StarNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlbumA_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlbumB",
                columns: table => new
                {
                    AlbumBId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AlbumText = table.Column<string>(nullable: true),
                    AlbumTime = table.Column<string>(nullable: true),
                    AlbumsName = table.Column<string>(nullable: true),
                    CoverLink = table.Column<string>(nullable: true),
                    Id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumB", x => x.AlbumBId);
                    table.ForeignKey(
                        name: "FK_AlbumB_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IdentifyPhoto",
                columns: table => new
                {
                    IdentifyPhotoId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Id = table.Column<string>(nullable: true),
                    IdentifyName = table.Column<string>(nullable: true),
                    IdentifyPhotoLink = table.Column<string>(nullable: true),
                    IdentifyRant = table.Column<string>(nullable: true),
                    IdentifyTime = table.Column<string>(nullable: true),
                    sure = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentifyPhoto", x => x.IdentifyPhotoId);
                    table.ForeignKey(
                        name: "FK_IdentifyPhoto_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhotoInformation",
                columns: table => new
                {
                    PhotoInformationId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AlbumName = table.Column<string>(nullable: true),
                    Cover = table.Column<bool>(nullable: false),
                    DeleteFlag = table.Column<bool>(nullable: false),
                    Id = table.Column<string>(nullable: true),
                    LunarTime = table.Column<string>(nullable: true),
                    PhotoGrade = table.Column<int>(nullable: false),
                    PhotoHeight = table.Column<float>(nullable: false),
                    PhotoLatitude = table.Column<decimal>(nullable: false),
                    PhotoLink = table.Column<string>(nullable: true),
                    PhotoLongitude = table.Column<decimal>(nullable: false),
                    PhotoPhenology = table.Column<string>(nullable: false),
                    PhotoPlant = table.Column<string>(nullable: false),
                    PhotoSource = table.Column<string>(nullable: true),
                    PhotoTakeTime = table.Column<string>(nullable: false),
                    PhotoUploadTime = table.Column<string>(nullable: true),
                    PlantText = table.Column<string>(nullable: true),
                    Season = table.Column<string>(nullable: true),
                    Temperature = table.Column<float>(nullable: false),
                    Weather = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoInformation", x => x.PhotoInformationId);
                    table.ForeignKey(
                        name: "FK_PhotoInformation_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StarAirticle",
                columns: table => new
                {
                    StarAirticleId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Id = table.Column<string>(nullable: true),
                    StarTime = table.Column<string>(nullable: true),
                    aid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StarAirticle", x => x.StarAirticleId);
                    table.ForeignKey(
                        name: "FK_StarAirticle_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlbumB_Id",
                table: "AlbumB",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_IdentifyPhoto_Id",
                table: "IdentifyPhoto",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoInformation_Id",
                table: "PhotoInformation",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_StarAirticle_Id",
                table: "StarAirticle",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AlbumA");

            migrationBuilder.DropTable(
                name: "AlbumB");

            migrationBuilder.DropTable(
                name: "allpoint");

            migrationBuilder.DropTable(
                name: "AnalysisAirticle");

            migrationBuilder.DropTable(
                name: "DataQuality");

            migrationBuilder.DropTable(
                name: "IdentifyPhoto");

            migrationBuilder.DropTable(
                name: "PhotoInformation");

            migrationBuilder.DropTable(
                name: "StarAirticle");

            migrationBuilder.DropTable(
                name: "stationsLocation");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
