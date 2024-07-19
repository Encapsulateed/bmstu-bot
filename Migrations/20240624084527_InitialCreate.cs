using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace bmstu_bot.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    chatId = table.Column<long>(type: "bigint", nullable: false),
                    link = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Admins_pkey", x => x.chatId);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    chatId = table.Column<long>(type: "bigint", nullable: false),
                    bmstu_group = table.Column<string>(name: "bmstu_group ", type: "character varying(255)", maxLength: 255, nullable: true),
                    anonim = table.Column<bool>(type: "boolean", nullable: true),
                    tgLink = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ComandLine = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Fio = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    complain_type = table.Column<int>(type: "integer", nullable: false),
                    complain_category = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.chatId);
                });

            migrationBuilder.CreateTable(
                name: "Complains",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    from = table.Column<long>(name: "from ", type: "bigint", nullable: false),
                    message = table.Column<string>(type: "text", nullable: true),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    admin = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    prev = table.Column<int>(type: "integer", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false),
                    answer = table.Column<string>(type: "text", nullable: true),
                    isAnon = table.Column<bool>(type: "boolean", nullable: false),
                    category = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Complains_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Complains_from _fkey",
                        column: x => x.from,
                        principalTable: "users",
                        principalColumn: "chatId");
                });

            migrationBuilder.CreateTable(
                name: "Entries",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    adminChat = table.Column<long>(type: "bigint", nullable: false),
                    complainId = table.Column<int>(type: "integer", nullable: false),
                    messageId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Entries_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Entries_complainId_fkey",
                        column: x => x.complainId,
                        principalTable: "Complains",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Complains_from ",
                table: "Complains",
                column: "from ");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_complainId",
                table: "Entries",
                column: "complainId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Entries");

            migrationBuilder.DropTable(
                name: "Complains");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
