using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WebHelsi.Migrations
{
    public partial class AddDoctorandCleintinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblCities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblSpecialization",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSpecialization", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblClinic",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Street = table.Column<string>(maxLength: 250, nullable: false),
                    CityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblClinic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblClinic_tblCities_CityId",
                        column: x => x.CityId,
                        principalTable: "tblCities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblDoctors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Surname = table.Column<string>(nullable: true),
                    Spetialization = table.Column<string>(nullable: true),
                    DateBirthday = table.Column<DateTime>(nullable: false),
                    ImageDoctor = table.Column<string>(nullable: true),
                    ClinicId = table.Column<int>(nullable: false),
                    SpecializationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDoctors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblDoctors_tblClinic_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "tblClinic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblDoctors_tblSpecialization_SpecializationId",
                        column: x => x.SpecializationId,
                        principalTable: "tblSpecialization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblClients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Surname = table.Column<string>(nullable: true),
                    Spetialization = table.Column<string>(nullable: true),
                    DateBirthday = table.Column<DateTime>(nullable: false),
                    DoctorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblClients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblClients_tblDoctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "tblDoctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblClients_DoctorId",
                table: "tblClients",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_tblClinic_CityId",
                table: "tblClinic",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_tblDoctors_ClinicId",
                table: "tblDoctors",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_tblDoctors_SpecializationId",
                table: "tblDoctors",
                column: "SpecializationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblClients");

            migrationBuilder.DropTable(
                name: "tblDoctors");

            migrationBuilder.DropTable(
                name: "tblClinic");

            migrationBuilder.DropTable(
                name: "tblSpecialization");

            migrationBuilder.DropTable(
                name: "tblCities");
        }
    }
}
