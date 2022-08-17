using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceMornitoringTool.API.Relational.Migrations
{
    public partial class ChangedTheMethodStatusType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ExecutionsStatus",
                table: "ServiceMethod",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ExecutionsStatus",
                table: "ServiceMethod",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
