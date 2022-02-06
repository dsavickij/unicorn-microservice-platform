using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicorn.Core.Services.ServiceDiscovery.DataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceHosts",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceHosts", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "GrpcServiceConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BaseUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false),
                    ServiceHostName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrpcServiceConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrpcServiceConfigurations_ServiceHosts_ServiceHostName",
                        column: x => x.ServiceHostName,
                        principalTable: "ServiceHosts",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HttpServiceConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BaseUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceHostName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HttpServiceConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HttpServiceConfigurations_ServiceHosts_ServiceHostName",
                        column: x => x.ServiceHostName,
                        principalTable: "ServiceHosts",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrpcServiceConfigurations_ServiceHostName",
                table: "GrpcServiceConfigurations",
                column: "ServiceHostName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HttpServiceConfigurations_ServiceHostName",
                table: "HttpServiceConfigurations",
                column: "ServiceHostName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrpcServiceConfigurations");

            migrationBuilder.DropTable(
                name: "HttpServiceConfigurations");

            migrationBuilder.DropTable(
                name: "ServiceHosts");
        }
    }
}
