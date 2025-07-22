using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Paynext.Infra.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    UUID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    FullName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Document = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: false),
                    UserRole = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ClientId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ForeignKey = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.UUID);
                });

            migrationBuilder.CreateTable(
                name: "CONTRACTS",
                columns: table => new
                {
                    UUID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ContractNumber = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UserUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    InitialAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    RemainingValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsFinished = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTRACTS", x => x.UUID);
                    table.ForeignKey(
                        name: "FK_CONTRACTS_USERS_UserUuid",
                        column: x => x.UserUuid,
                        principalTable: "USERS",
                        principalColumn: "UUID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "INSTALLMENTS",
                columns: table => new
                {
                    UUID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    InstallmentId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ContractUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    IsAntecipated = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ActionedByUserUuiD = table.Column<Guid>(type: "uuid", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INSTALLMENTS", x => x.UUID);
                    table.ForeignKey(
                        name: "FK_INSTALLMENTS_CONTRACTS_ContractUuid",
                        column: x => x.ContractUuid,
                        principalTable: "CONTRACTS",
                        principalColumn: "UUID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_INSTALLMENTS_USERS_ActionedByUserUuiD",
                        column: x => x.ActionedByUserUuiD,
                        principalTable: "USERS",
                        principalColumn: "UUID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CONTRACTS_Id",
                table: "CONTRACTS",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CONTRACTS_UserUuid",
                table: "CONTRACTS",
                column: "UserUuid");

            migrationBuilder.CreateIndex(
                name: "IX_INSTALLMENTS_ActionedByUserUuiD",
                table: "INSTALLMENTS",
                column: "ActionedByUserUuiD");

            migrationBuilder.CreateIndex(
                name: "IX_INSTALLMENTS_ContractUuid",
                table: "INSTALLMENTS",
                column: "ContractUuid");

            migrationBuilder.CreateIndex(
                name: "IX_INSTALLMENTS_Id",
                table: "INSTALLMENTS",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USERS_Document",
                table: "USERS",
                column: "Document",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USERS_Email",
                table: "USERS",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USERS_Id",
                table: "USERS",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USERS_Phone",
                table: "USERS",
                column: "Phone",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "INSTALLMENTS");

            migrationBuilder.DropTable(
                name: "CONTRACTS");

            migrationBuilder.DropTable(
                name: "USERS");
        }
    }
}
