using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProjectAPI_Travel.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TbBus",
                columns: table => new
                {
                    id_bus = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    placa = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    num_columns = table.Column<int>(type: "int", nullable: false),
                    num_rows = table.Column<int>(type: "int", nullable: false),
                    availability = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    state = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbBus", x => x.id_bus);
                });

            migrationBuilder.CreateTable(
                name: "TbStations",
                columns: table => new
                {
                    id_stn = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    city = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    street = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    pseudonym = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    state = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbStations", x => x.id_stn);
                });

            migrationBuilder.CreateTable(
                name: "TbUsers",
                columns: table => new
                {
                    id_usr = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    lastname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    birtdate = table.Column<DateOnly>(type: "date", nullable: false),
                    type_document = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    num_document = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    mail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    state = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ban = table.Column<bool>(type: "bit", nullable: false),
                    registration_date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbUsers", x => x.id_usr);
                });

            migrationBuilder.CreateTable(
                name: "TbSeating",
                columns: table => new
                {
                    id_bus = table.Column<int>(type: "int", nullable: false),
                    row = table.Column<int>(type: "int", nullable: false),
                    column = table.Column<int>(type: "int", nullable: false),
                    tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    busy = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    state = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbSeating", x => new { x.id_bus, x.column, x.row });
                    table.ForeignKey(
                        name: "FK_tb_seating_bus",
                        column: x => x.id_bus,
                        principalTable: "TbBus",
                        principalColumn: "id_bus",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbWorkers",
                columns: table => new
                {
                    id_wrk = table.Column<int>(type: "int", nullable: false),
                    registration_date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    salary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    availability = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    state = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbWorkers", x => x.id_wrk);
                    table.ForeignKey(
                        name: "FK_TbWorkers_TbUsers_id_wrk",
                        column: x => x.id_wrk,
                        principalTable: "TbUsers",
                        principalColumn: "id_usr");
                });

            migrationBuilder.CreateTable(
                name: "TbItineraries",
                columns: table => new
                {
                    id_itn = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_origin = table.Column<int>(type: "int", nullable: false),
                    id_destination = table.Column<int>(type: "int", nullable: false),
                    id_wrk = table.Column<int>(type: "int", nullable: false),
                    id_bus = table.Column<int>(type: "int", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    arrival_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    availability = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    state = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbItineraries", x => x.id_itn);
                    table.ForeignKey(
                        name: "FK_tb_itinerary_bus",
                        column: x => x.id_bus,
                        principalTable: "TbBus",
                        principalColumn: "id_bus");
                    table.ForeignKey(
                        name: "FK_tb_itinerary_destination",
                        column: x => x.id_destination,
                        principalTable: "TbStations",
                        principalColumn: "id_stn");
                    table.ForeignKey(
                        name: "FK_tb_itinerary_origin",
                        column: x => x.id_origin,
                        principalTable: "TbStations",
                        principalColumn: "id_stn");
                    table.ForeignKey(
                        name: "FK_tb_itinerary_worker",
                        column: x => x.id_wrk,
                        principalTable: "TbWorkers",
                        principalColumn: "id_wrk");
                });

            migrationBuilder.CreateTable(
                name: "TbTickets",
                columns: table => new
                {
                    id_tck = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usr = table.Column<int>(type: "int", nullable: false),
                    id_wrk = table.Column<int>(type: "int", nullable: false),
                    id_itn = table.Column<int>(type: "int", nullable: false),
                    id_bus = table.Column<int>(type: "int", nullable: false),
                    row = table.Column<int>(type: "int", nullable: false),
                    column = table.Column<int>(type: "int", nullable: false),
                    payment_method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    age = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type_document = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    num_document = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    state = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    problem = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    problem_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    creation_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbTickets", x => x.id_tck);
                    table.ForeignKey(
                        name: "FK_TbTickets_TbBus_id_bus",
                        column: x => x.id_bus,
                        principalTable: "TbBus",
                        principalColumn: "id_bus",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbTickets_TbItineraries_id_itn",
                        column: x => x.id_itn,
                        principalTable: "TbItineraries",
                        principalColumn: "id_itn",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbTickets_TbWorkers_id_wrk",
                        column: x => x.id_wrk,
                        principalTable: "TbWorkers",
                        principalColumn: "id_wrk",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_ticket_user",
                        column: x => x.id_usr,
                        principalTable: "TbUsers",
                        principalColumn: "id_usr");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbItineraries_id_bus",
                table: "TbItineraries",
                column: "id_bus");

            migrationBuilder.CreateIndex(
                name: "IX_TbItineraries_id_destination",
                table: "TbItineraries",
                column: "id_destination");

            migrationBuilder.CreateIndex(
                name: "IX_TbItineraries_id_origin",
                table: "TbItineraries",
                column: "id_origin");

            migrationBuilder.CreateIndex(
                name: "IX_TbItineraries_id_wrk",
                table: "TbItineraries",
                column: "id_wrk");

            migrationBuilder.CreateIndex(
                name: "IX_TbTickets_id_bus",
                table: "TbTickets",
                column: "id_bus");

            migrationBuilder.CreateIndex(
                name: "IX_TbTickets_id_itn",
                table: "TbTickets",
                column: "id_itn");

            migrationBuilder.CreateIndex(
                name: "IX_TbTickets_id_usr",
                table: "TbTickets",
                column: "id_usr");

            migrationBuilder.CreateIndex(
                name: "IX_TbTickets_id_wrk",
                table: "TbTickets",
                column: "id_wrk");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbSeating");

            migrationBuilder.DropTable(
                name: "TbTickets");

            migrationBuilder.DropTable(
                name: "TbItineraries");

            migrationBuilder.DropTable(
                name: "TbBus");

            migrationBuilder.DropTable(
                name: "TbStations");

            migrationBuilder.DropTable(
                name: "TbWorkers");

            migrationBuilder.DropTable(
                name: "TbUsers");
        }
    }
}
