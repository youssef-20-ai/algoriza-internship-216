using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlgorizaProject.DAL.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appoitments_Doctors_DoctorId",
                table: "Appoitments");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "Appoitments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appoitments_Doctors_DoctorId",
                table: "Appoitments",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appoitments_Doctors_DoctorId",
                table: "Appoitments");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "Appoitments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Appoitments_Doctors_DoctorId",
                table: "Appoitments",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id");
        }
    }
}
