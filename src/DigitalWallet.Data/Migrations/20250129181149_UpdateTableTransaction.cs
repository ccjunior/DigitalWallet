using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalWallet.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_WalletId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "FromWalletId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ToWalletId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Transactions",
                newName: "TransactionType");

            migrationBuilder.AlterColumn<Guid>(
                name: "WalletId",
                table: "Transactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Transactions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_WalletId",
                table: "Transactions",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_WalletId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "TransactionType",
                table: "Transactions",
                newName: "Status");

            migrationBuilder.AlterColumn<Guid>(
                name: "WalletId",
                table: "Transactions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "FromWalletId",
                table: "Transactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ToWalletId",
                table: "Transactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_WalletId",
                table: "Transactions",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id");
        }
    }
}
