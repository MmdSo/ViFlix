using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViFlix.Data.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "DurationDays",
                table: "SubscriptionPlan",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "ReleaseDate",
                table: "Series",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Cast",
                table: "Series",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cuntry",
                table: "Series",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Director",
                table: "Series",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GanreId",
                table: "Series",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDubed",
                table: "Series",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LanguageId",
                table: "Series",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SeasonsId",
                table: "Series",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EpisodeUrl",
                table: "Seasons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "SeasonId",
                table: "Seasons",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "ReleaseDate",
                table: "Movie",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Cast",
                table: "Movie",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cuntry",
                table: "Movie",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Director",
                table: "Movie",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDubed",
                table: "Movie",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cast",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "Cuntry",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "Director",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "GanreId",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "IsDubed",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "SeasonsId",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "EpisodeUrl",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "SeasonId",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "Cast",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "Cuntry",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "Director",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "IsDubed",
                table: "Movie");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DurationDays",
                table: "SubscriptionPlan",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                table: "Series",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                table: "Movie",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
