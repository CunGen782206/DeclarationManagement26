using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeclarationManagement.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTemporaryDeclarationAttachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeclarationAttachments_Declarations_DeclarationId",
                table: "DeclarationAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_DeclarationAttachments_Users_UploadedByUserId",
                table: "DeclarationAttachments");

            migrationBuilder.AlterColumn<long>(
                name: "DeclarationId",
                table: "DeclarationAttachments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "TempAttachmentKey",
                table: "DeclarationAttachments",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeclarationAttachments_TempAttachmentKey",
                table: "DeclarationAttachments",
                column: "TempAttachmentKey");

            migrationBuilder.AddForeignKey(
                name: "FK_DeclarationAttachments_Declarations_DeclarationId",
                table: "DeclarationAttachments",
                column: "DeclarationId",
                principalTable: "Declarations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeclarationAttachments_Users_UploadedByUserId",
                table: "DeclarationAttachments",
                column: "UploadedByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeclarationAttachments_Declarations_DeclarationId",
                table: "DeclarationAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_DeclarationAttachments_Users_UploadedByUserId",
                table: "DeclarationAttachments");

            migrationBuilder.DropIndex(
                name: "IX_DeclarationAttachments_TempAttachmentKey",
                table: "DeclarationAttachments");

            migrationBuilder.DropColumn(
                name: "TempAttachmentKey",
                table: "DeclarationAttachments");

            migrationBuilder.AlterColumn<long>(
                name: "DeclarationId",
                table: "DeclarationAttachments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeclarationAttachments_Declarations_DeclarationId",
                table: "DeclarationAttachments",
                column: "DeclarationId",
                principalTable: "Declarations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeclarationAttachments_Users_UploadedByUserId",
                table: "DeclarationAttachments",
                column: "UploadedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
