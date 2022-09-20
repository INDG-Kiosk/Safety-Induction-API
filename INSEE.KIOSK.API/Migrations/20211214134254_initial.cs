using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace INSEE.KIOSK.API.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "M_Role",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "M_User",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "M_Role_Claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_Role_Claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_M_Role_Claims_M_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "M_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "M_Company",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_Company", x => x.Code);
                    table.ForeignKey(
                        name: "FK_M_Company_M_User_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "M_User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "M_Question",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextEN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextSN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextTA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_Question", x => x.Code);
                    table.ForeignKey(
                        name: "FK_M_Question_M_User_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "M_User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "M_User_Role",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_User_Role", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_M_User_Role_M_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "M_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_M_User_Role_M_User_UserId",
                        column: x => x.UserId,
                        principalTable: "M_User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Revoked = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => new { x.ApplicationUserId, x.Id });
                    table.ForeignKey(
                        name: "FK_RefreshToken_M_User_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "M_User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_User_Claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_User_Claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_User_Claims_M_User_UserId",
                        column: x => x.UserId,
                        principalTable: "M_User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_User_Login",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_User_Login", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_T_User_Login_M_User_UserId",
                        column: x => x.UserId,
                        principalTable: "M_User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_User_Token",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_User_Token", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_T_User_Token_M_User_UserId",
                        column: x => x.UserId,
                        principalTable: "M_User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "M_Contractor_Master",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameSN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameTA = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MailingAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FK_CompanyCode = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_Contractor_Master", x => x.Code);
                    table.ForeignKey(
                        name: "FK_M_Contractor_Master_M_Company_FK_CompanyCode",
                        column: x => x.FK_CompanyCode,
                        principalTable: "M_Company",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_M_Contractor_Master_M_User_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "M_User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "M_Settings",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReprintValidDaysForWorker = table.Column<int>(type: "int", nullable: false),
                    PassValidPeridINMonthsForWorker = table.Column<int>(type: "int", nullable: false),
                    PassValidPeridINMonthsForVisitor = table.Column<int>(type: "int", nullable: false),
                    FK_CompanyCode = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Modified_DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_Settings", x => x.Code);
                    table.ForeignKey(
                        name: "FK_M_Settings_M_Company_FK_CompanyCode",
                        column: x => x.FK_CompanyCode,
                        principalTable: "M_Company",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_M_Settings_M_User_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "M_User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "M_Site",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IP = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    ResourcePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FK_CompanyCode = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_Site", x => x.Code);
                    table.ForeignKey(
                        name: "FK_M_Site_M_Company_FK_CompanyCode",
                        column: x => x.FK_CompanyCode,
                        principalTable: "M_Company",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_M_Site_M_User_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "M_User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_Guest_Master",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    NIC = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    FK_CompanyCode = table.Column<int>(type: "int", nullable: false),
                    InsertedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Guest_Master", x => x.Code);
                    table.ForeignKey(
                        name: "FK_T_Guest_Master_M_Company_FK_CompanyCode",
                        column: x => x.FK_CompanyCode,
                        principalTable: "M_Company",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "M_Answer",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextEN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextSN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextTA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    FK_QuestionCode = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_Answer", x => x.Code);
                    table.ForeignKey(
                        name: "FK_M_Answer_M_Question_FK_QuestionCode",
                        column: x => x.FK_QuestionCode,
                        principalTable: "M_Question",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_M_Answer_M_User_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "M_User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "M_Course",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Video = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PassRate = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    FK_SiteCode = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_Course", x => x.Code);
                    table.ForeignKey(
                        name: "FK_M_Course_M_Site_FK_SiteCode",
                        column: x => x.FK_SiteCode,
                        principalTable: "M_Site",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_M_Course_M_User_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "M_User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_Log",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsertedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FK_SiteCode = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Log", x => x.Code);
                    table.ForeignKey(
                        name: "FK_T_Log_M_Site_FK_SiteCode",
                        column: x => x.FK_SiteCode,
                        principalTable: "M_Site",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_Guest_Detail",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ProfileImg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FK_SiteCode = table.Column<int>(type: "int", nullable: true),
                    FK_GuestMasterCode = table.Column<int>(type: "int", nullable: false),
                    FK_ContractorCode = table.Column<int>(type: "int", nullable: true),
                    InsertedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Guest_Detail", x => x.Code);
                    table.ForeignKey(
                        name: "FK_T_Guest_Detail_M_Contractor_Master_FK_ContractorCode",
                        column: x => x.FK_ContractorCode,
                        principalTable: "M_Contractor_Master",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_Guest_Detail_M_Site_FK_SiteCode",
                        column: x => x.FK_SiteCode,
                        principalTable: "M_Site",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_Guest_Detail_T_Guest_Master_FK_GuestMasterCode",
                        column: x => x.FK_GuestMasterCode,
                        principalTable: "T_Guest_Master",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "M_Course_Question",
                columns: table => new
                {
                    FK_QuestionCode = table.Column<int>(type: "int", nullable: false),
                    FK_CourseCode = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_Course_Question", x => new { x.FK_CourseCode, x.FK_QuestionCode });
                    table.ForeignKey(
                        name: "FK_M_Course_Question_M_Course_FK_CourseCode",
                        column: x => x.FK_CourseCode,
                        principalTable: "M_Course",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_M_Course_Question_M_Question_FK_QuestionCode",
                        column: x => x.FK_QuestionCode,
                        principalTable: "M_Question",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_M_Course_Question_M_User_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "M_User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_Guest_Detail_Attempt",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestStartedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalMarks = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    Print_Count = table.Column<int>(type: "int", nullable: false),
                    TestCompletedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FK_GuestDetailCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Guest_Detail_Attempt", x => x.Code);
                    table.ForeignKey(
                        name: "FK_T_Guest_Detail_Attempt_T_Guest_Detail_FK_GuestDetailCode",
                        column: x => x.FK_GuestDetailCode,
                        principalTable: "T_Guest_Detail",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_M_Answer_FK_QuestionCode",
                table: "M_Answer",
                column: "FK_QuestionCode");

            migrationBuilder.CreateIndex(
                name: "IX_M_Answer_ModifiedBy",
                table: "M_Answer",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_M_Company_ModifiedBy",
                table: "M_Company",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_M_Contractor_Master_FK_CompanyCode",
                table: "M_Contractor_Master",
                column: "FK_CompanyCode");

            migrationBuilder.CreateIndex(
                name: "IX_M_Contractor_Master_ModifiedBy",
                table: "M_Contractor_Master",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_M_Course_FK_SiteCode",
                table: "M_Course",
                column: "FK_SiteCode");

            migrationBuilder.CreateIndex(
                name: "IX_M_Course_ModifiedBy",
                table: "M_Course",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_M_Course_Question_FK_QuestionCode",
                table: "M_Course_Question",
                column: "FK_QuestionCode");

            migrationBuilder.CreateIndex(
                name: "IX_M_Course_Question_ModifiedBy",
                table: "M_Course_Question",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_M_Question_ModifiedBy",
                table: "M_Question",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "M_Role",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_M_Role_Claims_RoleId",
                table: "M_Role_Claims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_M_Settings_FK_CompanyCode",
                table: "M_Settings",
                column: "FK_CompanyCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_M_Settings_ModifiedBy",
                table: "M_Settings",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_M_Site_FK_CompanyCode",
                table: "M_Site",
                column: "FK_CompanyCode");

            migrationBuilder.CreateIndex(
                name: "IX_M_Site_ModifiedBy",
                table: "M_Site",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "M_User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "M_User",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_M_User_Role_RoleId",
                table: "M_User_Role",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Guest_Detail_FK_ContractorCode",
                table: "T_Guest_Detail",
                column: "FK_ContractorCode");

            migrationBuilder.CreateIndex(
                name: "IX_T_Guest_Detail_FK_GuestMasterCode",
                table: "T_Guest_Detail",
                column: "FK_GuestMasterCode");

            migrationBuilder.CreateIndex(
                name: "IX_T_Guest_Detail_FK_SiteCode",
                table: "T_Guest_Detail",
                column: "FK_SiteCode");

            migrationBuilder.CreateIndex(
                name: "IX_T_Guest_Detail_Attempt_FK_GuestDetailCode",
                table: "T_Guest_Detail_Attempt",
                column: "FK_GuestDetailCode");

            migrationBuilder.CreateIndex(
                name: "IX_T_Guest_Master_FK_CompanyCode",
                table: "T_Guest_Master",
                column: "FK_CompanyCode");

            migrationBuilder.CreateIndex(
                name: "IX_T_Log_FK_SiteCode",
                table: "T_Log",
                column: "FK_SiteCode");

            migrationBuilder.CreateIndex(
                name: "IX_T_User_Claims_UserId",
                table: "T_User_Claims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_User_Login_UserId",
                table: "T_User_Login",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "M_Answer");

            migrationBuilder.DropTable(
                name: "M_Course_Question");

            migrationBuilder.DropTable(
                name: "M_Role_Claims");

            migrationBuilder.DropTable(
                name: "M_Settings");

            migrationBuilder.DropTable(
                name: "M_User_Role");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "T_Guest_Detail_Attempt");

            migrationBuilder.DropTable(
                name: "T_Log");

            migrationBuilder.DropTable(
                name: "T_User_Claims");

            migrationBuilder.DropTable(
                name: "T_User_Login");

            migrationBuilder.DropTable(
                name: "T_User_Token");

            migrationBuilder.DropTable(
                name: "M_Course");

            migrationBuilder.DropTable(
                name: "M_Question");

            migrationBuilder.DropTable(
                name: "M_Role");

            migrationBuilder.DropTable(
                name: "T_Guest_Detail");

            migrationBuilder.DropTable(
                name: "M_Contractor_Master");

            migrationBuilder.DropTable(
                name: "M_Site");

            migrationBuilder.DropTable(
                name: "T_Guest_Master");

            migrationBuilder.DropTable(
                name: "M_Company");

            migrationBuilder.DropTable(
                name: "M_User");
        }
    }
}
