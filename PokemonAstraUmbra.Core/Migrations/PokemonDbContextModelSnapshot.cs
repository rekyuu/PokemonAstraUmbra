﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PokemonAstraUmbra.Core.Database;

#nullable disable

namespace PokemonAstraUmbra.Core.Migrations
{
    [DbContext(typeof(PokemonDbContext))]
    partial class PokemonDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("PokemonAstraUmbra.Core.Items.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Cost")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FlingEffect")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FlingPower")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Pocket")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("PokemonAstraUmbra.Core.Pokemon.Ability", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Effect")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Abilities");
                });

            modelBuilder.Entity("PokemonAstraUmbra.Core.Pokemon.Move", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Accuracy")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DamageClass")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Effect")
                        .HasColumnType("INTEGER");

                    b.Property<float>("EffectChance")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("PokemonSpeciesId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PokemonSpeciesId1")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Power")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PowerPoints")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Priority")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PokemonSpeciesId");

                    b.HasIndex("PokemonSpeciesId1");

                    b.ToTable("Moves");
                });

            modelBuilder.Entity("PokemonAstraUmbra.Core.Pokemon.MoveLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MoveId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PokemonSpeciesId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MoveId");

                    b.HasIndex("PokemonSpeciesId");

                    b.ToTable("MoveLevel");
                });

            modelBuilder.Entity("PokemonAstraUmbra.Core.Pokemon.PokemonSpecies", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Ability1Id")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Ability2Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BaseExpYield")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BaseFriendship")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BaseStatsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CatchRate")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EffortValueYieldId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EggCycles")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EggGroup1")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EggGroup2")
                        .HasColumnType("INTEGER");

                    b.Property<float>("GenderMaleChance")
                        .HasColumnType("REAL");

                    b.Property<int>("GrowthRate")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Height")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("HeldItemId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("HiddenAbilityId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsBaby")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsGenderless")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("RareHeldItemId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type1")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type2")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Weight")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Ability1Id");

                    b.HasIndex("Ability2Id");

                    b.HasIndex("BaseStatsId");

                    b.HasIndex("EffortValueYieldId");

                    b.HasIndex("HeldItemId");

                    b.HasIndex("HiddenAbilityId");

                    b.HasIndex("RareHeldItemId");

                    b.ToTable("PokemonSpecies");
                });

            modelBuilder.Entity("PokemonAstraUmbra.Core.Pokemon.Stats", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Attack")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Defense")
                        .HasColumnType("INTEGER");

                    b.Property<int>("HitPoints")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SpecialAttack")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SpecialDefense")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Speed")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Stats");
                });

            modelBuilder.Entity("PokemonAstraUmbra.Core.Pokemon.Move", b =>
                {
                    b.HasOne("PokemonAstraUmbra.Core.Pokemon.PokemonSpecies", null)
                        .WithMany("EggMoves")
                        .HasForeignKey("PokemonSpeciesId");

                    b.HasOne("PokemonAstraUmbra.Core.Pokemon.PokemonSpecies", null)
                        .WithMany("MachineMoves")
                        .HasForeignKey("PokemonSpeciesId1");
                });

            modelBuilder.Entity("PokemonAstraUmbra.Core.Pokemon.MoveLevel", b =>
                {
                    b.HasOne("PokemonAstraUmbra.Core.Pokemon.Move", "Move")
                        .WithMany()
                        .HasForeignKey("MoveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PokemonAstraUmbra.Core.Pokemon.PokemonSpecies", null)
                        .WithMany("LevelMoves")
                        .HasForeignKey("PokemonSpeciesId");

                    b.Navigation("Move");
                });

            modelBuilder.Entity("PokemonAstraUmbra.Core.Pokemon.PokemonSpecies", b =>
                {
                    b.HasOne("PokemonAstraUmbra.Core.Pokemon.Ability", "Ability1")
                        .WithMany()
                        .HasForeignKey("Ability1Id");

                    b.HasOne("PokemonAstraUmbra.Core.Pokemon.Ability", "Ability2")
                        .WithMany()
                        .HasForeignKey("Ability2Id");

                    b.HasOne("PokemonAstraUmbra.Core.Pokemon.Stats", "BaseStats")
                        .WithMany()
                        .HasForeignKey("BaseStatsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PokemonAstraUmbra.Core.Pokemon.Stats", "EffortValueYield")
                        .WithMany()
                        .HasForeignKey("EffortValueYieldId");

                    b.HasOne("PokemonAstraUmbra.Core.Items.Item", "HeldItem")
                        .WithMany()
                        .HasForeignKey("HeldItemId");

                    b.HasOne("PokemonAstraUmbra.Core.Pokemon.Ability", "HiddenAbility")
                        .WithMany()
                        .HasForeignKey("HiddenAbilityId");

                    b.HasOne("PokemonAstraUmbra.Core.Items.Item", "RareHeldItem")
                        .WithMany()
                        .HasForeignKey("RareHeldItemId");

                    b.Navigation("Ability1");

                    b.Navigation("Ability2");

                    b.Navigation("BaseStats");

                    b.Navigation("EffortValueYield");

                    b.Navigation("HeldItem");

                    b.Navigation("HiddenAbility");

                    b.Navigation("RareHeldItem");
                });

            modelBuilder.Entity("PokemonAstraUmbra.Core.Pokemon.PokemonSpecies", b =>
                {
                    b.Navigation("EggMoves");

                    b.Navigation("LevelMoves");

                    b.Navigation("MachineMoves");
                });
#pragma warning restore 612, 618
        }
    }
}
