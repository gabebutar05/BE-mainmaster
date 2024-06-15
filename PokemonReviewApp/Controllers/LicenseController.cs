using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using API_Dinamis.Repository;
using System.Collections.Generic;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using API_Dinamis.Helper;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;
using System.Numerics;
using API_Dinamis.Data;
using Microsoft.EntityFrameworkCore;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using API_Dinamis.Dto;
using API_Dinamis.Utilities;
using System.ComponentModel;
using License = API_Dinamis.Models.License;
using Microsoft.EntityFrameworkCore.Storage;

namespace API_Dinamis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenseController : Controller
    {
        private readonly ILicenseRepository _licenserepository;
        private readonly IMapper _mapper;
        private readonly IAuthRepository _authrepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly DataContext _context;

        public LicenseController(ILicenseRepository licenseRepository, IMapper mapper, IAuthRepository authrepository, IPasswordHasher passwordHasher, DataContext context)
        {
            _licenserepository = licenseRepository;
            _mapper = mapper;
            _authrepository = authrepository;
            _passwordHasher = passwordHasher;
            _context = context;
        }

        //[HttpGet("getlicenses/")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<License>))]
        public IActionResult GetLicense([FromQuery] int? limit = 0, [FromQuery] int? page = 1, [FromQuery] string? sortby = "d", [FromQuery] string? sortdesc = "id", [FromQuery] string? keyword = "")
        {
            var licenses = _mapper.Map<List<LicenseDtoForm>>(_licenserepository.GetLicenses(limit, page, sortby, sortdesc, keyword));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var count = 0;

            if (keyword == "")
            {
                count = _licenserepository.Getdatacount();
            }
            else
            {
                count = _licenserepository.GetLicensesAdvance(keyword).Count;
            }

            var response = ApiResponseHelper.StdApiResponse("Data load success", licenses, datacount: count);

            return Ok(response);
        }

        [HttpGet("{licenseId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<License>))]
        [ProducesResponseType(400)]
        public IActionResult GetLicense(int licenseId)
        {
            if (!_licenserepository.LicenseExists(licenseId))
                return NotFound();

            var licenses = _mapper.Map<LicenseDto>(_licenserepository.GetLicense(licenseId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(licenses);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateLicense([FromBody] LicenseDto licenseCreate)
        {
            if (licenseCreate == null)
                return BadRequest(ModelState);

            var licenses = _licenserepository.GetLicenses_all()
            .Where(c => c.LicenseCode.Trim().ToUpper() == licenseCreate.LicenseCode.TrimEnd().ToUpper())
            .FirstOrDefault();

            if (licenses != null)
            {
                ModelState.AddModelError("", "License already exists");
                return StatusCode(409, ModelState);
            }

            var licensestemp = _licenserepository.GetPendingLicenses_all()
            .Where(c => (c.LicenseCode.Trim().ToUpper() == licenseCreate.LicenseCode.TrimEnd().ToUpper()) && c.Action == "C")
            .FirstOrDefault();

            if (licensestemp != null)
            {
                ModelState.AddModelError("", "License already exists. Need approve");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //COBA MASUKIN KE TEMP DAN LOG DAHULU 2023-12-07
            //var licenseMap = _mapper.Map<License>(licenseCreate);

            //if (!_licenserepository.CreateLicense(licenseMap))
            //{
            //    ModelState.AddModelError("", "Something went wrong while saving");
            //    return StatusCode(500, ModelState);
            //}
            //COBA MASUKIN KE TEMP DAN LOG DAHULU

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var licenseMapLogTemp = _mapper.Map<LicenseLogTemp>(licenseCreate);
                    licenseMapLogTemp.Action = "C";
                    licenseMapLogTemp.Status = "NA";
                    if (!_licenserepository.CreateLicenseLogTemp(licenseMapLogTemp))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    var licenseMapLog = _mapper.Map<LicenseLog>(licenseCreate);
                    licenseMapLog.LicenseTempId = licenseMapLogTemp.Id;
                    licenseMapLog.Action = "C";
                    licenseMapLog.Status = "NA";
                    if (!_licenserepository.CreateLicenseLog(licenseMapLog))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    dbContextTransaction.Commit();
                    return Ok("Successfully Added to Pending list. Waiting for approval");
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

        [HttpPut("{licenseId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateLicense(int licenseId, [FromBody] LicenseDto updatedLicense)
        {
            if (updatedLicense == null)
                return BadRequest(ModelState);

            if (licenseId != updatedLicense.ID)
                return BadRequest(ModelState);

            if (!_licenserepository.LicenseExists(licenseId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var licenses_ = _licenserepository.GetLicenses_all()
            .Where(c => (c.LicenseCode.Trim().ToUpper() == updatedLicense.LicenseCode.TrimEnd().ToUpper()) && c.Id != licenseId)
            .FirstOrDefault();

            if (licenses_ != null)
            {
                ModelState.AddModelError("", "License already exists");
                return StatusCode(409, ModelState);
            }

            var licensestemp_ = _licenserepository.GetPendingLicenses_all()
            .Where(c => (c.LicenseCode.Trim().ToUpper() == updatedLicense.LicenseCode.TrimEnd().ToUpper()) && c.LicenseId != licenseId)
            .FirstOrDefault();

            if (licensestemp_ != null)
            {
                ModelState.AddModelError("", "License already exists. Need approve");
                return StatusCode(409, ModelState);
            }

            var licensestemp = _licenserepository.GetPendingLicenses_all()
            .Where(c => c.LicenseId == licenseId && c.Action == "U")
            .FirstOrDefault();

            if (licensestemp != null)
            {
                ModelState.AddModelError("", "License already exists. Need approve");
                return StatusCode(409, ModelState);
            }

            //COBA MASUKIN KE TEMP DAN LOG DAHULU 2023-12-07
            //var licenseMap = _mapper.Map<License>(updatedLicense);

            ////if (!_licenserepository.UpdateLicense(licenseId, licenseMap))
            //if (!_licenserepository.UpdateLicense(licenseMap))
            //{
            //    ModelState.AddModelError("", "Something went wrong updating license");
            //    return StatusCode(500, ModelState);
            //}

            //return NoContent();
            //COBA MASUKIN KE TEMP DAN LOG DAHULU 2023-12-07

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var licenses = _licenserepository.GetLicense(licenseId);

                    var licenseMap = _mapper.Map<License>(licenses);
                    licenseMap.Action = "U";
                    licenseMap.Status = "NA";
                    if (!_licenserepository.UpdateLicense(licenseMap))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    var licenseMapLogTemp = _mapper.Map<LicenseLogTemp>(updatedLicense);
                    licenseMapLogTemp.Id = 0;
                    licenseMapLogTemp.LicenseId = licenseId;
                    licenseMapLogTemp.Action = "U";
                    licenseMapLogTemp.Status = "NA";
                    if (!_licenserepository.CreateLicenseLogTemp(licenseMapLogTemp))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log temp");
                        return StatusCode(500, ModelState);
                    }

                    var licenseMapLog = _mapper.Map<LicenseLog>(updatedLicense);
                    licenseMapLog.Id = 0;
                    licenseMapLog.LicenseId = licenseId;
                    licenseMapLog.LicenseTempId = licenseMapLogTemp.Id;
                    licenseMapLog.Action = "U";
                    licenseMapLog.Status = "NA";
                    if (!_licenserepository.CreateLicenseLog(licenseMapLog))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log");
                        return StatusCode(500, ModelState);
                    }

                    dbContextTransaction.Commit();
                    return Ok("Successfully Added to Pending list. Waiting for approval");
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

        [HttpDelete("{licenseId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteLicense(int licenseId)
        {
            if (!_licenserepository.LicenseExists(licenseId))
            {
                return NotFound();
            }

            var licensestemp = _licenserepository.GetPendingLicenses_all()
            .Where(c => c.LicenseId == licenseId && c.Action == "D")
            .FirstOrDefault();

            if (licensestemp != null)
            {
                ModelState.AddModelError("", "License already exists. Need approve");
                return StatusCode(409, ModelState);
            }

            //COBA MASUKIN KE TEMP DAN LOG DAHULU 2023-12-07
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            //if (!_licenserepository.DeleteLicense(licenseToDelete))
            //{
            //    ModelState.AddModelError("", "Something went wrong deleting license");
            //}

            //return NoContent();
            //COBA MASUKIN KE TEMP DAN LOG DAHULU 2023-12-07

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var licenses = _licenserepository.GetLicenses(licenseId);

                    var licenseMap = _mapper.Map<License>(licenses);
                    licenseMap.Action = "D";
                    licenseMap.Status = "NA";
                    if (!_licenserepository.UpdateLicense(licenseMap))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    var licenseMapLogTemp = _mapper.Map<LicenseLogTemp>(licenseMap);
                    licenseMapLogTemp.Id = 0;
                    licenseMapLogTemp.LicenseId = licenseId;
                    licenseMapLogTemp.Action = "D";
                    licenseMapLogTemp.Status = "NA";
                    licenseMapLogTemp.Remarks = "";
                    if (!_licenserepository.CreateLicenseLogTemp(licenseMapLogTemp))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log temp");
                        return StatusCode(500, ModelState);
                    }

                    var licenseMapLog = _mapper.Map<LicenseLog>(licenseMap);
                    licenseMapLog.Id = 0;
                    licenseMapLog.LicenseId = licenseId;
                    licenseMapLog.LicenseTempId = licenseMapLogTemp.Id;
                    licenseMapLog.Action = "D";
                    licenseMapLog.Status = "NA";
                    licenseMapLog.Remarks = "";
                    if (!_licenserepository.CreateLicenseLog(licenseMapLog))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log");
                        return StatusCode(500, ModelState);
                    }

                    dbContextTransaction.Commit();
                    return Ok("Successfully Added to Pending list. Waiting for approval");
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

        [HttpDelete("deleteLicenseTemp/{pendingId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteLicenseTemp(int pendingId, [FromBody] ActionRemark Actremark)
        {
            if (!_licenserepository.LicenseTempExists(pendingId))
            {
                return NotFound();
            }

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var licenseLogTempToDelete = _licenserepository.GetLicenseLogTemp(pendingId);
                    var response = "";

                    var licenseMapLog = _mapper.Map<LicenseLog>(licenseLogTempToDelete);
                    licenseMapLog.Id = 0;
                    licenseMapLog.LicenseTempId = licenseLogTempToDelete.Id;
                    //licenseMapLog.Action = licenseLogTempToDelete.Action;
                    licenseMapLog.ActionRemarks = Actremark.ActionRemarks;
                    if (!_licenserepository.CreateLicenseLog(licenseMapLog))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log");
                        return StatusCode(500, ModelState);
                    }

                    if (_licenserepository.LicenseExists(licenseLogTempToDelete.LicenseId))
                    {
                        var licenses = _licenserepository.GetLicenses(licenseLogTempToDelete.LicenseId);

                        var licenseMap = _mapper.Map<License>(licenses);
                        licenseMap.Action = "N";
                        licenseMap.Status = "A";
                        if (!_licenserepository.UpdateLicense(licenseMap))
                        {
                            ModelState.AddModelError("", "Something went wrong while saving");
                            return StatusCode(500, ModelState);
                        }
                    }

                    //DELETE TEMP
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);

                    if (!_licenserepository.DeleteLicenseLogTemp(licenseLogTempToDelete))
                    {
                        ModelState.AddModelError("", "Something went wrong deleting license temp");
                        return StatusCode(500, ModelState);
                    }
                    //DELETE TEMP

                    dbContextTransaction.Commit();
                    response = "Successfully Deleted Pending License";
                    return Ok(response);
                    //return NoContent();
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

        [HttpPut("editLicenseTemp/{pendingId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult updateLicenseTemp(int pendingId, [FromBody] LicenseDtoPendingUpdate updateLicenseLogTemp)
        {
            if (!_licenserepository.LicenseTempExists(pendingId))
            {
                return NotFound();
            }

            var licenses_ = _licenserepository.GetLicenses_all()
            .Where(c => (c.LicenseCode.Trim().ToUpper() == updateLicenseLogTemp.LicenseCode.TrimEnd().ToUpper()) && c.Id != updateLicenseLogTemp.ID)
            .FirstOrDefault();

            if (licenses_ != null)
            {
                ModelState.AddModelError("", "License already exists");
                return StatusCode(409, ModelState);
            }

            var licensestemp_ = _licenserepository.GetPendingLicenses_all()
            .Where(c => (c.LicenseCode.Trim().ToUpper() == updateLicenseLogTemp.LicenseCode.TrimEnd().ToUpper()) && c.Id != pendingId)
            .FirstOrDefault();

            if (licensestemp_ != null)
            {
                ModelState.AddModelError("", "License already exists. Need approve");
                return StatusCode(409, ModelState);
            }

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var licenseLogTempToEdit = _licenserepository.GetLicenseLogTemp(pendingId);
                    var response = "";

                    var licenseMap = _mapper.Map<LicenseLogTemp>(licenseLogTempToEdit);
                    licenseMap.LicenseCode = updateLicenseLogTemp.LicenseCode;
                    licenseMap.UpdatedBy = updateLicenseLogTemp.UpdatedBy;
                    licenseMap.Remarks = updateLicenseLogTemp.Remarks;
                    licenseMap.ActionRemarks = updateLicenseLogTemp.ActionRemarks;
                    licenseMap.Status = "NA";
                    if (!_licenserepository.UpdateLicenseTemp(licenseMap))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    var licenseMapLog = _mapper.Map<LicenseLog>(licenseLogTempToEdit);
                    licenseMapLog.Id = 0;
                    licenseMapLog.LicenseTempId = licenseLogTempToEdit.Id;
                    licenseMapLog.Action = licenseLogTempToEdit.Action;
                    licenseMapLog.Status = "NA";
                    licenseMapLog.ActionRemarks = updateLicenseLogTemp.ActionRemarks;
                    if (!_licenserepository.CreateLicenseLog(licenseMapLog))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log");
                        return StatusCode(500, ModelState);
                    }

                    dbContextTransaction.Commit();
                    response = "Successfully Update Pending License";
                    return Ok(response);
                    //return NoContent();
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

        [HttpPost("approvalLicense/")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult ApprovalLicenses([FromBody] StatusIdList statusIdList)
        {
            var response = "";
            int total_data_failed = 0;
            int total_data_success = 0;

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var auth_id_hash = _authrepository.Getid2(statusIdList.UserName);

                    if (auth_id_hash == null)
                    {
                        //throw new Exception("username or password invalid");
                        ModelState.AddModelError("", "username or password invalid");
                        return StatusCode(401, ModelState);
                    }

                    var result = _passwordHasher.Verify(auth_id_hash.Password, statusIdList.Password);

                    if (!result)
                    {
                        //throw new Exception("username or password invalid");
                        ModelState.AddModelError("", "username or password invalid");
                        return StatusCode(401, ModelState);
                    }

                    // Process the list of IDs (for demonstration, just echoing back)
                    if (statusIdList.Approval == "A")
                    {
                        foreach (var id in statusIdList.IdList)
                        {
                            int _flag = 0;

                            if (_licenserepository.LicenseTempExists(Convert.ToInt32(id)))
                            {
                                var licenseLogTempToDelete = _licenserepository.GetLicenseLogTemp(Convert.ToInt32(id));
                                if (licenseLogTempToDelete != null)
                                {
                                    if (licenseLogTempToDelete.Action == "C" || licenseLogTempToDelete.Action == "U" || licenseLogTempToDelete.Action == "D")
                                    {
                                        //return StatusCode(500, licenseLogTempToDelete.Action);
                                        var updatedLicense = new LicenseDto();

                                        if (licenseLogTempToDelete.Action == "C")
                                        {
                                            //var updatedLicense = new LicenseDto();
                                            var licenseMap = _mapper.Map<License>(updatedLicense);

                                            licenseMap.LicenseCode = licenseLogTempToDelete.LicenseCode;
                                            licenseMap.UpdatedBy = licenseLogTempToDelete.UpdatedBy;
                                            licenseMap.Status = statusIdList.Approval;
                                            licenseMap.Action = "N";
                                            licenseMap.Remarks = licenseLogTempToDelete.Remarks;

                                            if (!_licenserepository.CreateLicense(licenseMap))
                                            {
                                                ModelState.AddModelError("", "Something went wrong while saving");
                                                return StatusCode(500, ModelState);
                                            }

                                            var licenseMapLog = _mapper.Map<LicenseLog>(updatedLicense);
                                            licenseMapLog.LicenseTempId = licenseLogTempToDelete.Id;
                                            licenseMapLog.LicenseId = licenseMap.Id;
                                            licenseMapLog.LicenseCode = licenseLogTempToDelete.LicenseCode;
                                            licenseMapLog.UpdatedBy = licenseLogTempToDelete.UpdatedBy;
                                            licenseMapLog.Status = statusIdList.Approval;
                                            licenseMapLog.Action = licenseLogTempToDelete.Action;
                                            licenseMapLog.Remarks = licenseLogTempToDelete.Remarks;
                                            licenseMapLog.ActionRemarks = statusIdList.ActionRemarks;
                                            if (!_licenserepository.CreateLicenseLog(licenseMapLog))
                                            {
                                                ModelState.AddModelError("", "Something went wrong while saving");
                                                return StatusCode(500, ModelState);
                                            }

                                            response = "Successfully Approve";

                                            _flag = 1;
                                        }
                                        else if (licenseLogTempToDelete.Action == "U")
                                        {
                                            //var updatedLicense = new LicenseDto();
                                            var licenseMap = _mapper.Map<License>(updatedLicense);

                                            licenseMap.Id = licenseLogTempToDelete.LicenseId;
                                            licenseMap.LicenseCode = licenseLogTempToDelete.LicenseCode;
                                            licenseMap.UpdatedBy = licenseLogTempToDelete.UpdatedBy;
                                            licenseMap.Status = statusIdList.Approval;
                                            licenseMap.Action = "N";
                                            licenseMap.Remarks = licenseLogTempToDelete.Remarks;

                                            if (!_licenserepository.UpdateLicense(licenseMap))
                                            {
                                                ModelState.AddModelError("", "Something went wrong updating license");
                                                return StatusCode(500, ModelState);
                                            }

                                            var licenseMapLog = _mapper.Map<LicenseLog>(updatedLicense);
                                            licenseMapLog.LicenseTempId = licenseLogTempToDelete.Id;
                                            licenseMapLog.LicenseId = licenseMap.Id;
                                            licenseMapLog.LicenseCode = licenseLogTempToDelete.LicenseCode;
                                            licenseMapLog.UpdatedBy = licenseLogTempToDelete.UpdatedBy;
                                            licenseMapLog.Status = statusIdList.Approval;
                                            licenseMapLog.Action = licenseLogTempToDelete.Action;
                                            licenseMapLog.Remarks = licenseLogTempToDelete.Remarks;
                                            licenseMapLog.ActionRemarks = statusIdList.ActionRemarks;
                                            if (!_licenserepository.CreateLicenseLog(licenseMapLog))
                                            {
                                                ModelState.AddModelError("", "Something went wrong while saving");
                                                return StatusCode(500, ModelState);
                                            }

                                            response = "Successfully Approve";

                                            _flag = 1;
                                        }
                                        else if (licenseLogTempToDelete.Action == "D")
                                        {
                                            var getlicenseToDelete = _licenserepository.GetLicense(licenseLogTempToDelete.LicenseId);
                                            //var updatedLicense = new LicenseDto();

                                            if (!_licenserepository.DeleteLicense(getlicenseToDelete))
                                            {
                                                ModelState.AddModelError("", "Something went wrong deleting license");
                                            }

                                            var licenseMapLog = _mapper.Map<LicenseLog>(updatedLicense);
                                            licenseMapLog.LicenseTempId = licenseLogTempToDelete.Id;
                                            licenseMapLog.LicenseId = getlicenseToDelete.Id;
                                            licenseMapLog.LicenseCode = licenseLogTempToDelete.LicenseCode;
                                            licenseMapLog.UpdatedBy = licenseLogTempToDelete.UpdatedBy;
                                            licenseMapLog.Status = statusIdList.Approval;
                                            licenseMapLog.Action = licenseLogTempToDelete.Action;
                                            licenseMapLog.Remarks = licenseLogTempToDelete.Remarks;
                                            licenseMapLog.ActionRemarks = statusIdList.ActionRemarks;
                                            if (!_licenserepository.CreateLicenseLog(licenseMapLog))
                                            {
                                                ModelState.AddModelError("", "Something went wrong while saving");
                                                return StatusCode(500, ModelState);
                                            }

                                            response = "Successfully Approve ";

                                            _flag = 1;
                                        }
                                    }

                                    if (_flag == 1)
                                    {
                                        if (!ModelState.IsValid)
                                            return BadRequest(ModelState);

                                        if (!_licenserepository.DeleteLicenseLogTemp(licenseLogTempToDelete))
                                        {
                                            ModelState.AddModelError("", "Something went wrong deleting license");
                                        }
                                    }
                                }
                                total_data_success = total_data_success + 1;
                            }
                            else
                            {
                                total_data_failed = total_data_failed + 1;
                            }
                        }
                    }
                    else if (statusIdList.Approval == "R")
                    {
                        foreach (var id in statusIdList.IdList)
                        {
                            int _flag = 0;
                            if (_licenserepository.LicenseTempExists(Convert.ToInt32(id)))
                            {
                                var licenseLogTempToDelete = _licenserepository.GetLicenseLogTemp(Convert.ToInt32(id));
                                if (licenseLogTempToDelete.Action == "C" || licenseLogTempToDelete.Action == "U" || licenseLogTempToDelete.Action == "D")
                                {
                                    var licenseMapTemp = _mapper.Map<LicenseLogTemp>(licenseLogTempToDelete);
                                    #region
                                    //licenseMap.Id = Convert.ToInt32(id);
                                    //licenseMap.LicenseId = 0;
                                    //licenseMap.LicenseCode = licenseLogTempToDelete.LicenseCode;
                                    //licenseMap.UpdatedBy = licenseLogTempToDelete.UpdatedBy;
                                    #endregion
                                    licenseMapTemp.Action = licenseLogTempToDelete.Action;
                                    licenseMapTemp.Status = statusIdList.Approval;
                                    licenseMapTemp.ActionRemarks = statusIdList.ActionRemarks;
                                    if (!_licenserepository.UpdateLicenseTemp(licenseMapTemp))
                                    {
                                        ModelState.AddModelError("", "Something went wrong while saving");
                                        return StatusCode(500, ModelState);
                                    }

                                    #region
                                    //var licenseMapLog = _mapper.Map<LicenseLog>(updatedLicense);
                                    //licenseMapLog.LicenseTempId = licenseLogTempToDelete.Id;
                                    //licenseMapLog.LicenseId = 0;
                                    //licenseMapLog.LicenseCode = licenseLogTempToDelete.LicenseCode;
                                    //licenseMapLog.UpdatedBy = licenseLogTempToDelete.UpdatedBy;
                                    //licenseMapLog.Status = statusIdList.Approval;
                                    //licenseMapLog.Action = licenseLogTempToDelete.Action;
                                    //licenseMapLog.ActionRemarks = "Rejected2";
                                    //licenseMapLog.Remarks = licenseLogTempToDelete.Remarks;
                                    #endregion

                                    var licenseMapLog = _mapper.Map<LicenseLog>(licenseLogTempToDelete);
                                    licenseMapLog.Id = 0;
                                    licenseMapLog.LicenseTempId = licenseLogTempToDelete.Id;
                                    licenseMapLog.Status = statusIdList.Approval;
                                    licenseMapLog.Action = licenseLogTempToDelete.Action;
                                    licenseMapLog.ActionRemarks = statusIdList.ActionRemarks;

                                    if (!_licenserepository.CreateLicenseLog(licenseMapLog))
                                    {
                                        ModelState.AddModelError("", "Something went wrong while saving");
                                        return StatusCode(500, ModelState);
                                    }

                                    response = "Successfully Rejected";

                                    _flag = 0;
                                    #region
                                    //    var updatedLicense = new LicenseDto();

                                    //    if (licenseLogTempToDelete.Action == "C")
                                    //    {
                                    //        var licenseMapTemp = _mapper.Map<LicenseLogTemp>(licenseLogTempToDelete);
                                    //        #region
                                    //        //licenseMap.Id = Convert.ToInt32(id);
                                    //        //licenseMap.LicenseId = 0;
                                    //        //licenseMap.LicenseCode = licenseLogTempToDelete.LicenseCode;
                                    //        //licenseMap.UpdatedBy = licenseLogTempToDelete.UpdatedBy;
                                    //        #endregion
                                    //        licenseMapTemp.Action = licenseLogTempToDelete.Action;
                                    //        licenseMapTemp.Status = statusIdList.Approval;
                                    //        licenseMapTemp.ActionRemarks = statusIdList.ActionRemarks;
                                    //        if (!_licenserepository.UpdateLicenseTemp(licenseMapTemp))
                                    //        {
                                    //            ModelState.AddModelError("", "Something went wrong while saving");
                                    //            return StatusCode(500, ModelState);
                                    //        }

                                    //        #region
                                    //        //var licenseMapLog = _mapper.Map<LicenseLog>(updatedLicense);
                                    //        //licenseMapLog.LicenseTempId = licenseLogTempToDelete.Id;
                                    //        //licenseMapLog.LicenseId = 0;
                                    //        //licenseMapLog.LicenseCode = licenseLogTempToDelete.LicenseCode;
                                    //        //licenseMapLog.UpdatedBy = licenseLogTempToDelete.UpdatedBy;
                                    //        //licenseMapLog.Status = statusIdList.Approval;
                                    //        //licenseMapLog.Action = licenseLogTempToDelete.Action;
                                    //        //licenseMapLog.ActionRemarks = "Rejected2";
                                    //        //licenseMapLog.Remarks = licenseLogTempToDelete.Remarks;
                                    //        #endregion

                                    //        var licenseMapLog = _mapper.Map<LicenseLog>(licenseLogTempToDelete);
                                    //        licenseMapLog.Id = 0;
                                    //        licenseMapLog.LicenseTempId = licenseLogTempToDelete.Id;
                                    //        licenseMapLog.Status = statusIdList.Approval;
                                    //        licenseMapLog.Action = licenseLogTempToDelete.Action;
                                    //        licenseMapLog.ActionRemarks = statusIdList.ActionRemarks;

                                    //        if (!_licenserepository.CreateLicenseLog(licenseMapLog))
                                    //        {
                                    //            ModelState.AddModelError("", "Something went wrong while saving");
                                    //            return StatusCode(500, ModelState);
                                    //        }

                                    //        response = "Successfully Rejected";

                                    //        _flag = 0;
                                    //    }
                                    //    else if (licenseLogTempToDelete.Action == "U")
                                    //    {
                                    //        #region
                                    //        //REJECT TIDAK LANGSUNG PENGARUH KE DATA UTAMA 2023-12-20
                                    //        //var licenseMap = _mapper.Map<License>(updatedLicense);

                                    //        //licenseMap.Id = licenseLogTempToDelete.LicenseId;
                                    //        //licenseMap.LicenseCode = licenseLogTempToDelete.LicenseCode;
                                    //        //licenseMap.UpdatedBy = licenseLogTempToDelete.UpdatedBy;
                                    //        //licenseMap.Status = "A";
                                    //        //licenseMap.Action = "N";

                                    //        //if (!_licenserepository.UpdateLicense(licenseMap))
                                    //        //{
                                    //        //    ModelState.AddModelError("", "Something went wrong updating license");
                                    //        //    return StatusCode(500, ModelState);
                                    //        //}
                                    //        //REJECT TIDAK LANGSUNG PENGARUH KE DATA UTAMA 2023-12-20
                                    //        #endregion

                                    //        var licenseMapTemp = _mapper.Map<LicenseLogTemp>(licenseLogTempToDelete);
                                    //        licenseMapTemp.Action = licenseLogTempToDelete.Action;
                                    //        licenseMapTemp.Status = statusIdList.Approval;
                                    //        licenseMapTemp.ActionRemarks = statusIdList.ActionRemarks;
                                    //        if (!_licenserepository.UpdateLicenseTemp(licenseMapTemp))
                                    //        {
                                    //            ModelState.AddModelError("", "Something went wrong while saving");
                                    //            return StatusCode(500, ModelState);
                                    //        }

                                    //        #region
                                    //        //var licenseMapLog = _mapper.Map<LicenseLog>(updatedLicense);
                                    //        //licenseMapLog.LicenseTempId = licenseLogTempToDelete.Id;
                                    //        //licenseMapLog.LicenseId = licenseLogTempToDelete.LicenseId;
                                    //        //licenseMapLog.LicenseCode = licenseLogTempToDelete.LicenseCode;
                                    //        //licenseMapLog.UpdatedBy = licenseLogTempToDelete.UpdatedBy;
                                    //        //licenseMapLog.Status = statusIdList.Approval;
                                    //        //licenseMapLog.Action = licenseLogTempToDelete.Action;
                                    //        //_licenserepository.CreateLicenseLog(licenseMapLog);
                                    //        #endregion

                                    //        var licenseMapLog = _mapper.Map<LicenseLog>(licenseLogTempToDelete);
                                    //        licenseMapLog.Id = 0;
                                    //        licenseMapLog.LicenseTempId = licenseLogTempToDelete.Id;
                                    //        licenseMapLog.Status = statusIdList.Approval;
                                    //        licenseMapLog.Action = licenseLogTempToDelete.Action;
                                    //        licenseMapLog.ActionRemarks = statusIdList.ActionRemarks;

                                    //        if (!_licenserepository.CreateLicenseLog(licenseMapLog))
                                    //        {
                                    //            ModelState.AddModelError("", "Something went wrong while saving");
                                    //            return StatusCode(500, ModelState);
                                    //        }

                                    //        response = "Successfully Rejected";

                                    //        _flag = 0;
                                    //    }
                                    //    else if (licenseLogTempToDelete.Action == "D")
                                    //    {
                                    //        #region
                                    //        //var licenseMap = _mapper.Map<License>(updatedLicense);

                                    //        //licenseMap.Id = licenseLogTempToDelete.LicenseId;
                                    //        //licenseMap.LicenseCode = licenseLogTempToDelete.LicenseCode;
                                    //        //licenseMap.UpdatedBy = licenseLogTempToDelete.UpdatedBy;
                                    //        //licenseMap.Status = "A";
                                    //        //licenseMap.Action = "N";

                                    //        //if (!_licenserepository.UpdateLicense(licenseMap))
                                    //        //{
                                    //        //    ModelState.AddModelError("", "Something went wrong updating license");
                                    //        //    return StatusCode(500, ModelState);
                                    //        //}
                                    //        #endregion
                                    //        var licenseMapTemp = _mapper.Map<LicenseLogTemp>(licenseLogTempToDelete);
                                    //        licenseMapTemp.Action = licenseLogTempToDelete.Action;
                                    //        licenseMapTemp.Status = statusIdList.Approval;
                                    //        licenseMapTemp.ActionRemarks = statusIdList.ActionRemarks;
                                    //        if (!_licenserepository.UpdateLicenseTemp(licenseMapTemp))
                                    //        {
                                    //            ModelState.AddModelError("", "Something went wrong while saving");
                                    //            return StatusCode(500, ModelState);
                                    //        }
                                    //        #region
                                    //        //var licenseMapLog = _mapper.Map<LicenseLog>(updatedLicense);
                                    //        //licenseMapLog.LicenseTempId = licenseLogTempToDelete.Id;
                                    //        //licenseMapLog.LicenseId = licenseLogTempToDelete.LicenseId;
                                    //        //licenseMapLog.LicenseCode = licenseLogTempToDelete.LicenseCode;
                                    //        //licenseMapLog.UpdatedBy = licenseLogTempToDelete.UpdatedBy;
                                    //        //licenseMapLog.Status = statusIdList.Approval;
                                    //        //licenseMapLog.Action = licenseLogTempToDelete.Action;
                                    //        //_licenserepository.CreateLicenseLog(licenseMapLog);
                                    //        #endregion
                                    //        var licenseMapLog = _mapper.Map<LicenseLog>(licenseLogTempToDelete);
                                    //        licenseMapLog.Id = 0;
                                    //        licenseMapLog.LicenseTempId = licenseLogTempToDelete.Id;
                                    //        licenseMapLog.Status = statusIdList.Approval;
                                    //        licenseMapLog.Action = licenseLogTempToDelete.Action;
                                    //        licenseMapLog.ActionRemarks = statusIdList.ActionRemarks;

                                    //        if (!_licenserepository.CreateLicenseLog(licenseMapLog))
                                    //        {
                                    //            ModelState.AddModelError("", "Something went wrong while saving");
                                    //            return StatusCode(500, ModelState);
                                    //        }

                                    //        response = "Successfully Rejected";

                                    //        _flag = 0;
                                    //    }
                                    #endregion
                                }

                                if (_flag == 1)
                                {
                                    if (!ModelState.IsValid)
                                        return BadRequest(ModelState);

                                    if (!_licenserepository.DeleteLicenseLogTemp(licenseLogTempToDelete))
                                    {
                                        ModelState.AddModelError("", "Something went wrong deleting license");
                                    }
                                }
                                total_data_success = total_data_success + 1;
                            }
                            else
                            {
                                total_data_failed = total_data_failed + 1;
                            }
                        }
                    }

                    dbContextTransaction.Commit();
                    response = response + ". Data success : " + total_data_success + ". Data failed : " + total_data_failed + ".";
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

        [HttpGet("PendingLicense/")]
        //[Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<License>))]
        public IActionResult GetPendingLicensees([FromQuery] int limit, [FromQuery] int page, [FromQuery] string sortby, [FromQuery] string sortdesc, [FromQuery] string? keyword)
        {
            var pendinglicenses = _mapper.Map<List<LicenseDtoPending>>(_licenserepository.GetPendingLicenses(limit, page, sortby, sortdesc, keyword));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var count = pendinglicenses.Count();

            var count = 0;

            if (keyword == "")
            {
                count = _licenserepository.Getpendingdatacount();
            }
            else
            {
                count = _licenserepository.GetLicensesAdvancePending(keyword).Count;
            }

            var response = ApiResponseHelper.StdApiResponse("Data load success", pendinglicenses, datacount: count);

            return Ok(response);
            //return CustomResult("Data load success", licenses);
        }

        [HttpGet("PendingLicense/{pendingId}")]
        //[Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<License>))]
        public IActionResult GetPendingLicense(int pendingId)
        {
            if (!_licenserepository.LicenseTempExists(pendingId))
            {
                return NotFound();
            }

            var licenses = _mapper.Map<LicenseDtoPending>(_licenserepository.GetLicenseLogTemp(pendingId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(licenses);
        }
    }
}
