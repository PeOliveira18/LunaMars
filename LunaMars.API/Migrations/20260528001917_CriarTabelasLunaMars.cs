using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LunaMars.API.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelasLunaMars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Colonias",
                columns: table => new
                {
                    idColonia = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nomeColonia = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    localizacao = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    dtCriacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    capacidadePessoas = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colonias", x => x.idColonia);
                });

            migrationBuilder.CreateTable(
                name: "Rovers",
                columns: table => new
                {
                    idEquipamento = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nivelBateria = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    distanciaPercorridaKm = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    nomeEquipamento = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    dtAtivacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ativo = table.Column<int>(type: "NUMBER(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rovers", x => x.idEquipamento);
                });

            migrationBuilder.CreateTable(
                name: "Satelites",
                columns: table => new
                {
                    idEquipamento = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    orbita = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    agenciaOperadora = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    nomeEquipamento = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    dtAtivacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ativo = table.Column<int>(type: "NUMBER(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Satelites", x => x.idEquipamento);
                });

            migrationBuilder.CreateTable(
                name: "Recursos",
                columns: table => new
                {
                    idRecurso = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    tipoRecurso = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    quantidade = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    unidadeMedida = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    nivelMinimo = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    dtValidade = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    coloniaEspacialId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recursos", x => x.idRecurso);
                    table.ForeignKey(
                        name: "FK_Recursos_Colonias_coloniaEspacialId",
                        column: x => x.coloniaEspacialId,
                        principalTable: "Colonias",
                        principalColumn: "idColonia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Setores",
                columns: table => new
                {
                    idSetor = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nomeSetor = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    tipoSetor = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    pressaoInterna = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    temperaturaAtual = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    ativo = table.Column<int>(type: "NUMBER(1)", nullable: false),
                    coloniaEspacialId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setores", x => x.idSetor);
                    table.ForeignKey(
                        name: "FK_Setores_Colonias_coloniaEspacialId",
                        column: x => x.coloniaEspacialId,
                        principalTable: "Colonias",
                        principalColumn: "idColonia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alertas",
                columns: table => new
                {
                    idAlerta = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    titulo = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    mensagem = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    nivelRisco = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    statusAlerta = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    dtCriacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    dtFinalizacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    setorColoniaId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alertas", x => x.idAlerta);
                    table.ForeignKey(
                        name: "FK_Alertas_Setores_setorColoniaId",
                        column: x => x.setorColoniaId,
                        principalTable: "Setores",
                        principalColumn: "idSetor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sensores",
                columns: table => new
                {
                    idSensor = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nomeSensor = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    tipoSensor = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ativo = table.Column<int>(type: "NUMBER(1)", nullable: false),
                    setorColoniaId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensores", x => x.idSensor);
                    table.ForeignKey(
                        name: "FK_Sensores_Setores_setorColoniaId",
                        column: x => x.setorColoniaId,
                        principalTable: "Setores",
                        principalColumn: "idSetor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Missoes",
                columns: table => new
                {
                    idMissao = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    descricao = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    statusMissao = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    dtInicio = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    dtFim = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    alertaColoniaId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Missoes", x => x.idMissao);
                    table.ForeignKey(
                        name: "FK_Missoes_Alertas_alertaColoniaId",
                        column: x => x.alertaColoniaId,
                        principalTable: "Alertas",
                        principalColumn: "idAlerta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Leituras",
                columns: table => new
                {
                    idLeitura = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    tipoSensor = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    valor = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    unidadeMedida = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    dtLeitura = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    sensorAmbientalId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    setorColoniaId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leituras", x => x.idLeitura);
                    table.ForeignKey(
                        name: "FK_Leituras_Sensores_sensorAmbientalId",
                        column: x => x.sensorAmbientalId,
                        principalTable: "Sensores",
                        principalColumn: "idSensor",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Leituras_Setores_setorColoniaId",
                        column: x => x.setorColoniaId,
                        principalTable: "Setores",
                        principalColumn: "idSetor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_setorColoniaId",
                table: "Alertas",
                column: "setorColoniaId");

            migrationBuilder.CreateIndex(
                name: "IX_Leituras_sensorAmbientalId",
                table: "Leituras",
                column: "sensorAmbientalId");

            migrationBuilder.CreateIndex(
                name: "IX_Leituras_setorColoniaId",
                table: "Leituras",
                column: "setorColoniaId");

            migrationBuilder.CreateIndex(
                name: "IX_Missoes_alertaColoniaId",
                table: "Missoes",
                column: "alertaColoniaId");

            migrationBuilder.CreateIndex(
                name: "IX_Recursos_coloniaEspacialId",
                table: "Recursos",
                column: "coloniaEspacialId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensores_setorColoniaId",
                table: "Sensores",
                column: "setorColoniaId");

            migrationBuilder.CreateIndex(
                name: "IX_Setores_coloniaEspacialId",
                table: "Setores",
                column: "coloniaEspacialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Leituras");

            migrationBuilder.DropTable(
                name: "Missoes");

            migrationBuilder.DropTable(
                name: "Recursos");

            migrationBuilder.DropTable(
                name: "Rovers");

            migrationBuilder.DropTable(
                name: "Satelites");

            migrationBuilder.DropTable(
                name: "Sensores");

            migrationBuilder.DropTable(
                name: "Alertas");

            migrationBuilder.DropTable(
                name: "Setores");

            migrationBuilder.DropTable(
                name: "Colonias");
        }
    }
}
