using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestePoo.Migrations
{
    /// <inheritdoc />
    public partial class ModifyEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "tb_tarefa",
                newName: "Status");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "tb_tarefa",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "enum('Pendente','EmAndamento','Atrasada','Concluida')")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "tb_tarefa",
                newName: "status");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "tb_tarefa",
                type: "enum('Pendente','EmAndamento','Atrasada','Concluida')",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
