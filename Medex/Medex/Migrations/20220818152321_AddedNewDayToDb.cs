using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Medex.Migrations
{
    public partial class AddedNewDayToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "WorkHours");

            migrationBuilder.AddColumn<int>(
                name: "WeekDays",
                table: "WorkHours",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeekDays",
                table: "WorkHours");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Day",
                table: "WorkHours",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
