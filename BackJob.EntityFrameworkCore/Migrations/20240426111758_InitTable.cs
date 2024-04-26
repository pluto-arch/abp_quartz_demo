using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackJob.EntityFrameworkCore.Migrations
{
    public partial class InitTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            for (int i = 0; i < 10; i++)
            {
                migrationBuilder.CreateTable(
                    name: $"Product{i}",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                        Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey($"PK_Product{i}", x => x.Id);
                    });
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            for (int i = 0; i < 10; i++)
            {
                migrationBuilder.DropTable(
                    name: $"Product{i}");
            }
        }
    }
}
