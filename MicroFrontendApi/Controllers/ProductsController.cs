using MicroFrontendDal.BusinessRules.Logger;
using MicroFrontendDal.BusinessRules.Products;
using MicroFrontendDal.DataModels;
using MicroFrontendDal.DTO.Authentication;
using MicroFrontendDal.DTO.Common;
using MicroFrontendDal.DTO.Products;
using MicroFrontendDal.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static MicroFrontendDal.BusinessConstants.BusinessConstant;

namespace MicroFrontendApi.Controllers
{
    [Route("Products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        protected IProducts UserRepository;
        private readonly Log Logger;
        public ProductsController(IProducts userRepository)
        {
            UserRepository = userRepository;
            Logger = new Log();
        }

        [HttpPost]
        [Route("PostCategory")]
        public async Task<IActionResult> PostCategory(DtoFrontendData objInputData)
        {
            try
            {
                var decryptedData = Utilities.DecryptStringAes(objInputData.ObjInputString);
                if (decryptedData != null)
                {
                    DtoProductCategory data = JsonConvert.DeserializeObject<DtoProductCategory>(decryptedData);
                    var Response = UserRepository.PostProductCategory(data);
                    if (Response)
                    {
                        var tempResponse = new DtoResponse()
                        {
                            Status = ResponseStatus.Success,
                            Message = CustomMessages.CM013
                        };
                        var jsonResponse = JsonConvert.SerializeObject(tempResponse);
                        return StatusCode(StatusCodes.Status200OK, Utilities.EncryptStringAes(jsonResponse));
                    }
                    else
                    {
                        var tempResponse = new DtoResponse()
                        {
                            Status = ResponseStatus.Failed,
                            Message = CustomMessages.CM014
                        };
                        var jsonResponse = JsonConvert.SerializeObject(tempResponse);
                        return StatusCode(StatusCodes.Status200OK, Utilities.EncryptStringAes(jsonResponse));
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { ErrorResponse.ErrorResponseMessage });
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("ProductsController", "PostCategory", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }

        [HttpGet]
        [Route("GetProductCategoryById")]
        public async Task<IActionResult> GetProductCategoryById(string data)
        {
            try
            {
                var decryptedData = Utilities.DecryptStringAes(data);
                if (decryptedData != null)
                {
                    int JsonData = JsonConvert.DeserializeObject<int>(decryptedData);
                    var Response = UserRepository.GetProductCategoryById(JsonData);
                    if (Response != null)
                    {
                        var tempResponse = new { Status = ResponseStatus.Success, Data = Response };
                        var jsonResponse = JsonConvert.SerializeObject(tempResponse);
                        return StatusCode(StatusCodes.Status200OK, new { Response = Utilities.EncryptStringAes(jsonResponse) });
                    }
                    else
                    {
                        var tempResponse = new { Status = ResponseStatus.Failed, Data = Response };
                        var jsonResponse = JsonConvert.SerializeObject(tempResponse);
                        return StatusCode(StatusCodes.Status200OK, new { Response = Utilities.EncryptStringAes(jsonResponse) });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { ErrorResponse.ErrorResponseMessage });
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("ProductsController", "PostCategory", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }

        [HttpGet]
        [Route("GetAllProductCategory")]
        public async Task<IActionResult> GetAllProductCategory()
        {
            try
            {

                List<Category> Response = UserRepository.GetAllProductCategory();
                if (Response.Count != 0)
                {
                    var tempResponse = new { Status = ResponseStatus.Success, Data = Response };
                    var jsonResponse = JsonConvert.SerializeObject(tempResponse);
                    return StatusCode(StatusCodes.Status200OK, new { Response = Utilities.EncryptStringAes(jsonResponse) });
                }
                else
                {
                    var tempResponse = new { Status = ResponseStatus.Failed, Data = Response };
                    var jsonResponse = JsonConvert.SerializeObject(tempResponse);
                    return StatusCode(StatusCodes.Status200OK, new { Response = Utilities.EncryptStringAes(jsonResponse) });
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("ProductsController", "PostCategory", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }
    }
}
