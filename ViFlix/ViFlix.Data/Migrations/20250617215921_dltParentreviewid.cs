using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViFlix.Data.Migrations
{
    public partial class dltParentreviewid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Reviews_ParentReviewId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ParentReviewId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ParentReviewId",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ParentId",
                table: "Reviews",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Reviews_ParentId",
                table: "Reviews",
                column: "ParentId",
                principalTable: "Reviews",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Reviews_ParentId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ParentId",
                table: "Reviews");

            migrationBuilder.AddColumn<long>(
                name: "ParentReviewId",
                table: "Reviews",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ParentReviewId",
                table: "Reviews",
                column: "ParentReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Reviews_ParentReviewId",
                table: "Reviews",
                column: "ParentReviewId",
                principalTable: "Reviews",
                principalColumn: "Id");
        }
    }
}
