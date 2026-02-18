using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToPropertyAd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PropertyAds",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyAds_UserId",
                table: "PropertyAds",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyAds_AspNetUsers_UserId",
                table: "PropertyAds",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyAds_AspNetUsers_UserId",
                table: "PropertyAds");

            migrationBuilder.DropIndex(
                name: "IX_PropertyAds_UserId",
                table: "PropertyAds");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PropertyAds");
        }
    }
}
