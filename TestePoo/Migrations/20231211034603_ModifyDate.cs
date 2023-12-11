using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestePoo.Migrations
{
    /// <inheritdoc />
    public partial class ModifyDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "data_final",
                table: "tb_tarefa",
                newName: "data_limite");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "data_limite",
                table: "tb_tarefa",
                newName: "data_final");
        }
    }
}
