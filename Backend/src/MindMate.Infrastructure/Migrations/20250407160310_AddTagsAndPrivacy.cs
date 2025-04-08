using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MindMate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTagsAndPrivacy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "JournalEntries",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "JournalEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "JournalEntries");
        }
    }
}
