using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Faturamento.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoProdutoIdNoItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProdutoId",
                table: "InvoiceItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProdutoId",
                table: "InvoiceItems");
        }
    }
}
