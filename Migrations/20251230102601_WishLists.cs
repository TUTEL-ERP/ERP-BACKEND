using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class WishLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "image",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ImageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ImageExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Created = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        CreatedBy = table.Column<int>(type: "int", nullable: false),
            //        CreatedDate = table.Column<int>(type: "int", nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        ModifiedBy = table.Column<int>(type: "int", nullable: false),
            //        isDelete = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_image", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "User",
            //    columns: table => new
            //    {
            //        UserId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Username = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_User", x => x.UserId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Brand",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        ImageId = table.Column<int>(type: "int", nullable: true),
            //        Created = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        CreatedBy = table.Column<int>(type: "int", nullable: false),
            //        CreatedDate = table.Column<int>(type: "int", nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        ModifiedBy = table.Column<int>(type: "int", nullable: false),
            //        isDelete = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Brand", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Brand_image_ImageId",
            //            column: x => x.ImageId,
            //            principalTable: "image",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.SetNull);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Categories",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        ImageId = table.Column<int>(type: "int", nullable: true),
            //        Created = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        CreatedBy = table.Column<int>(type: "int", nullable: false),
            //        CreatedDate = table.Column<int>(type: "int", nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        ModifiedBy = table.Column<int>(type: "int", nullable: false),
            //        isDelete = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Categories", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Categories_image_ImageId",
            //            column: x => x.ImageId,
            //            principalTable: "image",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.SetNull);
            //    });

            migrationBuilder.CreateTable(
                name: "WishList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WishList_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateTable(
            //    name: "Product",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        OrignalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
            //        DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
            //        StockQuantity = table.Column<int>(type: "int", nullable: false),
            //        AvearageRating = table.Column<double>(type: "float", nullable: false),
            //        TotalReviews = table.Column<int>(type: "int", nullable: false),
            //        IsFeatured = table.Column<bool>(type: "bit", nullable: false),
            //        CategoryId = table.Column<int>(type: "int", nullable: false),
            //        CategoriesId = table.Column<int>(type: "int", nullable: false),
            //        BrandId = table.Column<int>(type: "int", nullable: false),
            //        ThumbnailId = table.Column<int>(type: "int", nullable: true),
            //        Created = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        CreatedBy = table.Column<int>(type: "int", nullable: false),
            //        CreatedDate = table.Column<int>(type: "int", nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        ModifiedBy = table.Column<int>(type: "int", nullable: false),
            //        isDelete = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Product", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Product_Brand_BrandId",
            //            column: x => x.BrandId,
            //            principalTable: "Brand",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Product_Categories_CategoriesId",
            //            column: x => x.CategoriesId,
            //            principalTable: "Categories",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Product_image_ThumbnailId",
            //            column: x => x.ThumbnailId,
            //            principalTable: "image",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.SetNull);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ProductReview",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Review = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        ProductId = table.Column<int>(type: "int", nullable: false),
            //        Created = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        CreatedBy = table.Column<int>(type: "int", nullable: false),
            //        CreatedDate = table.Column<int>(type: "int", nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        ModifiedBy = table.Column<int>(type: "int", nullable: false),
            //        isDelete = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ProductReview", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_ProductReview_Product_ProductId",
            //            column: x => x.ProductId,
            //            principalTable: "Product",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            migrationBuilder.CreateTable(
                name: "WishListItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    WishListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WishListItems_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WishListItems_WishList_WishListId",
                        column: x => x.WishListId,
                        principalTable: "WishList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Brand_ImageId",
            //    table: "Brand",
            //    column: "ImageId",
            //    unique: true,
            //    filter: "[ImageId] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Categories_ImageId",
            //    table: "Categories",
            //    column: "ImageId",
            //    unique: true,
            //    filter: "[ImageId] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Product_BrandId",
            //    table: "Product",
            //    column: "BrandId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Product_CategoriesId",
            //    table: "Product",
            //    column: "CategoriesId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Product_ThumbnailId",
            //    table: "Product",
            //    column: "ThumbnailId",
            //    unique: true,
            //    filter: "[ThumbnailId] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProductReview_ProductId",
            //    table: "ProductReview",
            //    column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WishList_UserId",
                table: "WishList",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WishListItems_ProductId",
                table: "WishListItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WishListItems_WishListId",
                table: "WishListItems",
                column: "WishListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductReview");

            migrationBuilder.DropTable(
                name: "WishListItems");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "WishList");

            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "image");
        }
    }
}
