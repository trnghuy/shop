using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Migrations
{
    /// <inheritdoc />
    public partial class rename_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Statistics",
                table: "Statistics");

            migrationBuilder.RenameTable(
                name: "Statistics",
                newName: "StatisticaModels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StatisticaModels",
                table: "StatisticaModels",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StatisticaModels",
                table: "StatisticaModels");

            migrationBuilder.RenameTable(
                name: "StatisticaModels",
                newName: "Statistics");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Statistics",
                table: "Statistics",
                column: "Id");
        }
    }
}
