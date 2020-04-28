﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ChessApplicationDbContext))]
    [Migration("20200428103432_EnabledNullValuesForEndGameInfoProperty.")]
    partial class EnabledNullValuesForEndGameInfoProperty
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ChessSystem.Domain.Entities.ChessBoardPosition", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Horizontal")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<int>("Vertical")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ChessBoardPositions");
                });

            modelBuilder.Entity("ChessSystem.Domain.Entities.ChessGame", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BlackPlayerId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EndGameInfo")
                        .HasColumnType("int");

                    b.Property<string>("WhitePlayerId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ChessGames");
                });

            modelBuilder.Entity("ChessSystem.Domain.Entities.Moves.CastlingMove", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ChessGameId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ColorOfTheFigures")
                        .HasColumnType("int");

                    b.Property<string>("KingInitialPositionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("KingTargetPositionnId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("OrderInTheGame")
                        .HasColumnType("int");

                    b.Property<string>("RookInitialPositionnId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RookTargetPositionnId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ChessGameId");

                    b.HasIndex("KingInitialPositionId")
                        .IsUnique()
                        .HasFilter("[KingInitialPositionId] IS NOT NULL");

                    b.HasIndex("KingTargetPositionnId")
                        .IsUnique()
                        .HasFilter("[KingTargetPositionnId] IS NOT NULL");

                    b.HasIndex("RookInitialPositionnId")
                        .IsUnique()
                        .HasFilter("[RookInitialPositionnId] IS NOT NULL");

                    b.HasIndex("RookTargetPositionnId")
                        .IsUnique()
                        .HasFilter("[RookTargetPositionnId] IS NOT NULL");

                    b.ToTable("CastlingMoves");
                });

            modelBuilder.Entity("ChessSystem.Domain.Entities.Moves.NormalMove", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ChessFigureColor")
                        .HasColumnType("int");

                    b.Property<int>("ChessFigureType")
                        .HasColumnType("int");

                    b.Property<string>("ChessGameId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("InitialPositionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("OrderInTheGame")
                        .HasColumnType("int");

                    b.Property<string>("TargetPositionId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ChessGameId");

                    b.HasIndex("InitialPositionId")
                        .IsUnique()
                        .HasFilter("[InitialPositionId] IS NOT NULL");

                    b.HasIndex("TargetPositionId")
                        .IsUnique()
                        .HasFilter("[TargetPositionId] IS NOT NULL");

                    b.ToTable("NormalMoves");
                });

            modelBuilder.Entity("ChessSystem.Domain.Entities.Moves.PawnProductionMove", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ChessBoardPositionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ChessGameId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ColorOFTheFigures")
                        .HasColumnType("int");

                    b.Property<int>("FigureThatHasBeenProduced")
                        .HasColumnType("int");

                    b.Property<int>("OrderInTheGame")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ChessBoardPositionId")
                        .IsUnique()
                        .HasFilter("[ChessBoardPositionId] IS NOT NULL");

                    b.HasIndex("ChessGameId");

                    b.ToTable("PawnProductionMoves");
                });

            modelBuilder.Entity("ChessSystem.Domain.Entities.OnlineUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("OnlineSince")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LogedInUsers");
                });

            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.DeviceFlowCodes", b =>
                {
                    b.Property<string>("UserCode")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasMaxLength(50000);

                    b.Property<string>("DeviceCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<DateTime?>("Expiration")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("SubjectId")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("UserCode");

                    b.HasIndex("DeviceCode")
                        .IsUnique();

                    b.HasIndex("Expiration");

                    b.ToTable("DeviceCodes");
                });

            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.PersistedGrant", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasMaxLength(50000);

                    b.Property<DateTime?>("Expiration")
                        .HasColumnType("datetime2");

                    b.Property<string>("SubjectId")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Key");

                    b.HasIndex("Expiration");

                    b.HasIndex("SubjectId", "ClientId", "Type");

                    b.ToTable("PersistedGrants");
                });

            modelBuilder.Entity("Infrastructure.Identity.ChessAppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

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
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
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

                    b.ToTable("AspNetRoleClaims");
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

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ChessSystem.Domain.Entities.Moves.CastlingMove", b =>
                {
                    b.HasOne("ChessSystem.Domain.Entities.ChessGame", "ChessGame")
                        .WithMany("CastlingMoves")
                        .HasForeignKey("ChessGameId");

                    b.HasOne("ChessSystem.Domain.Entities.ChessBoardPosition", "KingInitialPosition")
                        .WithOne()
                        .HasForeignKey("ChessSystem.Domain.Entities.Moves.CastlingMove", "KingInitialPositionId");

                    b.HasOne("ChessSystem.Domain.Entities.ChessBoardPosition", "KingTargetPosition")
                        .WithOne()
                        .HasForeignKey("ChessSystem.Domain.Entities.Moves.CastlingMove", "KingTargetPositionnId");

                    b.HasOne("ChessSystem.Domain.Entities.ChessBoardPosition", "RookInitialPosition")
                        .WithOne()
                        .HasForeignKey("ChessSystem.Domain.Entities.Moves.CastlingMove", "RookInitialPositionnId");

                    b.HasOne("ChessSystem.Domain.Entities.ChessBoardPosition", "RookTargetPosition")
                        .WithOne()
                        .HasForeignKey("ChessSystem.Domain.Entities.Moves.CastlingMove", "RookTargetPositionnId");
                });

            modelBuilder.Entity("ChessSystem.Domain.Entities.Moves.NormalMove", b =>
                {
                    b.HasOne("ChessSystem.Domain.Entities.ChessGame", "ChessGame")
                        .WithMany("NormalChessMoves")
                        .HasForeignKey("ChessGameId");

                    b.HasOne("ChessSystem.Domain.Entities.ChessBoardPosition", "InitialPosition")
                        .WithOne()
                        .HasForeignKey("ChessSystem.Domain.Entities.Moves.NormalMove", "InitialPositionId");

                    b.HasOne("ChessSystem.Domain.Entities.ChessBoardPosition", "TargetPosition")
                        .WithOne()
                        .HasForeignKey("ChessSystem.Domain.Entities.Moves.NormalMove", "TargetPositionId");
                });

            modelBuilder.Entity("ChessSystem.Domain.Entities.Moves.PawnProductionMove", b =>
                {
                    b.HasOne("ChessSystem.Domain.Entities.ChessBoardPosition", "ChessBoardPosition")
                        .WithOne()
                        .HasForeignKey("ChessSystem.Domain.Entities.Moves.PawnProductionMove", "ChessBoardPositionId");

                    b.HasOne("ChessSystem.Domain.Entities.ChessGame", "ChessGame")
                        .WithMany("PawnProductionMoves")
                        .HasForeignKey("ChessGameId");
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
                    b.HasOne("Infrastructure.Identity.ChessAppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Infrastructure.Identity.ChessAppUser", null)
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

                    b.HasOne("Infrastructure.Identity.ChessAppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Infrastructure.Identity.ChessAppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
