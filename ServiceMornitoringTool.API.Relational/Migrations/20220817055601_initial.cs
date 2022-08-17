﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceMornitoringTool.API.Relational.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceMonitorAggregate",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceMonitorAggregate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceMethod",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Request = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeElapsed = table.Column<TimeSpan>(type: "time", nullable: false),
                    ExecutionsStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceMonitorAggregateId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceMethod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceMethod_ServiceMonitorAggregate_ServiceMonitorAggregateId",
                        column: x => x.ServiceMonitorAggregateId,
                        principalTable: "ServiceMonitorAggregate",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceMethod_ServiceMonitorAggregateId",
                table: "ServiceMethod",
                column: "ServiceMonitorAggregateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceMethod");

            migrationBuilder.DropTable(
                name: "ServiceMonitorAggregate");
        }
    }
}
