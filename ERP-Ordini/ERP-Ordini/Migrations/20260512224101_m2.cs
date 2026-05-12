using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_Ordini.Migrations
{
    /// <inheritdoc />
    public partial class m2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdOrdine",
                table: "DettagliOrdine");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdOrdine",
                table: "DettagliOrdine",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
