using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RenameField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsBorrowed",
                table: "Books",
                newName: "IsAvailable");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Books",
                newName: "IsBorrowed");
        }
    }
}
