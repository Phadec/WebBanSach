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
                    { 11, "Carol D. Tamparo", 4, "images/medical10.jpg", 64.99, 2022, "Diseases of the Human Body" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the medical books
            for (int i = 2; i <= 11; i++)
            {
                migrationBuilder.DeleteData(
                    table: "Books",
                    keyColumn: "Id",
                    keyValue: i);
            }
        }
    }
}
