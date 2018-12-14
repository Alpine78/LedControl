using Microsoft.EntityFrameworkCore.Migrations;

namespace HarjoitustyoLed.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LedSequences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedSequences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeRows",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Time = table.Column<int>(nullable: false),
                    LedSequenceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeRows_LedSequences_LedSequenceId",
                        column: x => x.LedSequenceId,
                        principalTable: "LedSequences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LedRows",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PinId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    TimeRowId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LedRows_TimeRows_TimeRowId",
                        column: x => x.TimeRowId,
                        principalTable: "TimeRows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LedRows_TimeRowId",
                table: "LedRows",
                column: "TimeRowId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeRows_LedSequenceId",
                table: "TimeRows",
                column: "LedSequenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LedRows");

            migrationBuilder.DropTable(
                name: "TimeRows");

            migrationBuilder.DropTable(
                name: "LedSequences");
        }
    }
}
