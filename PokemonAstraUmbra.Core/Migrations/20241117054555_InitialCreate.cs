using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonAstraUmbra.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Effect = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Cost = table.Column<int>(type: "INTEGER", nullable: false),
                    FlingPower = table.Column<int>(type: "INTEGER", nullable: false),
                    FlingEffect = table.Column<int>(type: "INTEGER", nullable: false),
                    Pocket = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HitPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    Attack = table.Column<int>(type: "INTEGER", nullable: false),
                    Defense = table.Column<int>(type: "INTEGER", nullable: false),
                    SpecialAttack = table.Column<int>(type: "INTEGER", nullable: false),
                    SpecialDefense = table.Column<int>(type: "INTEGER", nullable: false),
                    Speed = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PokemonSpecies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    BaseStatsId = table.Column<int>(type: "INTEGER", nullable: false),
                    Type1 = table.Column<int>(type: "INTEGER", nullable: false),
                    Type2 = table.Column<int>(type: "INTEGER", nullable: false),
                    CatchRate = table.Column<int>(type: "INTEGER", nullable: false),
                    BaseExpYield = table.Column<int>(type: "INTEGER", nullable: false),
                    EffortValueYieldId = table.Column<int>(type: "INTEGER", nullable: true),
                    HeldItemId = table.Column<int>(type: "INTEGER", nullable: true),
                    RareHeldItemId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsGenderless = table.Column<bool>(type: "INTEGER", nullable: false),
                    GenderMaleChance = table.Column<float>(type: "REAL", nullable: false),
                    EggCycles = table.Column<int>(type: "INTEGER", nullable: false),
                    BaseFriendship = table.Column<int>(type: "INTEGER", nullable: false),
                    GrowthRate = table.Column<int>(type: "INTEGER", nullable: false),
                    EggGroup1 = table.Column<int>(type: "INTEGER", nullable: false),
                    EggGroup2 = table.Column<int>(type: "INTEGER", nullable: false),
                    Ability1Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Ability2Id = table.Column<int>(type: "INTEGER", nullable: true),
                    HiddenAbilityId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsBaby = table.Column<bool>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    Weight = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonSpecies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PokemonSpecies_Abilities_Ability1Id",
                        column: x => x.Ability1Id,
                        principalTable: "Abilities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PokemonSpecies_Abilities_Ability2Id",
                        column: x => x.Ability2Id,
                        principalTable: "Abilities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PokemonSpecies_Abilities_HiddenAbilityId",
                        column: x => x.HiddenAbilityId,
                        principalTable: "Abilities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PokemonSpecies_Items_HeldItemId",
                        column: x => x.HeldItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PokemonSpecies_Items_RareHeldItemId",
                        column: x => x.RareHeldItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PokemonSpecies_Stats_BaseStatsId",
                        column: x => x.BaseStatsId,
                        principalTable: "Stats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PokemonSpecies_Stats_EffortValueYieldId",
                        column: x => x.EffortValueYieldId,
                        principalTable: "Stats",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Moves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    DamageClass = table.Column<int>(type: "INTEGER", nullable: false),
                    Power = table.Column<int>(type: "INTEGER", nullable: false),
                    Accuracy = table.Column<int>(type: "INTEGER", nullable: false),
                    PowerPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    EffectChance = table.Column<float>(type: "REAL", nullable: false),
                    Effect = table.Column<int>(type: "INTEGER", nullable: false),
                    PokemonSpeciesId = table.Column<int>(type: "INTEGER", nullable: true),
                    PokemonSpeciesId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Moves_PokemonSpecies_PokemonSpeciesId",
                        column: x => x.PokemonSpeciesId,
                        principalTable: "PokemonSpecies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Moves_PokemonSpecies_PokemonSpeciesId1",
                        column: x => x.PokemonSpeciesId1,
                        principalTable: "PokemonSpecies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MoveLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MoveId = table.Column<int>(type: "INTEGER", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    PokemonSpeciesId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoveLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoveLevel_Moves_MoveId",
                        column: x => x.MoveId,
                        principalTable: "Moves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoveLevel_PokemonSpecies_PokemonSpeciesId",
                        column: x => x.PokemonSpeciesId,
                        principalTable: "PokemonSpecies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoveLevel_MoveId",
                table: "MoveLevel",
                column: "MoveId");

            migrationBuilder.CreateIndex(
                name: "IX_MoveLevel_PokemonSpeciesId",
                table: "MoveLevel",
                column: "PokemonSpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_PokemonSpeciesId",
                table: "Moves",
                column: "PokemonSpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_PokemonSpeciesId1",
                table: "Moves",
                column: "PokemonSpeciesId1");

            migrationBuilder.CreateIndex(
                name: "IX_PokemonSpecies_Ability1Id",
                table: "PokemonSpecies",
                column: "Ability1Id");

            migrationBuilder.CreateIndex(
                name: "IX_PokemonSpecies_Ability2Id",
                table: "PokemonSpecies",
                column: "Ability2Id");

            migrationBuilder.CreateIndex(
                name: "IX_PokemonSpecies_BaseStatsId",
                table: "PokemonSpecies",
                column: "BaseStatsId");

            migrationBuilder.CreateIndex(
                name: "IX_PokemonSpecies_EffortValueYieldId",
                table: "PokemonSpecies",
                column: "EffortValueYieldId");

            migrationBuilder.CreateIndex(
                name: "IX_PokemonSpecies_HeldItemId",
                table: "PokemonSpecies",
                column: "HeldItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PokemonSpecies_HiddenAbilityId",
                table: "PokemonSpecies",
                column: "HiddenAbilityId");

            migrationBuilder.CreateIndex(
                name: "IX_PokemonSpecies_RareHeldItemId",
                table: "PokemonSpecies",
                column: "RareHeldItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoveLevel");

            migrationBuilder.DropTable(
                name: "Moves");

            migrationBuilder.DropTable(
                name: "PokemonSpecies");

            migrationBuilder.DropTable(
                name: "Abilities");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Stats");
        }
    }
}
