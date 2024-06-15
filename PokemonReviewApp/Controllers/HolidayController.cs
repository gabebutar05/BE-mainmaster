using API_Dinamis.Data;
using API_Dinamis.Dto;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using API_Dinamis.Repository;
using API_Dinamis.Utilities;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Dinamis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayController : Controller
    {
        private readonly IHolidayRepository _holidayrepository;
        private readonly IMapper _mapper;
        private readonly IAuthRepository _authrepository;
        private readonly DataContext _context;
        public HolidayController(IHolidayRepository holidayRepository, IMapper mapper, IAuthRepository authrepository, DataContext context)
        {
            _holidayrepository = holidayRepository;
            _mapper = mapper;
            _context = context;
            _authrepository = authrepository;
        }

        /*api for getting data start*/

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Holiday>))]

        public IActionResult GetHoliday([FromQuery] int? limit=0, [FromQuery] int? page=1, [FromQuery] string? sortby="d", [FromQuery] string? sortdesc="Id", [FromQuery] string? keyword = "")
        {
            var data_ = _mapper.Map<List<HolidayDto>>(_holidayrepository.GetHolidays(limit,page,sortby,sortdesc,keyword));
            var datacount_ = (limit != null && keyword != null) ? data_.Count() : _holidayrepository.GetHolidayCount("main");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = ApiResponseHelper.StdApiResponse("Data load success", data_, datacount: datacount_);

            return Ok(response);

        }

        [HttpGet("PendingHoliday/")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Holiday>))]
        public IActionResult GetPendingHolidays([FromQuery] int? limit = 0, [FromQuery] int? page = 1, [FromQuery] string? sortby = "d", [FromQuery] string? sortdesc = "Id", [FromQuery] string? keyword = "")
        {
            var data_ = _mapper.Map<List<HolidayTempDto>>(_holidayrepository.GetHolidaysLogTemp(limit, page, sortby, sortdesc, keyword));
            var datacount_ = (limit != null && keyword != null) ? data_.Count() : _holidayrepository.GetHolidayCount("temp");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = ApiResponseHelper.StdApiResponse("Data load success", data_, datacount: datacount_);

            return Ok(response);
        }

        [HttpGet("{holidayId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Holiday>))]
        [ProducesResponseType(400)]
        public IActionResult GetHoliday(int holidayId)
        {
            if (!_holidayrepository.HolidayExists(holidayId, "main"))
                return NotFound();

            var data_ = _mapper.Map<Holiday>(_holidayrepository.GetHoliday(holidayId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(data_);
        }

        [HttpGet("PendingHoliday/{pendingId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Holiday>))]
        [ProducesResponseType(400)]
        public IActionResult GetPendingHolidays(int pendingId)
        {
            if (!_holidayrepository.HolidayExists(pendingId, "temp"))
                return NotFound();

            var data_ = _mapper.Map<HolidayLogTemp>(_holidayrepository.GetHolidayLogTemp(pendingId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(data_);
        }

        /*api for getting data end*/

        /*api for process main data start*/
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateHoliday([FromBody] HolidayForm dataForm)
        {
            var response = "Successfully Added to Pending list. Waiting for approval";

            /*checking start*/
            if (dataForm == null)
                return BadRequest(ModelState);

            if(dataForm != null && dataForm.UpdatedBy != null)
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

            var data_ = _holidayrepository.GetHolidays().Where(data => data.HolidayDate == dataForm.HolidayDate).FirstOrDefault();
            if (data_ != null)
            {
                ModelState.AddModelError("", "Holiday already exists");
                return StatusCode(409, ModelState);
            }

            var dataPending_ = _holidayrepository.GetHolidaysLogTemp().Where(dataPending => dataPending.HolidayDate == dataForm.HolidayDate).FirstOrDefault();
            if (dataPending_ != null)
            {
                ModelState.AddModelError("", "Holiday has a pending information to be approved or rejected first.");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            /*checking end*/

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    /*set for process data start*/
                    var dataMapLogTemp = _mapper.Map<HolidayLogTemp>(dataForm);
                    dataMapLogTemp.Action = "C";
                    dataMapLogTemp.Status = "NA";
                    if (!_holidayrepository.CreateHolidayLogTemp(dataMapLogTemp))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    var dataMapLog = _mapper.Map<HolidayLog>(dataForm);
                    dataMapLog.Action = "C";
                    dataMapLog.Status = "NA";
                    if (!_holidayrepository.CreateHolidayLog(dataMapLog))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log");
                        return StatusCode(500, ModelState);
                    }

                    dbContextTransaction.Commit();

                    return Ok(response);
                    /*set for process data end*/
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

        [HttpPut("{holidayId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateHoliday(int holidayId, [FromBody] HolidayForm dataForm)
        {
            var response = "Successfully Added to Pending list. Waiting for approval";
            /*checking start*/
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

            if (!_holidayrepository.HolidayExists(holidayId, "main"))
            {
                return NotFound();
            }

            var data_ = _holidayrepository.GetHolidays().Where(data => data.HolidayDate == dataForm.HolidayDate && data.ID != holidayId).FirstOrDefault();
            if (data_ != null)
            {
                ModelState.AddModelError("", "Holiday already exists");
                return StatusCode(409, ModelState);
            }

            var dataPending_ = _holidayrepository.GetHolidaysLogTemp().Where(dataPending => dataPending.HolidayDate == dataForm.HolidayDate && dataPending.HolidayId != holidayId).FirstOrDefault();
            if (dataPending_ != null)
            {
                ModelState.AddModelError("", "Holiday has a pending information to be approved or rejected first.");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            /*checking end*/

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var existData = _holidayrepository.GetHoliday(holidayId);
                    var dataMap = _mapper.Map<Holiday>(existData);
                    dataMap.Action = "U";
                    dataMap.Status = "NA";
                    if (!_holidayrepository.UpdateHoliday(dataMap))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    var dataMapLogTemp = _mapper.Map<HolidayLogTemp>(dataForm);
                    dataMapLogTemp.Id = 0;
                    dataMapLogTemp.HolidayId = holidayId;
                    dataMapLogTemp.Action = "U";
                    dataMapLogTemp.Status = "NA";
                    if (!_holidayrepository.CreateHolidayLogTemp(dataMapLogTemp))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log temp");
                        return StatusCode(500, ModelState);
                    }

                    var dataMapLog = _mapper.Map<HolidayLog>(dataForm);
                    dataMapLog.Id = 0;
                    dataMapLog.HolidayId = holidayId;
                    dataMapLog.HolidayTempId = dataMapLogTemp.Id;
                    dataMapLog.Action = "U";
                    dataMapLog.Status = "NA";
                    if (!_holidayrepository.CreateHolidayLog(dataMapLog))
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

        [HttpDelete("{holidayId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteHoliday(int holidayId)
        {
            var response = "Successfully Added to Pending list. Waiting for approval";

            if (!_holidayrepository.HolidayExists(holidayId, "main"))
            {
                return NotFound();
            }
            var dataPending_ = _holidayrepository.GetHolidaysLogTemp().Where(dataPending => dataPending.HolidayId == holidayId).FirstOrDefault();
            if (dataPending_ != null)
            {
                ModelState.AddModelError("", "Holiday has a pending information to be approved or rejected first.");
                return StatusCode(409, ModelState);
            }

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var existData = _holidayrepository.GetHoliday(holidayId);
                    var dataMap = _mapper.Map<Holiday>(existData);
                    dataMap.Action = "D";
                    dataMap.Status = "NA";
                    if (!_holidayrepository.UpdateHoliday(dataMap))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    var dataMapLogTemp = _mapper.Map<HolidayLogTemp>(dataMap);
                    dataMapLogTemp.Id = 0;
                    dataMapLogTemp.HolidayId = holidayId;
                    dataMapLogTemp.Action = "D";
                    dataMapLogTemp.Status = "NA";
                    dataMapLogTemp.Remarks = "";
                    if (!_holidayrepository.CreateHolidayLogTemp(dataMapLogTemp))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log temp");
                        return StatusCode(500, ModelState);
                    }

                    var dataMapLog = _mapper.Map<HolidayLog>(dataMap);
                    dataMapLog.Id = 0;
                    dataMapLog.HolidayId = holidayId;
                    dataMapLog.HolidayTempId = dataMapLogTemp.Id;
                    dataMapLog.Action = "D";
                    dataMapLog.Status = "NA";
                    dataMapLog.Remarks = "";
                    if (!_holidayrepository.CreateHolidayLog(dataMapLog))
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
        /*api for process main data end*/

        /*api for process temp data start*/
        [HttpPut("editHolidayTemp/{pendingId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateHolidayTemp(int pendingId, [FromBody] HolidayForm dataForm)
        {
            var response = "Successfully Update Pending Holiday";

            /*checking start*/
            if (dataForm == null)
                return BadRequest(ModelState);

            if (!_holidayrepository.HolidayExists(pendingId,"temp"))
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

            var data_ = _holidayrepository.GetHolidays().Where(data => data.HolidayDate == dataForm.HolidayDate);
            if (dataForm.HolidayID > 0)
            {
                data_ = data_.Where(data => data.ID != dataForm.HolidayID);
            }
            var mainData_ = data_.FirstOrDefault();
            if (mainData_ != null)
            {
                ModelState.AddModelError("", "Holiday already exists");
                return StatusCode(409, ModelState);
            }

            var dataPending_ = _holidayrepository.GetHolidaysLogTemp().Where(dataPending => dataPending.HolidayDate == dataForm.HolidayDate && dataPending.ID != pendingId).FirstOrDefault();
            if (dataPending_ != null)
            {
                ModelState.AddModelError("", "Holiday has a pending information to be approved or rejected first.");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            /*checking end*/

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var existDataLogTemp = _holidayrepository.GetHolidayLogTemp(pendingId);
                    string existAction = existDataLogTemp.Action;
                    _context.Entry(existDataLogTemp).State = EntityState.Detached;

                    /*set for process data start*/
                    var dataMapLogTemp = _mapper.Map<HolidayLogTemp>(dataForm);
                    dataMapLogTemp.Action = existAction;
                    dataMapLogTemp.Status = "NA";

                    _context.Entry(dataMapLogTemp).State = EntityState.Modified;

                    if (!_holidayrepository.UpdateHolidayLogTemp(dataMapLogTemp))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    var dataMapLog = _mapper.Map<HolidayLog>(existDataLogTemp);
                    dataMapLog.Id = 0;
                    dataMapLog.HolidayTempId = pendingId;
                    dataMapLog.Status = "NA";
                    if (!_holidayrepository.CreateHolidayLog(dataMapLog))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log");
                        return StatusCode(500, ModelState);
                    }

                    dbContextTransaction.Commit();

                    return Ok(response);
                    /*set for process data end*/
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
        
        [HttpDelete("deleteHolidayTemp/{pendingId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteHolidayTemp(int pendingId, [FromBody] ActionRemark Actremark)
        {
            var response = "";
            if (!_holidayrepository.HolidayExists(pendingId, "temp"))
            {
                return NotFound();
            }
            var existDataLogTemp = _holidayrepository.GetHolidayLogTemp(pendingId);

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var dataMapLog = _mapper.Map<HolidayLog>(existDataLogTemp);
                    dataMapLog.Id = 0;
                    dataMapLog.HolidayTempId = pendingId;
                    dataMapLog.ActionRemarks = Actremark.ActionRemarks;
                    if (!_holidayrepository.CreateHolidayLog(dataMapLog))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log");
                        return StatusCode(500, ModelState);
                    }
                    if (_holidayrepository.HolidayExists(existDataLogTemp.HolidayId, "main"))
                    {
                        var data_ = _holidayrepository.GetHoliday(existDataLogTemp.HolidayId);
                        var dataMap = _mapper.Map<Holiday>(data_);
                        dataMap.Action = "N";
                        dataMap.Status = "A";
                        if (!_holidayrepository.UpdateHoliday(dataMap))
                        {
                            ModelState.AddModelError("", "Something went wrong while saving");
                            return StatusCode(500, ModelState);
                        }
                    }
                    
                    /*delete temporary start*/
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);

                    var mainTempMap = _mapper.Map<HolidayLogTemp>(existDataLogTemp);
                    if (!_holidayrepository.DeleteHolidayLogTemp(mainTempMap))
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
        /*api for process temp data end*/

        /*api for approval start*/
        [HttpPost("approvalHoliday")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult ApprovalHolidays([FromBody] StatusIdList statusIdList)
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
                        var dataTempExist = _holidayrepository.GetHolidayLogTemp(Convert.ToInt32(id));
                        if (dataTempExist != null)
                        {
                            if (statusIdList.Approval.ToUpper() == "A")
                            {
                                var dataForm = new HolidayForm();
                                if (dataTempExist.Action == "C" || dataTempExist.Action == "U")
                                {
                                    var mainDataMap = _mapper.Map<Holiday>(dataForm);
                                    mainDataMap.HolidayDate = dataTempExist.HolidayDate;
                                    mainDataMap.Description = dataTempExist.Description;
                                    mainDataMap.Remarks = dataTempExist.Remarks;
                                    mainDataMap.Status = statusIdList.Approval;
                                    mainDataMap.UpdatedBy = dataTempExist.UpdatedBy;
                                    mainDataMap.Action = "N";

                                    if (dataTempExist.Action == "U")
                                    {
                                        //update exist main data
                                        mainDataMap.Id = dataTempExist.HolidayId;
                                        mainID = dataTempExist.HolidayId;
                                        if (!_holidayrepository.UpdateHoliday(mainDataMap))
                                            successProcess = false;
                                    }
                                    if (dataTempExist.Action == "C")
                                    {
                                        //create new main data
                                        mainID = mainDataMap.Id;
                                        if (!_holidayrepository.CreateHoliday(mainDataMap))
                                            successProcess = false;
                                    }
                                }
                                if (dataTempExist.Action == "D")
                                {
                                    //process to delete data
                                    //check holidayid
                                    if (dataTempExist.HolidayId == 0)
                                    {
                                        successProcess = false;
                                    }
                                    else
                                    {
                                        //check holidayid exist
                                        var dataExist = _holidayrepository.GetHoliday(dataTempExist.HolidayId);
                                        if (dataExist == null)
                                        {
                                            successProcess = false;
                                        }
                                        else
                                        {
                                            //process deleting
                                            mainID = dataTempExist.HolidayId;
                                            if (!_holidayrepository.DeleteHoliday(dataExist))
                                                successProcess = false;
                                        }
                                    }
                                }
                            }
                            if (statusIdList.Approval.ToUpper() == "R")
                            {
                                //reject data process
                                /*update temp log data start*/
                                mainID = dataTempExist.HolidayId;
                                var mainTempMap = _mapper.Map<HolidayLogTemp>(dataTempExist);
                                mainTempMap.Status = statusIdList.Approval;
                                mainTempMap.ActionRemarks = statusIdList.ActionRemarks;
                                if (!_holidayrepository.UpdateHolidayLogTemp(mainTempMap))
                                {
                                    successProcess = false;
                                }
                            }


                            //set log if data process above success
                            if (successProcess == true)
                            {
                                var dataMapLog = _mapper.Map<HolidayLog>(dataTempExist);
                                dataMapLog.Id = 0;
                                dataMapLog.HolidayId = mainID;
                                dataMapLog.HolidayTempId = Convert.ToInt32(id);
                                dataMapLog.Status = statusIdList.Approval;
                                dataMapLog.ActionRemarks = statusIdList.ActionRemarks;
                                if (_holidayrepository.CreateHolidayLog(dataMapLog))
                                {
                                    if (statusIdList.Approval.ToUpper() == "A")
                                    {
                                        //delete temp log
                                        if (!_holidayrepository.DeleteHolidayLogTemp(dataTempExist))
                                            successProcess = false;
                                    }
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
                            else
                            {
                                totalDataFailed = totalDataFailed + 1;
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
        /*api for approval end*/
    }
}
