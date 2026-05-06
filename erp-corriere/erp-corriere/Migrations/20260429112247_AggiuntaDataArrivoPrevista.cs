using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace erp_corriere.Migrations
{
    /// <inheritdoc />
    public partial class AggiuntaDataArrivoPrevista : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataArrivoPrevista",
                table: "Spedizioni",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataArrivoPrevista",
                table: "Spedizioni");
        }
    }
}
