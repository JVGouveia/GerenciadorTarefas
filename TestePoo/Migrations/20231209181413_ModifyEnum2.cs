using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestePoo.Migrations
{
    /// <inheritdoc />
    public partial class ModifyEnum2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "tb_tarefa",
                newName: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "tb_tarefa",
                newName: "Status");
        }
    }
}
