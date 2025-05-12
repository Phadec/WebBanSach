using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Week2.Migrations
{
    /// <inheritdoc />
    public partial class AddVietnameseBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add a new category "Self-Help" if it doesn't exist
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[] { 5, "Self-Help" });

            // Add the Vietnamese books
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "CategoryId", "Cover", "Price", "PublishYear", "Title" },
                values: new object[] { 12, "CLINT EMERSON", 2, "images/vietnamese1.jpg", 99000, 2022, "100 Kỹ Năng Sinh Tồn" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "CategoryId", "Cover", "Price", "PublishYear", "Title" },
                values: new object[] { 13, "CAO MINH", 2, "images/vietnamese2.jpg", 163250, 2022, "Thiên Tài Bên Trái, Kẻ Điên Bên Phải" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "CategoryId", "Cover", "Price", "PublishYear", "Title" },
                values: new object[] { 14, "EMILIE ARIES", 5, "images/vietnamese3.jpg", 69500, 2023, "Sống, Làm Việc Và Yêu" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "CategoryId", "Cover", "Price", "PublishYear", "Title" },
                values: new object[] { 15, "ALBERT RUTHERFORD", 5, "images/vietnamese4.jpg", 86180, 2022, "Rèn Luyện Tư Duy Phản Biện" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "CategoryId", "Cover", "Price", "PublishYear", "Title" },
                values: new object[] { 16, "Daniel Kahneman & Judith Guedj", 5, "images/vietnamese5.jpg", 169000, 2023, "Kế Toán Via Hè - Thực Hành Báo Cáo Tài Chính Căn Bản Từ Quầy Bán Nước Chanh" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the Vietnamese books
            for (int i = 12; i <= 16; i++)
            {
                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: i);
            }
            
            // Remove the "Self-Help" category
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
