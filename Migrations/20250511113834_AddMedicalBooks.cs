using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Week2.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicalBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add a new category "Self-Help" if it doesn't exist
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[] { 5, "Self-Help" });
                
            // Insert medical books (ID 2-11) that appear in the Designer file
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "CategoryId", "Cover", "Price", "PublishYear", "Title" },
                values: new object[,]
                {
                    { 2, "Henry Gray", 4, "images/medical1.jpg", 89.99, 2020, "Gray's Anatomy" },
                    { 3, "J. Larry Jameson", 4, "images/medical2.jpg", 149.99, 2022, "Harrison's Principles of Internal Medicine" },
                    { 4, "Vinay Kumar", 4, "images/medical3.jpg", 79.99, 2021, "Robbins Basic Pathology" },
                    { 5, "John E. Hall", 4, "images/medical4.jpg", 94.99, 2021, "Guyton and Hall Textbook of Medical Physiology" },
                    { 6, "Morris J. Brown", 4, "images/medical5.jpg", 69.99, 2019, "Clinical Pharmacology" },
                    { 7, "Stuart H. Ralston", 4, "images/medical6.jpg", 84.99, 2022, "Davidson's Principles and Practice of Medicine" },
                    { 8, "Frank H. Netter", 4, "images/medical7.jpg", 74.99, 2019, "Netter's Atlas of Human Anatomy" },
                    { 9, "Ian B. Wilkinson", 4, "images/medical8.jpg", 49.99, 2020, "Oxford Handbook of Clinical Medicine" },
                    { 10, "Parveen Kumar", 4, "images/medical9.jpg", 89.99, 2021, "Kumar and Clark's Clinical Medicine" },
                    { 11, "Carol D. Tamparo", 4, "images/medical10.jpg", 64.99, 2022, "Diseases of the Human Body" },
                    // Add Vietnamese books (IDs 12-16)
                    { 12, "CLINT EMERSON", 2, "images/vietnamese1.jpg", 99000, 2022, "100 Kỹ Năng Sinh Tồn" },
                    { 13, "CAO MINH", 2, "images/vietnamese2.jpg", 163250, 2022, "Thiên Tài Bên Trái, Kẻ Điên Bên Phải" },
                    { 14, "EMILIE ARIES", 5, "images/vietnamese3.jpg", 69500, 2023, "Sống, Làm Việc Và Yêu" },
                    { 15, "ALBERT RUTHERFORD", 5, "images/vietnamese4.jpg", 86180, 2022, "Rèn Luyện Tư Duy Phản Biện" },
                    { 16, "Daniel Kahneman & Judith Guedj", 5, "images/vietnamese5.jpg", 169000, 2023, "Kế Toán Via Hè - Thực Hành Báo Cáo Tài Chính Căn Bản Từ Quầy Bán Nước Chanh" },
                    // Add Vietnamese books from AddNewBookFromTiki
                    { 20, "Đặng Thủy Trâm", 2, "images/tiki1.jpg", 64.800, 2005, "Nhật ký Đặng Thủy Trâm" },
                    { 21, "James Clear", 2, "images/tiki2.jpg", 143.000, 2019, "Thay Đổi Tí Hon - Hiệu Quả Bất Ngờ Atomic Habits" },
                    { 22, "Khaled Hosseini", 1, "images/tiki3.jpg", 99.600, 2018, "Ngàn Mặt Trời Rực Rỡ" },
                    { 23, "Daniel Goleman", 2, "images/tiki4.jpg", 134.000, 2020, "Dám Đặt Một Bầy Sói Hay Chăn Một Bầy Cừu" }
                });
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
            
            for (int i = 20; i <= 23; i++)
            {
                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: i);
            }
            
            // Remove the medical books
            for (int i = 2; i <= 11; i++)
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
