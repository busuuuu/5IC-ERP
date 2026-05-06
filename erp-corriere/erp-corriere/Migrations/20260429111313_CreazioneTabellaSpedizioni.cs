using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace erp_corriere.Migrations
{
    /// <inheritdoc />
    public partial class CreazioneTabellaSpedizioni : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Spedizioni",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Destinatario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Indirizzo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Citta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Corriere = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stato = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataSpedizione = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spedizioni", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Spedizioni");
        }
    }
}
