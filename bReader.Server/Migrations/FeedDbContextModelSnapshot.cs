﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using bReader.Server.Data;

namespace bReader.Server.Migrations
{
    [DbContext(typeof(FeedDbContext))]
    partial class FeedDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.8");

            modelBuilder.Entity("bReader.Shared.Models.AppSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("bReader.Shared.Models.Feed", b =>
                {
                    b.Property<int>("Pk")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Authors")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Categories")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Copyright")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Documentation")
                        .HasColumnType("TEXT");

                    b.Property<string>("Generator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("GroupId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsFavorite")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("LastUpdatedTime")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Links")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SubscribeLink")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UnreadCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("Pk");

                    b.HasIndex("GroupId");

                    b.HasIndex("SubscribeLink")
                        .IsUnique();

                    b.ToTable("Feeds");
                });

            modelBuilder.Entity("bReader.Shared.Models.FeedGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Groups");

                    b.HasData(
                        new
                        {
                            Id = -1,
                            Name = "默认"
                        });
                });

            modelBuilder.Entity("bReader.Shared.Models.FeedItem", b =>
                {
                    b.Property<int>("Pk")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Authors")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Categories")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsFavorite")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsRead")
                        .HasColumnType("INTEGER");

                    b.Property<long>("LastUpdatedTime")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Links")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("PublishDate")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SourceFeedPk")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Pk");

                    b.HasIndex("SourceFeedPk");

                    b.ToTable("FeedItems");
                });

            modelBuilder.Entity("bReader.Shared.Models.Feed", b =>
                {
                    b.HasOne("bReader.Shared.Models.FeedGroup", "Group")
                        .WithMany("Feeds")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("bReader.Shared.Models.FeedItem", b =>
                {
                    b.HasOne("bReader.Shared.Models.Feed", "SourceFeed")
                        .WithMany("Items")
                        .HasForeignKey("SourceFeedPk")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SourceFeed");
                });

            modelBuilder.Entity("bReader.Shared.Models.Feed", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("bReader.Shared.Models.FeedGroup", b =>
                {
                    b.Navigation("Feeds");
                });
#pragma warning restore 612, 618
        }
    }
}
