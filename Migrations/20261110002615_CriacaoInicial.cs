using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrilhasApi.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TRILHA",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TITULO = table.Column<string>(type: "VARCHAR2(100)", unicode: false, maxLength: 100, nullable: false),
                    DESCRICAO = table.Column<string>(type: "VARCHAR2(500)", unicode: false, maxLength: 500, nullable: true),
                    TEMA = table.Column<string>(type: "VARCHAR2(20)", unicode: false, maxLength: 20, nullable: false),
                    NIVEL = table.Column<int>(type: "NUMBER(1)", precision: 1, nullable: false),
                    DT_CRIACAO = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRILHA", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DESAFIO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TRILHA_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PERGUNTA = table.Column<string>(type: "VARCHAR2(500)", unicode: false, maxLength: 500, nullable: false),
                    ALTERNATIVA_A = table.Column<string>(type: "VARCHAR2(200)", unicode: false, maxLength: 200, nullable: false),
                    ALTERNATIVA_B = table.Column<string>(type: "VARCHAR2(200)", unicode: false, maxLength: 200, nullable: false),
                    ALTERNATIVA_C = table.Column<string>(type: "VARCHAR2(200)", unicode: false, maxLength: 200, nullable: false),
                    ALTERNATIVA_D = table.Column<string>(type: "VARCHAR2(200)", unicode: false, maxLength: 200, nullable: false),
                    RESPOSTA_CORRETA = table.Column<string>(type: "CHAR(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    PONTOS = table.Column<int>(type: "NUMBER(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DESAFIO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DESAFIO_TRILHA",
                        column: x => x.TRILHA_ID,
                        principalTable: "TRILHA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DESAFIO_TRILHA_ID",
                table: "DESAFIO",
                column: "TRILHA_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DESAFIO");

            migrationBuilder.DropTable(
                name: "TRILHA");
        }
    }
}
