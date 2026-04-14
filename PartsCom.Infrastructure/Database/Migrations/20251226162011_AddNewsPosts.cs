using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartsCom.Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class AddNewsPosts : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "news_posts",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                short_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                image_url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                published_at = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_news_posts", x => x.id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "news_posts");
    }
}
