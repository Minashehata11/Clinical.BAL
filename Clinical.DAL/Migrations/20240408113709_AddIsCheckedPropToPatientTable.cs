using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinical.DAL.Migrations
{
    public partial class AddIsCheckedPropToPatientTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsChecked",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChecked",
                table: "Patients");
        }
    }
}
