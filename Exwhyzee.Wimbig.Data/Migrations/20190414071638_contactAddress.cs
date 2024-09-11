using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Exwhyzee.Wimbig.Data.Migrations
{
    public partial class contactAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AddColumn<string>(
                name: "ContactAddress",
                table: "AspNetUsers",
                nullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropColumn(
                name: "ContactAddress",
                table: "AspNetUsers");

        }
    }
}
