using MicroFrontendDal.DataModels;
using MicroFrontendDal.DTO.Products;
namespace MicroFrontendDal.BusinessRules.Products
{
    public interface IProducts
    {
        bool PostProductCategory(DtoProductCategory category);
        Category GetProductCategoryById(int id);
        List<Category> GetAllProductCategory();
    }
}
