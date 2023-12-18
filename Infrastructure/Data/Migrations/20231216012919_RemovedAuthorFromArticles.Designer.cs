﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231216012919_RemovedAuthorFromArticles")]
    partial class RemovedAuthorFromArticles
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Article", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("boolean");

                    b.Property<string>("RedirectArticleId")
                        .HasColumnType("character varying(512)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.HasKey("Id");

                    b.HasIndex("RedirectArticleId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("Domain.Entities.ArticleCategory", b =>
                {
                    b.Property<string>("ArticleId")
                        .HasColumnType("character varying(512)");

                    b.Property<string>("CategoryId")
                        .HasColumnType("character varying(512)");

                    b.HasKey("ArticleId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("ArticleCategories");
                });

            modelBuilder.Entity("Domain.Entities.Author", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("Domain.Entities.Category", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ParentId")
                        .HasColumnType("character varying(512)");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Domain.Entities.Revision", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ArticleId")
                        .IsRequired()
                        .HasColumnType("character varying(512)");

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("PreviousRevisionId")
                        .HasColumnType("uuid");

                    b.Property<string>("Review")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ReviewTimestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Reviewer")
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("PreviousRevisionId");

                    b.ToTable("Revisions");
                });

            modelBuilder.Entity("Domain.Entities.Article", b =>
                {
                    b.HasOne("Domain.Entities.Article", "RedirectArticle")
                        .WithMany()
                        .HasForeignKey("RedirectArticleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("RedirectArticle");
                });

            modelBuilder.Entity("Domain.Entities.ArticleCategory", b =>
                {
                    b.HasOne("Domain.Entities.Article", "Article")
                        .WithMany("ArticleCategories")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Category", "Category")
                        .WithMany("CategoryArticles")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Domain.Entities.Category", b =>
                {
                    b.HasOne("Domain.Entities.Category", "Parent")
                        .WithMany("SubCategories")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Domain.Entities.Revision", b =>
                {
                    b.HasOne("Domain.Entities.Article", "Article")
                        .WithMany("Revisions")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Revision", "PreviousRevision")
                        .WithMany()
                        .HasForeignKey("PreviousRevisionId");

                    b.Navigation("Article");

                    b.Navigation("Author");

                    b.Navigation("PreviousRevision");
                });

            modelBuilder.Entity("Domain.Entities.Article", b =>
                {
                    b.Navigation("ArticleCategories");

                    b.Navigation("Revisions");
                });

            modelBuilder.Entity("Domain.Entities.Category", b =>
                {
                    b.Navigation("CategoryArticles");

                    b.Navigation("SubCategories");
                });
#pragma warning restore 612, 618
        }
    }
}
