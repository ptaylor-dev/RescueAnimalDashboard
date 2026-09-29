using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RescueAnimalDashboard.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "animal_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_animal_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "outcome_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outcome_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sexes_upon_outcome",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sexes_upon_outcome", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "animals",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    animal_id = table.Column<string>(type: "text", nullable: false),
                    animal_type_id = table.Column<int>(type: "integer", nullable: false),
                    sex_upon_outcome_id = table.Column<int>(type: "integer", nullable: true),
                    outcome_type_id = table.Column<int>(type: "integer", nullable: true),
                    breed = table.Column<string>(type: "text", nullable: true),
                    color = table.Column<string>(type: "text", nullable: true),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    date_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    outcome_subtype = table.Column<string>(type: "text", nullable: true),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    age_upon_outcome = table.Column<string>(type: "text", nullable: true),
                    age_upon_outcome_in_weeks = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_animals", x => x.id);
                    table.ForeignKey(
                        name: "fk_animals_animal_types_animal_type_id",
                        column: x => x.animal_type_id,
                        principalTable: "animal_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_animals_outcome_types_outcome_type_id",
                        column: x => x.outcome_type_id,
                        principalTable: "outcome_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_animals_sex_upon_outcomes_sex_upon_outcome_id",
                        column: x => x.sex_upon_outcome_id,
                        principalTable: "sexes_upon_outcome",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_animal_types_name",
                table: "animal_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_animals_animal_id",
                table: "animals",
                column: "animal_id");

            migrationBuilder.CreateIndex(
                name: "ix_animals_animal_type_id",
                table: "animals",
                column: "animal_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_animals_breed",
                table: "animals",
                column: "breed");

            migrationBuilder.CreateIndex(
                name: "ix_animals_outcome_type_id",
                table: "animals",
                column: "outcome_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_animals_sex_upon_outcome_id",
                table: "animals",
                column: "sex_upon_outcome_id");

            migrationBuilder.CreateIndex(
                name: "ix_outcome_types_name",
                table: "outcome_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sexes_upon_outcome_name",
                table: "sexes_upon_outcome",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "animals");

            migrationBuilder.DropTable(
                name: "animal_types");

            migrationBuilder.DropTable(
                name: "outcome_types");

            migrationBuilder.DropTable(
                name: "sexes_upon_outcome");
        }
    }
}
