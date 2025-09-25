using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaCitas.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioIdToMedico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Medicos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Medicos");
        }
    }
}
