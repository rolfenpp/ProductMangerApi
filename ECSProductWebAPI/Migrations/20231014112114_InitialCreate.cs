using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECSProductWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Namn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Beskrivning = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Sku = table.Column<string>(type: "nchar(6)", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Pris = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_Sku",
                table: "Product",
                column: "Sku",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
