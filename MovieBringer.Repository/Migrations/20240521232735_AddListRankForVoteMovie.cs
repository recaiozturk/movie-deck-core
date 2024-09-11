using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieBringer.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddListRankForVoteMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ListRank",
                table: "VoteMovieLists",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListRank",
                table: "VoteMovieLists");
        }
    }
}
