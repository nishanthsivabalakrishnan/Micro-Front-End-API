using MicroFrontendDal.BusinessRules.Authentication;
using MicroFrontendDal.BusinessRules.Logger;
using MicroFrontendDal.BusinessRules.Management;
using MicroFrontendDal.DTO.Common;
using MicroFrontendDal.DTO.Management;
using MicroFrontendDal.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Runtime.InteropServices;
using static MicroFrontendDal.BusinessConstants.BusinessConstant;

namespace MicroFrontendApi.Controllers
{
    [Route("Management")]
    [EnableCors("corsapp")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class ManagementController : ControllerBase
    {
        #region Variable Region
        protected IManagement UserRepository;
        private readonly Log Logger;
        #endregion

        #region Constructor
        public ManagementController(IManagement userRepository,Log log)
        {
            UserRepository = userRepository;
            Logger = log;
        }
        #endregion

        [HttpGet("GetAllUsers")]
        [Authorize(Roles ="1")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var Response = UserRepository.GetAllUsers();
                var jsonResponse = JsonConvert.SerializeObject(Response);
                var temp = Utilities.EncryptStringAes(jsonResponse);
                return StatusCode(StatusCodes.Status200OK, new { Response = Utilities.EncryptStringAes(jsonResponse) });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("AuthenticationController", "GetAllRoles", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }

        [HttpGet("GetAllReportingMaster")]
        public async Task<IActionResult> GetAllReportingMaster(string Id)
        {
            try
            {
                Id = Id.Replace(' ', '+');
                var decryptedData = Utilities.DecryptStringAes(Id);
                int roleId= JsonConvert.DeserializeObject<int>(decryptedData);
                var data = UserRepository.GetReportingList(roleId);
                var jsonResponse = JsonConvert.SerializeObject(data);
                return StatusCode(StatusCodes.Status200OK, new { Response = Utilities.EncryptStringAes(jsonResponse) });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("AuthenticationController", "GetAllRoles", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }

        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(string Id)
        {
            try
            {
                Id = Id.Replace(' ', '+');
                var decryptedData = Utilities.DecryptStringAes(Id);
                int userId = JsonConvert.DeserializeObject<int>(decryptedData);
                var data = UserRepository.GetUserDetailsWithManagerList(userId);
                var jsonResponse = JsonConvert.SerializeObject(data);
                return StatusCode(StatusCodes.Status200OK, new { Response = Utilities.EncryptStringAes(jsonResponse) });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("AuthenticationController", "GetUserById", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }

        [HttpGet("GetTeamList")]
        public async Task<IActionResult> GetTeamList(string Id)
        {
            try
            {
                Id = Id.Replace(' ', '+');
                var decryptedData = Utilities.DecryptStringAes(Id);
                int userId = JsonConvert.DeserializeObject<int>(decryptedData);
                var data = UserRepository.GetTeamList(userId);
                var jsonResponse = JsonConvert.SerializeObject(data);
                return StatusCode(StatusCodes.Status200OK, new { Response = Utilities.EncryptStringAes(jsonResponse) });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("AuthenticationController", "GetUserById", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }

        [HttpPost("PostTask")]
        public async Task<IActionResult> PostTask(DtoFrontendData frontendData)
        {
            try
            {
                var decryptedData = Utilities.DecryptStringAes(frontendData.ObjInputString);
                var decData = JsonConvert.DeserializeObject<DtoPostTask>(decryptedData);
                var data = UserRepository.PostTask(decData);
                var jsonResponse = JsonConvert.SerializeObject(data);
                return StatusCode(StatusCodes.Status200OK, new { Response = Utilities.EncryptStringAes(jsonResponse) });
            }
            catch(Exception ex)
            {
                Logger.ErrorLog("AuthenticationController", "PostTask", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }

        [HttpGet("GetTaskBoard")]
        public async Task<IActionResult> GetTaskBoard(string Id)
        {
            try
            {
                Id = Id.Replace(' ', '+');
                var decryptedData = Utilities.DecryptStringAes(Id);
                var decData = JsonConvert.DeserializeObject<int>(decryptedData);
                var data = UserRepository.GetTaskBoard(decData);
                var jsonResponse = JsonConvert.SerializeObject(data);
                return StatusCode(StatusCodes.Status200OK, new { Response = Utilities.EncryptStringAes(jsonResponse) });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("AuthenticationController", "GetTaskBoard", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }

        [HttpPost("DeleteTaskById")]
        public async Task<IActionResult> DeleteTaskById(DtoFrontendData frontendData)
        {
            try
            {
                var decryptedData = Utilities.DecryptStringAes(frontendData.ObjInputString);
                var decData = JsonConvert.DeserializeObject<int>(decryptedData);
                var data = UserRepository.DeleteTaskById(decData);
                var jsonResponse = JsonConvert.SerializeObject(data);
                return StatusCode(StatusCodes.Status200OK, new { Response = Utilities.EncryptStringAes(jsonResponse) });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("AuthenticationController", "DeleteTaskById", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }

        [HttpPost("ChangetaskStatus")]
        public async Task<IActionResult> ChangetaskStatus(DtoFrontendData frontendData)
        {
            try
            {
                var decryptedData = Utilities.DecryptStringAes(frontendData.ObjInputString);
                var decData = JsonConvert.DeserializeObject<DtoChangeTaskStatus>(decryptedData);
                var data = UserRepository.ChangeTaskStatus(decData);
                var jsonResponse = JsonConvert.SerializeObject(data);
                return StatusCode(StatusCodes.Status200OK, new { Response = Utilities.EncryptStringAes(jsonResponse) });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("AuthenticationController", "ChangetaskStatus", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }

        [HttpPost("PostUserDetails")]
        public async Task<IActionResult> PostUserDetails(DtoFrontendData frontendData)
        {
            try
            {
                var decryptedData = Utilities.DecryptStringAes(frontendData.ObjInputString);
                var decData = JsonConvert.DeserializeObject<DtoPostUserDetails>(decryptedData);
                var data = UserRepository.PostUserProfileDetails(decData);
                var jsonResponse = JsonConvert.SerializeObject(data);
                return StatusCode(StatusCodes.Status200OK, new { Response = Utilities.EncryptStringAes(jsonResponse) });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("AuthenticationController", "PostUserDetails", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }

        [HttpGet("GetUserDetailsById")]
        public async Task<IActionResult> GetUserDetailsById(string Id)
        {
            try
            {
                Id = Id.Replace(' ', '+');
                var decryptedData = Utilities.DecryptStringAes(Id);
                var decData = JsonConvert.DeserializeObject<int>(decryptedData);
                var data = UserRepository.GetUserDetailsById(decData);
                var jsonResponse = JsonConvert.SerializeObject(data);
                return StatusCode(StatusCodes.Status200OK, new { Response = Utilities.EncryptStringAes(jsonResponse) });
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("AuthenticationController", "GetUserDetailsById", ex);
                return StatusCode(StatusCodes.Status501NotImplemented, new { ErrorResponse.ErrorResponseMessage });
            }
        }
    }
}
