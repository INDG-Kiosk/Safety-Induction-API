﻿// <auto-generated />
using System;
using INSEE.KIOSK.API.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace INSEE.KIOSK.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220108174131_VW_DailyGuestSummaries")]
    partial class VW_DailyGuestSummaries
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Answer", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Code")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FK_QuestionCode")
                        .HasColumnType("int");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("ModifiedBy");

                    b.Property<DateTime>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("TextEN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TextSN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TextTA")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Code");

                    b.HasIndex("FK_QuestionCode");

                    b.HasIndex("ModifiedBy");

                    b.ToTable("M_Answer");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Company", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Code")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("ModifiedBy");

                    b.Property<DateTime>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Code");

                    b.HasIndex("ModifiedBy");

                    b.ToTable("M_Company");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Contractor_Master", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Code")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FK_CompanyCode")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("MailingAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("ModifiedBy");

                    b.Property<DateTime>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("NameEN")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("NameSN")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("NameTA")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Code");

                    b.HasIndex("FK_CompanyCode");

                    b.HasIndex("ModifiedBy");

                    b.ToTable("M_Contractor_Master");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Course", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Code")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FK_SiteCode")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("ModifiedBy");

                    b.Property<DateTime>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("PassRate")
                        .HasColumnType("decimal(16,4)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Video")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Code");

                    b.HasIndex("FK_SiteCode");

                    b.HasIndex("ModifiedBy");

                    b.ToTable("M_Course");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Course_Question", b =>
                {
                    b.Property<int>("FK_CourseCode")
                        .HasColumnType("int");

                    b.Property<int>("FK_QuestionCode")
                        .HasColumnType("int");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("ModifiedBy");

                    b.Property<DateTime>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("FK_CourseCode", "FK_QuestionCode");

                    b.HasIndex("FK_QuestionCode");

                    b.HasIndex("ModifiedBy");

                    b.ToTable("M_Course_Question");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Guest_Detail", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Code")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("FK_ContractorCode")
                        .HasColumnType("int");

                    b.Property<int>("FK_GuestMasterCode")
                        .HasColumnType("int");

                    b.Property<int?>("FK_SiteCode")
                        .HasColumnType("int");

                    b.Property<DateTime>("InsertedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProfileImg")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Code");

                    b.HasIndex("FK_ContractorCode");

                    b.HasIndex("FK_GuestMasterCode");

                    b.HasIndex("FK_SiteCode");

                    b.ToTable("T_Guest_Detail");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Guest_Detail_Attempt", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Code")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FK_GuestDetailCode")
                        .HasColumnType("int");

                    b.Property<int>("Print_Count")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("TestCompletedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TestStartedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TotalMarks")
                        .HasColumnType("decimal(16,4)");

                    b.HasKey("Code");

                    b.HasIndex("FK_GuestDetailCode");

                    b.ToTable("T_Guest_Detail_Attempt");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Guest_Master", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Code")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FK_CompanyCode")
                        .HasColumnType("int")
                        .HasColumnName("FK_CompanyCode");

                    b.Property<DateTime>("InsertedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("NIC")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Code");

                    b.HasIndex("FK_CompanyCode");

                    b.ToTable("T_Guest_Master");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Log", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Code")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FK_SiteCode")
                        .HasColumnType("int");

                    b.Property<DateTime>("InsertedDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Code");

                    b.HasIndex("FK_SiteCode");

                    b.ToTable("T_Log");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Question", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Code")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("ModifiedBy");

                    b.Property<DateTime>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("TextEN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TextSN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TextTA")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Code");

                    b.HasIndex("ModifiedBy");

                    b.ToTable("M_Question");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Settings", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Code")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FK_CompanyCode")
                        .HasColumnType("int");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("ModifiedBy");

                    b.Property<DateTime>("Modified_DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("PassValidPeridINMonthsForVisitor")
                        .HasColumnType("int");

                    b.Property<int>("PassValidPeridINMonthsForWorker")
                        .HasColumnType("int");

                    b.Property<int>("ReprintValidDaysForWorker")
                        .HasColumnType("int");

                    b.HasKey("Code");

                    b.HasIndex("FK_CompanyCode")
                        .IsUnique();

                    b.HasIndex("ModifiedBy");

                    b.ToTable("M_Settings");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Site", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Code")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FK_CompanyCode")
                        .HasColumnType("int");

                    b.Property<string>("IP")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("ModifiedBy");

                    b.Property<DateTime>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResourcePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Code");

                    b.HasIndex("FK_CompanyCode");

                    b.HasIndex("ModifiedBy");

                    b.ToTable("M_Site");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.VW_DailyGuestSummary", b =>
                {
                    b.Property<string>("PersonNIC")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Company")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExamCompleted")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExamStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ExamTotalMarks")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("PersonName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PersonProfileImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SiteCode")
                        .HasColumnType("int");

                    b.Property<string>("SiteLocation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SiteName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("inserted")
                        .HasColumnType("datetime2");

                    b.HasKey("PersonNIC");

                    b.ToView("VW_DailyGuestSummaries");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Model.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("UserId");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("M_User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("M_Role");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("M_Role_Claims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("T_User_Claims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("T_User_Login");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("M_User_Role");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("T_User_Token");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Answer", b =>
                {
                    b.HasOne("INSEE.KIOSK.API.Context.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("FK_QuestionCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("INSEE.KIOSK.API.Model.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.Navigation("Question");

                    b.Navigation("User");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Company", b =>
                {
                    b.HasOne("INSEE.KIOSK.API.Model.ApplicationUser", "User")
                        .WithMany("Companies")
                        .HasForeignKey("ModifiedBy");

                    b.Navigation("User");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Contractor_Master", b =>
                {
                    b.HasOne("INSEE.KIOSK.API.Context.Company", "Company")
                        .WithMany("Contractors_Master")
                        .HasForeignKey("FK_CompanyCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("INSEE.KIOSK.API.Model.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.Navigation("Company");

                    b.Navigation("User");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Course", b =>
                {
                    b.HasOne("INSEE.KIOSK.API.Context.Site", "Site")
                        .WithMany("Courses")
                        .HasForeignKey("FK_SiteCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("INSEE.KIOSK.API.Model.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.Navigation("Site");

                    b.Navigation("User");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Course_Question", b =>
                {
                    b.HasOne("INSEE.KIOSK.API.Context.Course", "Course")
                        .WithMany("Course_Questions")
                        .HasForeignKey("FK_CourseCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("INSEE.KIOSK.API.Context.Question", "Question")
                        .WithMany("Course_Questions")
                        .HasForeignKey("FK_QuestionCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("INSEE.KIOSK.API.Model.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.Navigation("Course");

                    b.Navigation("Question");

                    b.Navigation("User");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Guest_Detail", b =>
                {
                    b.HasOne("INSEE.KIOSK.API.Context.Contractor_Master", "Contractor_Master")
                        .WithMany("Guest_Details")
                        .HasForeignKey("FK_ContractorCode");

                    b.HasOne("INSEE.KIOSK.API.Context.Guest_Master", "Guest_Master")
                        .WithMany("Guest_Details")
                        .HasForeignKey("FK_GuestMasterCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("INSEE.KIOSK.API.Context.Site", "Site")
                        .WithMany("Guest_Details")
                        .HasForeignKey("FK_SiteCode");

                    b.Navigation("Contractor_Master");

                    b.Navigation("Guest_Master");

                    b.Navigation("Site");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Guest_Detail_Attempt", b =>
                {
                    b.HasOne("INSEE.KIOSK.API.Context.Guest_Detail", "Guest_Detail")
                        .WithMany("Guest_Detail_Attempts")
                        .HasForeignKey("FK_GuestDetailCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guest_Detail");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Guest_Master", b =>
                {
                    b.HasOne("INSEE.KIOSK.API.Context.Company", "Company")
                        .WithMany("Guest_Master")
                        .HasForeignKey("FK_CompanyCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Log", b =>
                {
                    b.HasOne("INSEE.KIOSK.API.Context.Site", "Site")
                        .WithMany("Logs")
                        .HasForeignKey("FK_SiteCode");

                    b.Navigation("Site");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Question", b =>
                {
                    b.HasOne("INSEE.KIOSK.API.Model.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.Navigation("User");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Settings", b =>
                {
                    b.HasOne("INSEE.KIOSK.API.Context.Company", "Company")
                        .WithOne("Settings")
                        .HasForeignKey("INSEE.KIOSK.API.Context.Settings", "FK_CompanyCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("INSEE.KIOSK.API.Model.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.Navigation("Company");

                    b.Navigation("User");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Site", b =>
                {
                    b.HasOne("INSEE.KIOSK.API.Context.Company", "Company")
                        .WithMany("Sites")
                        .HasForeignKey("FK_CompanyCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("INSEE.KIOSK.API.Model.ApplicationUser", "User")
                        .WithMany("Kioks")
                        .HasForeignKey("ModifiedBy");

                    b.Navigation("Company");

                    b.Navigation("User");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Model.ApplicationUser", b =>
                {
                    b.OwnsMany("INSEE.KIOSK.API.Model.RefreshToken", "RefreshTokens", b1 =>
                        {
                            b1.Property<string>("ApplicationUserId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<DateTime>("Created")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime>("Expires")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime?>("Revoked")
                                .HasColumnType("datetime2");

                            b1.Property<string>("Token")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ApplicationUserId", "Id");

                            b1.ToTable("RefreshToken");

                            b1.WithOwner()
                                .HasForeignKey("ApplicationUserId");
                        });

                    b.Navigation("RefreshTokens");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("INSEE.KIOSK.API.Model.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("INSEE.KIOSK.API.Model.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("INSEE.KIOSK.API.Model.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("INSEE.KIOSK.API.Model.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Company", b =>
                {
                    b.Navigation("Contractors_Master");

                    b.Navigation("Guest_Master");

                    b.Navigation("Settings");

                    b.Navigation("Sites");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Contractor_Master", b =>
                {
                    b.Navigation("Guest_Details");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Course", b =>
                {
                    b.Navigation("Course_Questions");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Guest_Detail", b =>
                {
                    b.Navigation("Guest_Detail_Attempts");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Guest_Master", b =>
                {
                    b.Navigation("Guest_Details");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Question", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("Course_Questions");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Context.Site", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("Guest_Details");

                    b.Navigation("Logs");
                });

            modelBuilder.Entity("INSEE.KIOSK.API.Model.ApplicationUser", b =>
                {
                    b.Navigation("Companies");

                    b.Navigation("Kioks");
                });
#pragma warning restore 612, 618
        }
    }
}
