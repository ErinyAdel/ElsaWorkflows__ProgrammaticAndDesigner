using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElsaServer.Migrations
{
    /// <inheritdoc />
    public partial class AddReqIdInDocumentTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestId",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "Documents");
        }
    }
}
