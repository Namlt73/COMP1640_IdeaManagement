using Microsoft.EntityFrameworkCore.Migrations;

namespace COMP1640_IdeaManagement.Data.Migrations
{
    public partial class AddDbIdeaImgs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Docs",
                table: "Ideas");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Ideas");

            migrationBuilder.CreateTable(
                name: "IdeaImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdeaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeaImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdeaImages_Ideas_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "Ideas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdeaImages_IdeaId",
                table: "IdeaImages",
                column: "IdeaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdeaImages");

            migrationBuilder.AddColumn<string>(
                name: "Docs",
                table: "Ideas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Ideas",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
