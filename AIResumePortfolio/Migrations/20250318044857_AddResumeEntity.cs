using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIResumePortfolio.Migrations
{
    /// <inheritdoc />
    public partial class AddResumeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParsedTextContent",
                table: "Resumes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParsedTextContent",
                table: "Resumes");
        }
    }
}
