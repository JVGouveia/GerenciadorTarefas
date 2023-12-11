using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestePoo.Migrations
{
    /// <inheritdoc />
    public partial class TesteOnDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_lista_tb_usuario_id_usuario",
                table: "tb_lista");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_tarefa_tb_lista_id_lista",
                table: "tb_tarefa");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "tb_lista",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_lista_tb_usuario_id_usuario",
                table: "tb_lista",
                column: "id_usuario",
                principalTable: "tb_usuario",
                principalColumn: "id_usuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tarefa_tb_lista_id_lista",
                table: "tb_tarefa",
                column: "id_lista",
                principalTable: "tb_lista",
                principalColumn: "id_lista",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_lista_tb_usuario_id_usuario",
                table: "tb_lista");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_tarefa_tb_lista_id_lista",
                table: "tb_tarefa");

            migrationBuilder.UpdateData(
                table: "tb_lista",
                keyColumn: "nome",
                keyValue: null,
                column: "nome",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "tb_lista",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_lista_tb_usuario_id_usuario",
                table: "tb_lista",
                column: "id_usuario",
                principalTable: "tb_usuario",
                principalColumn: "id_usuario",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tarefa_tb_lista_id_lista",
                table: "tb_tarefa",
                column: "id_lista",
                principalTable: "tb_lista",
                principalColumn: "id_lista",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
