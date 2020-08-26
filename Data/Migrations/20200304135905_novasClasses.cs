using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CancelamentoIdentity.Data.Migrations
{
    public partial class novasClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tipos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CodAdm = table.Column<string>(nullable: true),
                    Tipo = table.Column<string>(nullable: true),
                    TipoSistemaCancelamento = table.Column<string>(nullable: true),
                    Abonavel = table.Column<bool>(nullable: false),
                    Descricao = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Voos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DataVoo = table.Column<DateTime>(nullable: false),
                    NumeroDoVoo = table.Column<string>(nullable: false),
                    STA = table.Column<DateTime>(nullable: false),
                    STD = table.Column<DateTime>(nullable: false),
                    Origem = table.Column<string>(nullable: true),
                    Destino = table.Column<string>(nullable: true),
                    QtdPassageiros = table.Column<int>(nullable: false),
                    MetarOrigem = table.Column<string>(nullable: true),
                    MetarDestino = table.Column<string>(nullable: true),
                    Matricula = table.Column<string>(maxLength: 10, nullable: true),
                    TipoVoo = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cancelamentos",
                columns: table => new
                {
                    CriadoPor = table.Column<string>(maxLength: 256, nullable: true),
                    AtualizadoPor = table.Column<string>(maxLength: 256, nullable: true),
                    DataAtualizacao = table.Column<DateTime>(nullable: false),
                    DataCriacao = table.Column<DateTime>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TipoCancelamentoId = table.Column<int>(nullable: false),
                    VooCanceladoId = table.Column<int>(nullable: false),
                    VooAnteriorId = table.Column<int>(nullable: true),
                    Observacao = table.Column<string>(nullable: true),
                    Cancelado = table.Column<bool>(nullable: false),
                    AlvoDeProcesso = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cancelamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cancelamentos_Tipos_TipoCancelamentoId",
                        column: x => x.TipoCancelamentoId,
                        principalTable: "Tipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cancelamentos_Voos_VooAnteriorId",
                        column: x => x.VooAnteriorId,
                        principalTable: "Voos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cancelamentos_Voos_VooCanceladoId",
                        column: x => x.VooCanceladoId,
                        principalTable: "Voos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Anexos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NomeOriginal = table.Column<string>(nullable: true),
                    NomeCriado = table.Column<string>(nullable: true),
                    CancelamentoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anexos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Anexos_Cancelamentos_CancelamentoId",
                        column: x => x.CancelamentoId,
                        principalTable: "Cancelamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Anexos_CancelamentoId",
                table: "Anexos",
                column: "CancelamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cancelamentos_TipoCancelamentoId",
                table: "Cancelamentos",
                column: "TipoCancelamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cancelamentos_VooAnteriorId",
                table: "Cancelamentos",
                column: "VooAnteriorId");

            migrationBuilder.CreateIndex(
                name: "IX_Cancelamentos_VooCanceladoId",
                table: "Cancelamentos",
                column: "VooCanceladoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Anexos");

            migrationBuilder.DropTable(
                name: "Cancelamentos");

            migrationBuilder.DropTable(
                name: "Tipos");

            migrationBuilder.DropTable(
                name: "Voos");
        }
    }
}
