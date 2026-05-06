using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace erp_corriere.Migrations
{
    /// <inheritdoc />
    public partial class AggiuntaCampiCostoECollo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cap",
                table: "Spedizioni",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dimensioni",
                table: "Spedizioni",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PesoKg",
                table: "Spedizioni",
                type: "decimal(8,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CostoNetto",
                table: "Spedizioni",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AliquotaIva",
                table: "Spedizioni",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Spedizioni",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Cap", table: "Spedizioni");
            migrationBuilder.DropColumn(name: "Dimensioni", table: "Spedizioni");
            migrationBuilder.DropColumn(name: "PesoKg", table: "Spedizioni");
            migrationBuilder.DropColumn(name: "CostoNetto", table: "Spedizioni");
            migrationBuilder.DropColumn(name: "AliquotaIva", table: "Spedizioni");
            migrationBuilder.DropColumn(name: "Note", table: "Spedizioni");
        }
    }
}
