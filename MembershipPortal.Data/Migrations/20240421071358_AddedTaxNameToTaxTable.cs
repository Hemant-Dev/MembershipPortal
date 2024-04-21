using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MembershipPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedTaxNameToTaxTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TaxName",
                table: "Taxes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxName",
                table: "Taxes");
        }
    }
}
