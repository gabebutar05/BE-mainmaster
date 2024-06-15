using API_Dinamis.Data;
using API_Dinamis.Dto;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using API_Dinamis.Repository;
using API_Dinamis.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API_Dinamis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StandardValueController : Controller
    {
        private readonly IStandardValueRepository _standardvaluerepository;
        private readonly IMapper _mapper;
        private readonly IAuthRepository _authrepository;
        private readonly DataContext _context;
        public StandardValueController(IStandardValueRepository standardValueRepository, IMapper mapper, IAuthRepository authrepository, DataContext context)
        {
            _standardvaluerepository = standardValueRepository;
            _mapper = mapper;
            _context = context;
            _authrepository = authrepository;
        }

        /*api for getting data start*/
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<StandardValue>))]
        public IActionResult GetStandardValue([FromQuery] int? limit = 0, [FromQuery] int? page = 1, [FromQuery] string? sortby = "d", [FromQuery] string? sortdesc = "Id", [FromQuery] string? keyword = "")
        {
            var data_ = _mapper.Map<List<StandardValueDto>>(_standardvaluerepository.GetStandardValues(limit, page, sortby, sortdesc, keyword));
            var datacount_ = (limit != null && keyword != null) ? data_.Count() : _standardvaluerepository.GetStandardValueCount("main");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = ApiResponseHelper.StdApiResponse("Data load success", data_, datacount: datacount_);

            return Ok(response);
        }

        [HttpGet("PendingStandardValue/")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<StandardValueLogTemp>))]
        public IActionResult GetPendingStandardValues([FromQuery] int? limit = 0, [FromQuery] int? page = 1, [FromQuery] string? sortby = "d", [FromQuery] string? sortdesc = "Id", [FromQuery] string? keyword = "")
        {
            var data_ = _mapper.Map<List<StandardValueDto>>(_standardvaluerepository.GetStandardValuesLogTemp(limit, page, sortby, sortdesc, keyword));
            var datacount_ = (limit != null && keyword != null) ? data_.Count() : _standardvaluerepository.GetStandardValueCount("temp");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = ApiResponseHelper.StdApiResponse("Data load success", data_, datacount: datacount_);

            return Ok(response);
        }

        [HttpGet("{standardValueId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<StandardValue>))]
        [ProducesResponseType(400)]
        public IActionResult GetStandardValue(int standardValueId)
        {
            if (!_standardvaluerepository.StandardValueExists(standardValueId, "main"))
                return NotFound();

            var data_ = _mapper.Map<StandardValue>(_standardvaluerepository.GetStandardValue(standardValueId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(data_);
        }

        [HttpGet("PendingStandardValue/{pendingId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<StandardValueLogTemp>))]
        [ProducesResponseType(400)]
        public IActionResult GetPendingStandardValues(int pendingId)
        {
            if (!_standardvaluerepository.StandardValueExists(pendingId, "temp"))
                return NotFound();

            var data_ = _mapper.Map<StandardValueLogTemp>(_standardvaluerepository.GetStandardValueLogTemp(pendingId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(data_);
        }

        /*api for getting data end*/

        /*api for update data start*/
        [HttpPut("{standardValueId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateStandardValue(int standardValueId, [FromBody] StandardValueForm dataForm)
        {
            var response = "Successfully Added to Pending list. Waiting for approval";
            if (dataForm == null)
                return BadRequest(ModelState);

            if (dataForm != null && dataForm.UpdatedBy != null)
            {
                try
                {
                    var accountChecking = _authrepository.AuthExists(dataForm.UpdatedBy);
                    if (!(bool)accountChecking)
                    {
                        ModelState.AddModelError("", "user invalid");
                        return StatusCode(401, ModelState);
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception details for debugging
                    Console.WriteLine($"An error occurred: {ex}");
                    // Handle or rethrow the exception as needed
                }
            }
            if (!_standardvaluerepository.StandardValueExists(standardValueId, dataForm.Target))
            {
                return NotFound();
            }
            if (dataForm.Target == "main")
            {

                var dataPending_ = _standardvaluerepository.GetStandardValuesLogTemp().Where(dataPending => dataPending.DataId == standardValueId).FirstOrDefault();
                if (dataPending_ != null)
                {
                    ModelState.AddModelError("", "Standard value has a pending information to be approved or rejected first.");
                    return StatusCode(409, ModelState);
                }
            }
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //if four eyes value is false
                    /*
                    var existData = _standardvaluerepository.GetStandardValue(standardValueId);
                    var dataMap = _mapper.Map<StandardValue>(existData);
                    dataMap.DataValue = dataForm.DataValue;
                    dataMap.Action = "U";
                    dataMap.Status = "NA";
                    dataMap.UpdatedBy = dataForm.UpdatedBy;

                    if (!_standardvaluerepository.UpdateStandardValue(dataMap))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }
                    */

                    //if four eyes value is true
                    var existData = _standardvaluerepository.GetStandardValue(standardValueId);
                    if (dataForm.ValueInPercentage == true)
                    {
                        if (existData.ValueType != "P" && existData.ValueType != "N" && existData.ValueType != "I")
                        {
                            ModelState.AddModelError("", "Chosen standard value data cannot set into percentage.");
                            return StatusCode(500, ModelState);
                        }
                    }
                    else
                    {
                        if (existData.ValueType == "P")
                        {
                            ModelState.AddModelError("", "Chosen standard value data cannot set into not percentage data.");
                            return StatusCode(500, ModelState);
                        }
                    }

                    var dataMap = _mapper.Map<StandardValue>(existData);
                    dataMap.Action = "U";
                    dataMap.Status = "NA";
                    if (!_standardvaluerepository.UpdateStandardValue(dataMap))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }
                    var dataMapLogTemp = _mapper.Map<StandardValueLogTemp>(dataForm);
                    dataMapLogTemp.Id = 0;
                    dataMapLogTemp.DataId = standardValueId;
                    dataMapLogTemp.DataName = existData.DataName;
                    dataMapLogTemp.Description = existData.Description;
                    dataMapLogTemp.ValueType = existData.ValueType;
                    dataMapLogTemp.ValueOption = existData.ValueOption;
                    dataMapLogTemp.Action = "U";
                    dataMapLogTemp.Status = "NA";
                    if (!_standardvaluerepository.CreateStandardValueLogTemp(dataMapLogTemp))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log temp");
                        return StatusCode(500, ModelState);
                    }

                    var dataMapLog = _mapper.Map<StandardValueLog>(dataForm);
                    dataMapLog.Id = 0;
                    dataMapLog.DataId = standardValueId;
                    dataMapLog.DataTempId = dataMapLogTemp.Id;
                    dataMapLog.DataName = existData.DataName;
                    dataMapLog.Description = existData.Description;
                    dataMapLog.ValueType = existData.ValueType;
                    dataMapLog.ValueOption = existData.ValueOption;
                    dataMapLog.Action = "U";
                    dataMapLog.Status = "NA";
                    if (!_standardvaluerepository.CreateStandardValueLog(dataMapLog))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log");
                        return StatusCode(500, ModelState);
                    }

                    dbContextTransaction.Commit();

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");

                    if (ex.InnerException != null)
                    {
                        ModelState.AddModelError("InnerExceptionMessage", $"Inner Exception: {ex.InnerException.Message}");
                    }

                    return StatusCode(500, ModelState);
                }
            }
        }
        [HttpPut("editStandardValueTemp/{pendingId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateStandardValueTemp(int pendingId, [FromBody] StandardValueForm dataForm)
        {
            var response = "Successfully Update Pending Standard Value";
            if (dataForm == null)
                return BadRequest(ModelState);

            if (!_standardvaluerepository.StandardValueExists(pendingId, dataForm.Target))
            {
                return NotFound();
            }

            if (dataForm != null && dataForm.UpdatedBy != null)
            {
                try
                {
                    var accountChecking = _authrepository.AuthExists(dataForm.UpdatedBy);
                    if (!(bool)accountChecking)
                    {
                        ModelState.AddModelError("", "user invalid");
                        return StatusCode(401, ModelState);
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception details for debugging
                    Console.WriteLine($"An error occurred: {ex}");
                    // Handle or rethrow the exception as needed
                }
            }
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var existDataLogTemp = _standardvaluerepository.GetStandardValueLogTemp(pendingId);
                    if (dataForm.ValueInPercentage == true)
                    {
                        if (existDataLogTemp.ValueType != "P" && existDataLogTemp.ValueType != "N" && existDataLogTemp.ValueType != "I")
                        {
                            ModelState.AddModelError("", "Chosen standard value data cannot set into percentage.");
                            return StatusCode(500, ModelState);
                        }
                    }
                    else
                    {
                        if (existDataLogTemp.ValueType == "P")
                        {
                            ModelState.AddModelError("", "Chosen standard value data cannot set into not percentage data.");
                            return StatusCode(500, ModelState);
                        }
                    }
                    string existAction = existDataLogTemp.Action;

                    var dataMapLogTemp = _mapper.Map<StandardValueLogTemp>(existDataLogTemp);
                    dataMapLogTemp.DataValue = dataForm.DataValue;
                    dataMapLogTemp.ValueInPercentage = dataForm.ValueInPercentage;
                    dataMapLogTemp.Action = existAction;
                    dataMapLogTemp.Status = "NA";

                    if (!_standardvaluerepository.UpdateStandardValueLogTemp(dataMapLogTemp))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    var dataMapLog = _mapper.Map<StandardValueLog>(existDataLogTemp);
                    dataMapLog.Id = 0;
                    dataMapLog.DataTempId = pendingId;
                    dataMapLog.Status = "NA"; 
                    
                    if (!_standardvaluerepository.CreateStandardValueLog(dataMapLog))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log");
                        return StatusCode(500, ModelState);
                    }

                    dbContextTransaction.Commit();

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");

                    if (ex.InnerException != null)
                    {
                        ModelState.AddModelError("InnerExceptionMessage", $"Inner Exception: {ex.InnerException.Message}");
                    }

                    return StatusCode(500, ModelState);
                }
            }
        }

        /*api for update data end*/

        /*api for approve reject data start*/
        [HttpPost("approvalStandardValue")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult ApprovalStandardValues([FromBody] StatusIdList statusIdList)
        {
            var response = "";
            int totalDataFailed = 0;
            int totalDataSuccess = 0;
            int totalDataProcess = 0;
            int totalDataMissed = 0;

            //checking username & password - useable
            var accountChecking = _authrepository.CheckingAccount(statusIdList.UserName, statusIdList.Password);
            if (!(bool)accountChecking.GetType().GetProperty("result").GetValue(accountChecking, null))
            {
                var message = accountChecking.GetType().GetProperty("message").GetValue(accountChecking, null);
                ModelState.AddModelError("", (string)message);
                return StatusCode(401, ModelState);
            }

            //checking id list - useable
            if (statusIdList.IdList == null || statusIdList.IdList.Count == 0)
            {
                ModelState.AddModelError("", "id list is required");
                return BadRequest(ModelState);
            }

            //checking required field if reject activity
            if (statusIdList.Approval.ToUpper() == "R")
            {
                if (statusIdList.ActionRemarks == null)
                {
                    ModelState.AddModelError("", "remarks is required");
                    return BadRequest(ModelState);
                }
            }

            foreach (var id in statusIdList.IdList)
            {
                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        bool successProcess = true;
                        totalDataProcess = totalDataProcess + 1;

                        int mainID = 0;
                        var dataTempExist = _standardvaluerepository.GetStandardValueLogTemp(Convert.ToInt32(id));
                        if (dataTempExist != null)
                        {
                            if (statusIdList.Approval.ToUpper() == "A")
                            {
                                //var dataForm = new StandardValueForm();
                                if (dataTempExist.Action == "U")
                                {
                                    var mainDataMap = _mapper.Map<StandardValue>(dataTempExist);
                                    mainDataMap.Status = statusIdList.Approval;
                                    mainDataMap.UpdatedBy = dataTempExist.UpdatedBy;
                                    mainDataMap.Action = "N";

                                    mainDataMap.Id = dataTempExist.DataId;
                                    mainID = dataTempExist.DataId;
                                    if (!_standardvaluerepository.UpdateStandardValue(mainDataMap))
                                        successProcess = false;
                                }
                            }
                            if (statusIdList.Approval.ToUpper() == "R")
                            {
                                // reject data process
                                /*update temp log data start*/
                                mainID = dataTempExist.DataId;
                                var mainTempMap = _mapper.Map<StandardValueLogTemp>(dataTempExist);
                                mainTempMap.Status = statusIdList.Approval;
                                mainTempMap.ActionRemarks = statusIdList.ActionRemarks;
                                if (!_standardvaluerepository.UpdateStandardValueLogTemp(mainTempMap))
                                {
                                    successProcess = false;
                                }
                            }
                            if (successProcess == true)
                            {
                                var dataMapLog = _mapper.Map<StandardValueLog>(dataTempExist);
                                dataMapLog.Id = 0;
                                dataMapLog.DataId = mainID;
                                dataMapLog.DataTempId = Convert.ToInt32(id);
                                dataMapLog.Status = statusIdList.Approval;
                                dataMapLog.ActionRemarks = statusIdList.ActionRemarks;
                                if (_standardvaluerepository.CreateStandardValueLog(dataMapLog))
                                {
                                    if (statusIdList.Approval.ToUpper() == "A")
                                    {
                                        //delete temp log
                                        if (!_standardvaluerepository.DeleteStandardValueLogTemp(dataTempExist))
                                            successProcess = false;
                                    }
                                    else
                                    {
                                        successProcess = false;
                                    }

                                    //commit data when all process in a data is true / success
                                    if (successProcess == true)
                                    {
                                        totalDataSuccess = totalDataSuccess + 1;
                                        dbContextTransaction.Commit();
                                    }
                                    else
                                    {

                                        totalDataFailed = totalDataFailed + 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            totalDataFailed = totalDataFailed + 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        ModelState.AddModelError("", $"An error occurred: {ex.Message}");

                        if (ex.InnerException != null)
                        {
                            ModelState.AddModelError("InnerExceptionMessage", $"Inner Exception: {ex.InnerException.Message}");
                        }

                        return StatusCode(500, ModelState);
                    }
                }
            }

            response = "Successfully Process. Data success : " + totalDataSuccess + ". Data failed : " + totalDataFailed + ". Total data missed : " + totalDataMissed + ". Total data process : " + totalDataProcess + ".";

            return Ok(response);
        }
        /*api for approve reject data end*/

        /*api for delete data start*/
        [HttpDelete("deleteStandardValueTemp/{pendingId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteStandardValueTemp(int pendingId, [FromBody] ActionRemark Actremark)
        {
            var response = "";
            if (!_standardvaluerepository.StandardValueExists(pendingId, "temp"))
            {
                return NotFound();
            }
            var existDataLogTemp = _standardvaluerepository.GetStandardValueLogTemp(pendingId);

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var dataMapLog = _mapper.Map<StandardValueLog>(existDataLogTemp);
                    dataMapLog.Id = 0;
                    dataMapLog.DataTempId = pendingId;
                    dataMapLog.ActionRemarks = Actremark.ActionRemarks;
                    if (!_standardvaluerepository.CreateStandardValueLog(dataMapLog))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log");
                        return StatusCode(500, ModelState);
                    }
                    if (_standardvaluerepository.StandardValueExists(existDataLogTemp.DataId, "main"))
                    {
                        var data_ = _standardvaluerepository.GetStandardValue(existDataLogTemp.DataId);
                        var dataMap = _mapper.Map<StandardValue>(data_);
                        dataMap.Action = "N";
                        dataMap.Status = "A";
                        if (!_standardvaluerepository.UpdateStandardValue(dataMap))
                        {
                            ModelState.AddModelError("", "Something went wrong while saving");
                            return StatusCode(500, ModelState);
                        }
                    }

                    /*delete temporary start*/
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);

                    var mainTempMap = _mapper.Map<StandardValueLogTemp>(existDataLogTemp);
                    if (!_standardvaluerepository.DeleteStandardValueLogTemp(mainTempMap))
                    {
                        ModelState.AddModelError("", "Something went wrong deleting branch temp");
                        return StatusCode(500, ModelState);
                    }
                    /*delete temporary end*/

                    response = "Successfully Deleted Pending Holiday";
                    dbContextTransaction.Commit();

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");

                    if (ex.InnerException != null)
                    {
                        ModelState.AddModelError("InnerExceptionMessage", $"Inner Exception: {ex.InnerException.Message}");
                    }

                    return StatusCode(500, ModelState);
                }
            }
        }
        /*api for delete data end*/
    }
}
