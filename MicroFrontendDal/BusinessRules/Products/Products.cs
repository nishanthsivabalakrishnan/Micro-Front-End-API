using MicroFrontendDal.BusinessRules.Logger;
using MicroFrontendDal.DataModels;
using MicroFrontendDal.DTO.Products;
using Microsoft.EntityFrameworkCore;


namespace MicroFrontendDal.BusinessRules.Products
{
    public class Products : IProducts
    {
        private readonly Log Logger;
        private readonly Utilities.Utilities Utilities;
        public MicroFrontEndDbContext DbContext { get; set; }
        private const string FileName = "Products";

        public Products()
        {
            DbContext = new MicroFrontEndDbContext();
            Logger = new Log();
            Utilities = new Utilities.Utilities();
        }

        public bool PostProductCategory(DtoProductCategory category)
        {
            try
            {
                Category productCategory = DbContext.Categories.FirstOrDefault(x => x.CategoryName == category.CategoryName && x.IsActive == true);
                if (productCategory == null && category.ProductId == 0)
                {
                    Category newCategory = new()
                    {
                        CategoryName = category.CategoryName,
                        CategoryDescription = category.CategoryDescription,
                        CreatedOn = DateTime.Now,
                        IsActive = true,
                    };
                    DbContext.Categories.Add(newCategory);
                    DbContext.SaveChanges();
                    return true;
                }
                else
                {
                    productCategory.CategoryName = category.CategoryName;
                    productCategory.CategoryDescription = category.CategoryDescription;
                    productCategory.UpdatedOn = DateTime.Now;
                    DbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "PostProductCategory", ex);
                throw;
            }
        }

        public Category GetProductCategoryById(int id)
        {
            try
            {
                Category productCategory = DbContext.Categories.FirstOrDefault(x => x.CategoryId == id && x.IsActive == true);
                if (productCategory != null)
                {
                    return productCategory;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "GetProductCategoryList", ex);
                throw;
            }
        }
        public List<Category> GetAllProductCategory()
        {
            try
            {
                List<Category> productCategory = DbContext.Categories.Where(x => x.IsActive == true).ToList();
                if (productCategory != null)
                {
                    return productCategory;
                }
                else
                {
                    return new List<Category>();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(FileName, "GetProductCategoryList", ex);
                throw;
            }
        }
    }
}
