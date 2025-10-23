using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageManager.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_TagValues_ValueId",
                table: "Tags");

            migrationBuilder.DropTable(
                name: "TagValues");

            migrationBuilder.DropIndex(
                name: "IX_Tags_ImageId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_ValueId",
                table: "Tags");

            migrationBuilder.RenameColumn(
                name: "ValueId",
                table: "Tags",
                newName: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ImageId_Name",
                table: "Tags",
                columns: new[] { "ImageId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tags_ImageId_Name",
                table: "Tags");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Tags",
                newName: "ValueId");

            migrationBuilder.CreateTable(
                name: "TagValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagValues", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ImageId",
                table: "Tags",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ValueId",
                table: "Tags",
                column: "ValueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_TagValues_ValueId",
                table: "Tags",
                column: "ValueId",
                principalTable: "TagValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
