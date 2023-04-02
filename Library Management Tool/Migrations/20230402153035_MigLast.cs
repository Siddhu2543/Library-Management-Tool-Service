using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_Management_Tool.Migrations
{
    /// <inheritdoc />
    public partial class MigLast : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Books_BookId",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Members_MemberId",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_BookId",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_MemberId",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Books_PublisherId",
                table: "Books");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Issues_BookId",
                table: "Issues",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_MemberId",
                table: "Issues",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_PublisherId",
                table: "Books",
                column: "PublisherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Books_BookId",
                table: "Issues",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Members_MemberId",
                table: "Issues",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
