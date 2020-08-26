using Microsoft.EntityFrameworkCore.Migrations;

namespace CancelamentoIdentity.Data.Migrations
{
    public partial class reativacaoVooCancelado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Reativado",
                table: "Cancelamentos",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reativado",
                table: "Cancelamentos");
        }
    }
}
