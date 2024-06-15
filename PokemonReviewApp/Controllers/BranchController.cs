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

namespace API_Dinamis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : BaseController
    {
        private readonly IBranchRepository _branchrepository;
        private readonly IMapper _mapper;
        private readonly IAuthRepository _authrepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ICityRepository _cityrepository;
        private readonly IZipCodeRepository _zipcoderepository;
        private readonly DataContext _context;
        public BranchController(IBranchRepository branchRepository, IMapper mapper, IAuthRepository authrepository, IPasswordHasher passwordHasher, ICityRepository cityrepository, IZipCodeRepository zipcoderepository, DataContext context)
        {
            _branchrepository = branchRepository;
            _mapper = mapper;
            _authrepository = authrepository;
            _passwordHasher = passwordHasher;
            _cityrepository = cityrepository;
            _zipcoderepository = zipcoderepository;
            _context = context;
        }

        [HttpGet("getbranches2/")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Branch>))]
        public IActionResult GetBranch2([FromQuery] int? limit=0, [FromQuery] int? page=1, [FromQuery] string? sortby="d", [FromQuery] string? sortdesc="id", [FromQuery] string? keyword="")
        {
            var branches = _mapper.Map<List<BranchDtoForm>>(_branchrepository.GetBranches2(limit, page, sortby, sortdesc, keyword));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var count = 0;

            if (keyword == "")
            {
                count = _branchrepository.Getdatacount();
            }
            else
            {
                count = _branchrepository.GetBranchesAdvance(keyword).Count;
            }

            var response = ApiResponseHelper.StdApiResponse("Data load success", branches, datacount: count);

            return Ok(response);
        }

        [HttpGet]
        //[Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Branch>))]
        public IActionResult GetBranch([FromQuery] int limit, [FromQuery] int page, [FromQuery] string sortby, [FromQuery] string sortdesc, [FromQuery] string? keyword)
        {
            var branches = _mapper.Map<List<BranchDtoForm>>(_branchrepository.GetBranches(limit, page, sortby, sortdesc, keyword));
            //var branches = _mapper.Map<List<BranchDto>>(_branchrepository.GetBranches(limit, page, sortby, sortdesc, keyword));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var count = 0;

            if (keyword == "")
            {
                count = _branchrepository.Getdatacount();
            }
            else
            {
                count = _branchrepository.GetBranchesAdvance(keyword).Count;
            }

            var response = ApiResponseHelper.StdApiResponse("Data load success", branches, datacount: count);

            return Ok(response);
            //return CustomResult("Data load success", branches);
        }

        [HttpGet("{branchId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Branch>))]
        [ProducesResponseType(400)]
        public IActionResult GetBranch(int branchId)
        {
            if (!_branchrepository.BranchExists(branchId))
                return NotFound();

            //var branches = _mapper.Map<BranchDto>(_branchrepository.GetBranch(branchId));
            var branches = _mapper.Map<BranchDtoListTableJoin>(_branchrepository.BranchDtoTableJoin(branchId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(branches);
        }

        [HttpGet("ListTableJoin/")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Branch>))]
        [ProducesResponseType(400)]
        public IActionResult ListTableJoin()
        {
            var branchess = _mapper.Map<List<BranchDtoListTableJoin>>(_branchrepository.BranchDtoListTableJoin_all());

            return Ok(branchess);
        }

        [HttpGet("TableJoin/{branchid}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Branch>))]
        [ProducesResponseType(400)]
        public IActionResult TableJoin(int branchid)
        {
            var branchess = _mapper.Map<BranchDtoListTableJoin>(_branchrepository.BranchDtoTableJoin(branchid));

            return Ok(branchess);
        }

        //[HttpPost("CreateBranchTest")]
        //[ProducesResponseType(204)]
        //[ProducesResponseType(400)]
        //public IActionResult CreateBranchTest([FromBody] BranchDto branchCreate)
        //{

        //    //COBA MASUKIN KE TEMP DAN LOG DAHULU 2023-12-07
        //    var branchMap = _mapper.Map<Branch>(branchCreate);

        //    if (!_branchrepository.CreateBranchTest(branchMap))
        //    {
        //        ModelState.AddModelError("", "Something went wrong while saving");
        //        return StatusCode(500, ModelState);
        //    }
        //    //COBA MASUKIN KE TEMP DAN LOG DAHULU

        //    return Ok("Successfully Added to Pending list. Waiting for approval");
        //}

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateBranch([FromBody] BranchDto branchCreate)
        {
            if (branchCreate == null)
                return BadRequest(ModelState);

            var branches = _branchrepository.GetBranches_all()
            .Where(c => c.BranchName.Trim().ToUpper() == branchCreate.BranchName.TrimEnd().ToUpper() || c.BranchCode.TrimEnd().ToUpper() == branchCreate.BranchCode.TrimEnd().ToUpper())
            .FirstOrDefault();

            if (branches != null)
            {
                ModelState.AddModelError("", "Branch already exists");
                return StatusCode(409, ModelState);
            }

            var branchestemp = _branchrepository.GetPendingBranches_all()
            .Where(c => (c.BranchName.Trim().ToUpper() == branchCreate.BranchName.TrimEnd().ToUpper() || c.BranchCode.TrimEnd().ToUpper() == branchCreate.BranchCode.TrimEnd().ToUpper()) && c.Action == "C")
            .FirstOrDefault();

            if (branchestemp != null)
            {
                ModelState.AddModelError("", "Branch already exists. Need approve");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //COBA MASUKIN KE TEMP DAN LOG DAHULU 2023-12-07
            //var branchMap = _mapper.Map<Branch>(branchCreate);

            //if (!_branchrepository.CreateBranch(branchMap))
            //{
            //    ModelState.AddModelError("", "Something went wrong while saving");
            //    return StatusCode(500, ModelState);
            //}
            //COBA MASUKIN KE TEMP DAN LOG DAHULU

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var branchMapLogTemp = _mapper.Map<BranchLogTemp>(branchCreate);
                    branchMapLogTemp.Action = "C";
                    branchMapLogTemp.Status = "NA";
                    if (!_branchrepository.CreateBranchLogTemp(branchMapLogTemp))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    var branchMapLog = _mapper.Map<BranchLog>(branchCreate);
                    branchMapLog.BranchTempId = branchMapLogTemp.Id;
                    branchMapLog.Action = "C";
                    branchMapLog.Status = "NA";
                    _branchrepository.CreateBranchLog(branchMapLog);

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

        [HttpPut("{branchId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateBranch(int branchId, [FromBody] BranchDto updatedBranch)
        {
            if (updatedBranch == null)
                return BadRequest(ModelState);

            if (branchId != updatedBranch.ID)
                return BadRequest(ModelState);

            if (!_branchrepository.BranchExists(branchId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var branches_ = _branchrepository.GetBranches_all()
            .Where(c => (c.BranchName.Trim().ToUpper() == updatedBranch.BranchName.TrimEnd().ToUpper() || c.BranchCode.TrimEnd().ToUpper() == updatedBranch.BranchCode.TrimEnd().ToUpper()) && c.Id != branchId)
            .FirstOrDefault();

            if (branches_ != null)
            {
                ModelState.AddModelError("", "Branch already exists");
                return StatusCode(409, ModelState);
            }

            var branchestemp_ = _branchrepository.GetPendingBranches_all()
            .Where(c => (c.BranchName.Trim().ToUpper() == updatedBranch.BranchName.TrimEnd().ToUpper() || c.BranchCode.TrimEnd().ToUpper() == updatedBranch.BranchCode.TrimEnd().ToUpper()) && c.BranchId != branchId)
            .FirstOrDefault();

            if (branchestemp_ != null)
            {
                ModelState.AddModelError("", "Branch already exists. Need approve");
                return StatusCode(409, ModelState);
            }

            var branchestemp = _branchrepository.GetPendingBranches_all()
            .Where(c => c.BranchId == branchId && c.Action == "U")
            .FirstOrDefault();

            if (branchestemp != null)
            {
                ModelState.AddModelError("", "Branch already exists. Need approve");
                return StatusCode(409, ModelState);
            }

            //COBA MASUKIN KE TEMP DAN LOG DAHULU 2023-12-07
            //var branchMap = _mapper.Map<Branch>(updatedBranch);

            ////if (!_branchrepository.UpdateBranch(branchId, branchMap))
            //if (!_branchrepository.UpdateBranch(branchMap))
            //{
            //    ModelState.AddModelError("", "Something went wrong updating branch");
            //    return StatusCode(500, ModelState);
            //}

            //return NoContent();
            //COBA MASUKIN KE TEMP DAN LOG DAHULU 2023-12-07

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try 
                {
                    var branches = _branchrepository.GetBranch(branchId);

                    var branchMap = _mapper.Map<Branch>(branches);
                    branchMap.Action = "U";
                    branchMap.Status = "NA";
                    if (!_branchrepository.UpdateBranch(branchMap))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    var branchMapLogTemp = _mapper.Map<BranchLogTemp>(updatedBranch);
                    branchMapLogTemp.Id = 0;
                    branchMapLogTemp.BranchId = branchId;
                    branchMapLogTemp.Action = "U";
                    branchMapLogTemp.Status = "NA";
                    if (!_branchrepository.CreateBranchLogTemp(branchMapLogTemp))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log temp");
                        return StatusCode(500, ModelState);
                    }

                    var branchMapLog = _mapper.Map<BranchLog>(updatedBranch);
                    branchMapLog.Id = 0;
                    branchMapLog.BranchId = branchId;
                    branchMapLog.BranchTempId = branchMapLogTemp.Id;
                    branchMapLog.Action = "U";
                    branchMapLog.Status = "NA";
                    if (!_branchrepository.CreateBranchLog(branchMapLog))
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

        [HttpDelete("{branchId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteBranch(int branchId)
        {
            if (!_branchrepository.BranchExists(branchId))
            {
                return NotFound();
            }

            var branchestemp = _branchrepository.GetPendingBranches_all()
            .Where(c => c.BranchId == branchId && c.Action == "D")
            .FirstOrDefault();

            if (branchestemp != null)
            {
                ModelState.AddModelError("", "Branch already exists. Need approve");
                return StatusCode(409, ModelState);
            }

            //COBA MASUKIN KE TEMP DAN LOG DAHULU 2023-12-07
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            //if (!_branchrepository.DeleteBranch(branchToDelete))
            //{
            //    ModelState.AddModelError("", "Something went wrong deleting branch");
            //}

            //return NoContent();
            //COBA MASUKIN KE TEMP DAN LOG DAHULU 2023-12-07

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var branches = _branchrepository.GetBranch(branchId);

                    var branchMap = _mapper.Map<Branch>(branches);
                    branchMap.Action = "D";
                    branchMap.Status = "NA";
                    if (!_branchrepository.UpdateBranch(branchMap))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    ////var getbranchToDelete = _branchrepository.GetBranch(branchId);
                    //var _Branch = new BranchDto();
                    //_Branch.ID = branches.Id;
                    //_Branch.BranchName = branches.BranchName;
                    //_Branch.BranchCode = branches.BranchCode;
                    //_Branch.UpdatedBy = branches.UpdatedBy;
                    //_Branch.Address1 = branches.Address1;
                    //_Branch.Address2 = branches.Address2;
                    //_Branch.ZipCodeId = branches.ZipCodeId;
                    //_Branch.ContactPerson = branches.ContactPerson;
                    //_Branch.Fax = branches.Fax;
                    //_Branch.Phone = branches.Phone;
                    //_Branch.Remarks = branches.Remarks;
                    //_Branch.ZipCodeId = branches.ZipCodeId;
                    ////_Branch.Status = "NA";

                    var branchMapLogTemp = _mapper.Map<BranchLogTemp>(branchMap);
                    branchMapLogTemp.Id = 0;
                    branchMapLogTemp.BranchId = branchId;
                    branchMapLogTemp.Action = "D";
                    branchMapLogTemp.Status = "NA";
                    branchMapLogTemp.Remarks = "";
                    if (!_branchrepository.CreateBranchLogTemp(branchMapLogTemp))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log temp");
                        return StatusCode(500, ModelState);
                    }

                    var branchMapLog = _mapper.Map<BranchLog>(branchMap);
                    branchMapLog.Id = 0;
                    branchMapLog.BranchId = branchId;
                    branchMapLog.BranchTempId = branchMapLogTemp.Id;
                    branchMapLog.Action = "D";
                    branchMapLog.Status = "NA";
                    branchMapLog.Remarks = "";
                    //_branchrepository.CreateBranchLog(branchMapLog);
                    if (!_branchrepository.CreateBranchLog(branchMapLog))
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

        //[HttpPost("searchBranch/")]
        //[ProducesResponseType(204)]
        //[ProducesResponseType(400)]
        //public IActionResult GetBranchesAdvance([FromBody] searchBranchDto SearchBranch)
        //{
        //    //if (!_branchrepository.BranchExists(SearchBranch.ID))
        //    //    return NotFound();

        //    //var branches = _mapper.Map<List<BranchDto>>(_branchrepository.GetBranches());
        //    //var branchMap = _mapper.Map<Branch>(branchCreate);
        //    var branches = _mapper.Map<List<BranchDto>>(_branchrepository.GetBranchesAdvance(SearchBranch.keyword));

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    return Ok(branches);
        //}

        //[HttpPost("searchBranch/")]
        //[ProducesResponseType(204)]
        //[ProducesResponseType(400)]
        //public IActionResult GetBranchesAdvancePending([FromBody] searchBranchDto SearchBranch)
        //{
        //    var branches = _mapper.Map<List<BranchDto>>(_branchrepository.GetBranchesAdvancePending(SearchBranch.keyword));

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    return Ok(branches);
        //}

        [HttpDelete("deleteBranchTemp/{pendingId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteBranchTemp(int pendingId, [FromBody] ActionRemark Actremark)
        {
            if (!_branchrepository.BranchTempExists(pendingId))
            {
                return NotFound();
            }

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var branchLogTempToDelete = _branchrepository.GetBranchLogTemp(pendingId);
                    var response = "";

                    var branchMapLog = _mapper.Map<BranchLog>(branchLogTempToDelete);
                    branchMapLog.Id = 0;
                    branchMapLog.BranchTempId = branchLogTempToDelete.Id;
                    //branchMapLog.Action = branchLogTempToDelete.Action;
                    branchMapLog.ActionRemarks = Actremark.ActionRemarks;
                    if (!_branchrepository.CreateBranchLog(branchMapLog))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log");
                        return StatusCode(500, ModelState);
                    }

                    if (_branchrepository.BranchExists(branchLogTempToDelete.BranchId))
                    {
                        var branches = _branchrepository.GetBranch(branchLogTempToDelete.BranchId);

                        var branchMap = _mapper.Map<Branch>(branches);
                        branchMap.Action = "N";
                        branchMap.Status = "A";
                        if (!_branchrepository.UpdateBranch(branchMap))
                        {
                            ModelState.AddModelError("", "Something went wrong while saving");
                            return StatusCode(500, ModelState);
                        }
                    }

                    //DELETE TEMP
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);

                    if (!_branchrepository.DeleteBranchLogTemp(branchLogTempToDelete))
                    {
                        ModelState.AddModelError("", "Something went wrong deleting branch temp");
                        return StatusCode(500, ModelState);
                    }
                    //DELETE TEMP

                    dbContextTransaction.Commit();
                    response = "Successfully Deleted Pending Branch";
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

        [HttpPut("editBranchTemp/{pendingId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult updateBranchTemp(int pendingId, [FromBody] BranchDtoPendingUpdate updateBranchLogTemp)
        {
            if (!_branchrepository.BranchTempExists(pendingId))
            {
                return NotFound();
            }

            var branches_ = _branchrepository.GetBranches_all()
            .Where(c => (c.BranchName.Trim().ToUpper() == updateBranchLogTemp.BranchName.TrimEnd().ToUpper() || c.BranchCode.TrimEnd().ToUpper() == updateBranchLogTemp.BranchCode.TrimEnd().ToUpper()) && c.Id != updateBranchLogTemp.ID)
            .FirstOrDefault();

            if (branches_ != null)
            {
                ModelState.AddModelError("", "Branch already exists");
                return StatusCode(409, ModelState);
            }

            var branchestemp_ = _branchrepository.GetPendingBranches_all()
            .Where(c => (c.BranchName.Trim().ToUpper() == updateBranchLogTemp.BranchName.TrimEnd().ToUpper() || c.BranchCode.TrimEnd().ToUpper() == updateBranchLogTemp.BranchCode.TrimEnd().ToUpper()) && c.Id != pendingId)
            .FirstOrDefault();

            if (branchestemp_ != null)
            {
                ModelState.AddModelError("", "Branch already exists. Need approve");
                return StatusCode(409, ModelState);
            }

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var branchLogTempToEdit = _branchrepository.GetBranchLogTemp(pendingId);
                    var response = "";

                    var branchMap = _mapper.Map<BranchLogTemp>(branchLogTempToEdit);
                    branchMap.BranchCode = updateBranchLogTemp.BranchCode;
                    branchMap.BranchName = updateBranchLogTemp.BranchName;
                    branchMap.UpdatedBy = updateBranchLogTemp.UpdatedBy;
                    branchMap.Remarks = updateBranchLogTemp.Remarks;
                    branchMap.ActionRemarks = updateBranchLogTemp.ActionRemarks;
                    branchMap.Status = "NA";
                    if (!_branchrepository.UpdateBranchTemp(branchMap))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    var branchMapLog = _mapper.Map<BranchLog>(branchLogTempToEdit);
                    branchMapLog.Id = 0;
                    branchMapLog.BranchTempId = branchLogTempToEdit.Id;
                    branchMapLog.Action = branchLogTempToEdit.Action;
                    branchMapLog.Status = "NA";
                    branchMapLog.ActionRemarks = updateBranchLogTemp.ActionRemarks;
                    if (!_branchrepository.CreateBranchLog(branchMapLog))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log");
                        return StatusCode(500, ModelState);
                    }

                    dbContextTransaction.Commit();
                    response = "Successfully Update Pending Branch";
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

        [HttpPost("approvalBranch/")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult ApprovalBranches([FromBody] StatusIdList statusIdList)
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

                            if (_branchrepository.BranchTempExists(Convert.ToInt32(id)))
                            {
                                var branchLogTempToDelete = _branchrepository.GetBranchLogTemp(Convert.ToInt32(id));
                                if (branchLogTempToDelete != null)
                                {
                                    if (branchLogTempToDelete.Action == "C" || branchLogTempToDelete.Action == "U" || branchLogTempToDelete.Action == "D")
                                    {
                                        //return StatusCode(500, branchLogTempToDelete.Action);
                                        var updatedBranch = new BranchDto();

                                        if (branchLogTempToDelete.Action == "C")
                                        {
                                            //var updatedBranch = new BranchDto();
                                            var branchMap = _mapper.Map<Branch>(updatedBranch);

                                            branchMap.BranchCode = branchLogTempToDelete.BranchCode;
                                            branchMap.BranchName = branchLogTempToDelete.BranchName;
                                            branchMap.Address1 = branchLogTempToDelete.Address1;
                                            branchMap.Address2 = branchLogTempToDelete.Address2;
                                            branchMap.CityId = branchLogTempToDelete.CityId;
                                            branchMap.ZipCodeId = branchLogTempToDelete.ZipCodeId;
                                            branchMap.ContactPerson = branchLogTempToDelete.ContactPerson;
                                            branchMap.Phone = branchLogTempToDelete.Phone;
                                            branchMap.Fax = branchLogTempToDelete.Fax;
                                            branchMap.UpdatedBy = branchLogTempToDelete.UpdatedBy;
                                            branchMap.Status = statusIdList.Approval;
                                            branchMap.Action = "N";
                                            branchMap.Remarks = branchLogTempToDelete.Remarks;

                                            if (!_branchrepository.CreateBranch(branchMap))
                                            {
                                                ModelState.AddModelError("", "Something went wrong while saving");
                                                return StatusCode(500, ModelState);
                                            }

                                            var branchMapLog = _mapper.Map<BranchLog>(updatedBranch);
                                            branchMapLog.BranchTempId = branchLogTempToDelete.Id;
                                            branchMapLog.BranchId = branchMap.Id;
                                            branchMapLog.BranchCode = branchLogTempToDelete.BranchCode;
                                            branchMapLog.BranchName = branchLogTempToDelete.BranchName;
                                            branchMapLog.Address1 = branchLogTempToDelete.Address1;
                                            branchMapLog.Address2 = branchLogTempToDelete.Address2;
                                            branchMapLog.CityId = branchLogTempToDelete.CityId;
                                            branchMapLog.ZipCodeId = branchLogTempToDelete.ZipCodeId;
                                            branchMapLog.ContactPerson = branchLogTempToDelete.ContactPerson;
                                            branchMapLog.Phone = branchLogTempToDelete.Phone;
                                            branchMapLog.Fax = branchLogTempToDelete.Fax;
                                            branchMapLog.UpdatedBy = branchLogTempToDelete.UpdatedBy;
                                            branchMapLog.Status = statusIdList.Approval;
                                            branchMapLog.Action = branchLogTempToDelete.Action;
                                            branchMapLog.Remarks = branchLogTempToDelete.Remarks;
                                            branchMapLog.ActionRemarks = statusIdList.ActionRemarks;
                                            if (!_branchrepository.CreateBranchLog(branchMapLog))
                                            {
                                                ModelState.AddModelError("", "Something went wrong while saving");
                                                return StatusCode(500, ModelState);
                                            }

                                            response = "Successfully Approve";

                                            _flag = 1;
                                        }
                                        else if (branchLogTempToDelete.Action == "U")
                                        {
                                            //var updatedBranch = new BranchDto();
                                            var branchMap = _mapper.Map<Branch>(updatedBranch);

                                            branchMap.Id = branchLogTempToDelete.BranchId;
                                            branchMap.BranchCode = branchLogTempToDelete.BranchCode;
                                            branchMap.BranchName = branchLogTempToDelete.BranchName;
                                            branchMap.Address1 = branchLogTempToDelete.Address1;
                                            branchMap.Address2 = branchLogTempToDelete.Address2;
                                            branchMap.CityId = branchLogTempToDelete.CityId;
                                            branchMap.ZipCodeId = branchLogTempToDelete.ZipCodeId;
                                            branchMap.ContactPerson = branchLogTempToDelete.ContactPerson;
                                            branchMap.Phone = branchLogTempToDelete.Phone;
                                            branchMap.Fax = branchLogTempToDelete.Fax;
                                            branchMap.UpdatedBy = branchLogTempToDelete.UpdatedBy;
                                            branchMap.Status = statusIdList.Approval;
                                            branchMap.Action = "N";
                                            branchMap.Remarks = branchLogTempToDelete.Remarks;

                                            if (!_branchrepository.UpdateBranch(branchMap))
                                            {
                                                ModelState.AddModelError("", "Something went wrong updating branch");
                                                return StatusCode(500, ModelState);
                                            }

                                            var branchMapLog = _mapper.Map<BranchLog>(updatedBranch);
                                            branchMapLog.BranchTempId = branchLogTempToDelete.Id;
                                            branchMapLog.BranchId = branchMap.Id;
                                            branchMapLog.BranchCode = branchLogTempToDelete.BranchCode;
                                            branchMapLog.BranchName = branchLogTempToDelete.BranchName;
                                            branchMapLog.Address1 = branchLogTempToDelete.Address1;
                                            branchMapLog.Address2 = branchLogTempToDelete.Address2;
                                            branchMapLog.CityId = branchLogTempToDelete.CityId;
                                            branchMapLog.ZipCodeId = branchLogTempToDelete.ZipCodeId;
                                            branchMapLog.ContactPerson = branchLogTempToDelete.ContactPerson;
                                            branchMapLog.Phone = branchLogTempToDelete.Phone;
                                            branchMapLog.Fax = branchLogTempToDelete.Fax;
                                            branchMapLog.UpdatedBy = branchLogTempToDelete.UpdatedBy;
                                            branchMapLog.Status = statusIdList.Approval;
                                            branchMapLog.Action = branchLogTempToDelete.Action;
                                            branchMapLog.Remarks = branchLogTempToDelete.Remarks;
                                            branchMapLog.ActionRemarks = statusIdList.ActionRemarks;
                                            if (!_branchrepository.CreateBranchLog(branchMapLog))
                                            {
                                                ModelState.AddModelError("", "Something went wrong while saving");
                                                return StatusCode(500, ModelState);
                                            }

                                            response = "Successfully Approve";

                                            _flag = 1;
                                        }
                                        else if (branchLogTempToDelete.Action == "D")
                                        {
                                            var getbranchToDelete = _branchrepository.GetBranch(branchLogTempToDelete.BranchId);
                                            //var updatedBranch = new BranchDto();

                                            if (!_branchrepository.DeleteBranch(getbranchToDelete))
                                            {
                                                ModelState.AddModelError("", "Something went wrong deleting branch");
                                            }

                                            var branchMapLog = _mapper.Map<BranchLog>(updatedBranch);
                                            branchMapLog.BranchTempId = branchLogTempToDelete.Id;
                                            branchMapLog.BranchId = getbranchToDelete.Id;
                                            branchMapLog.BranchCode = branchLogTempToDelete.BranchCode;
                                            branchMapLog.BranchName = branchLogTempToDelete.BranchName;
                                            branchMapLog.Address1 = branchLogTempToDelete.Address1;
                                            branchMapLog.Address2 = branchLogTempToDelete.Address2;
                                            branchMapLog.CityId = branchLogTempToDelete.CityId;
                                            branchMapLog.ZipCodeId = branchLogTempToDelete.ZipCodeId;
                                            branchMapLog.ContactPerson = branchLogTempToDelete.ContactPerson;
                                            branchMapLog.Phone = branchLogTempToDelete.Phone;
                                            branchMapLog.Fax = branchLogTempToDelete.Fax;
                                            branchMapLog.UpdatedBy = branchLogTempToDelete.UpdatedBy;
                                            branchMapLog.Status = statusIdList.Approval;
                                            branchMapLog.Action = branchLogTempToDelete.Action;
                                            branchMapLog.Remarks = branchLogTempToDelete.Remarks;
                                            branchMapLog.ActionRemarks = statusIdList.ActionRemarks;
                                            if (!_branchrepository.CreateBranchLog(branchMapLog))
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

                                        if (!_branchrepository.DeleteBranchLogTemp(branchLogTempToDelete))
                                        {
                                            ModelState.AddModelError("", "Something went wrong deleting branch");
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
                            if (_branchrepository.BranchTempExists(Convert.ToInt32(id)))
                            {
                                var branchLogTempToDelete = _branchrepository.GetBranchLogTemp(Convert.ToInt32(id));
                                if (branchLogTempToDelete.Action == "C" || branchLogTempToDelete.Action == "U" || branchLogTempToDelete.Action == "D")
                                {
                                    var branchMapTemp = _mapper.Map<BranchLogTemp>(branchLogTempToDelete);
                                    #region
                                    //branchMap.Id = Convert.ToInt32(id);
                                    //branchMap.BranchId = 0;
                                    //branchMap.BranchCode = branchLogTempToDelete.BranchCode;
                                    //branchMap.BranchName = branchLogTempToDelete.BranchName;
                                    //branchMap.UpdatedBy = branchLogTempToDelete.UpdatedBy;
                                    #endregion
                                    branchMapTemp.Action = branchLogTempToDelete.Action;
                                    branchMapTemp.Status = statusIdList.Approval;
                                    branchMapTemp.ActionRemarks = statusIdList.ActionRemarks;
                                    if (!_branchrepository.UpdateBranchTemp(branchMapTemp))
                                    {
                                        ModelState.AddModelError("", "Something went wrong while saving");
                                        return StatusCode(500, ModelState);
                                    }

                                    #region
                                    //var branchMapLog = _mapper.Map<BranchLog>(updatedBranch);
                                    //branchMapLog.BranchTempId = branchLogTempToDelete.Id;
                                    //branchMapLog.BranchId = 0;
                                    //branchMapLog.BranchCode = branchLogTempToDelete.BranchCode;
                                    //branchMapLog.BranchName = branchLogTempToDelete.BranchName;
                                    //branchMapLog.UpdatedBy = branchLogTempToDelete.UpdatedBy;
                                    //branchMapLog.Status = statusIdList.Approval;
                                    //branchMapLog.Action = branchLogTempToDelete.Action;
                                    //branchMapLog.ActionRemarks = "Rejected2";
                                    //branchMapLog.Remarks = branchLogTempToDelete.Remarks;
                                    #endregion

                                    var branchMapLog = _mapper.Map<BranchLog>(branchLogTempToDelete);
                                    branchMapLog.Id = 0;
                                    branchMapLog.BranchTempId = branchLogTempToDelete.Id;
                                    branchMapLog.Status = statusIdList.Approval;
                                    branchMapLog.Action = branchLogTempToDelete.Action;
                                    branchMapLog.ActionRemarks = statusIdList.ActionRemarks;

                                    if (!_branchrepository.CreateBranchLog(branchMapLog))
                                    {
                                        ModelState.AddModelError("", "Something went wrong while saving");
                                        return StatusCode(500, ModelState);
                                    }

                                    response = "Successfully Rejected";

                                    _flag = 0;
                                    #region
                                    //    var updatedBranch = new BranchDto();

                                    //    if (branchLogTempToDelete.Action == "C")
                                    //    {
                                    //        var branchMapTemp = _mapper.Map<BranchLogTemp>(branchLogTempToDelete);
                                    //        #region
                                    //        //branchMap.Id = Convert.ToInt32(id);
                                    //        //branchMap.BranchId = 0;
                                    //        //branchMap.BranchCode = branchLogTempToDelete.BranchCode;
                                    //        //branchMap.BranchName = branchLogTempToDelete.BranchName;
                                    //        //branchMap.UpdatedBy = branchLogTempToDelete.UpdatedBy;
                                    //        #endregion
                                    //        branchMapTemp.Action = branchLogTempToDelete.Action;
                                    //        branchMapTemp.Status = statusIdList.Approval;
                                    //        branchMapTemp.ActionRemarks = statusIdList.ActionRemarks;
                                    //        if (!_branchrepository.UpdateBranchTemp(branchMapTemp))
                                    //        {
                                    //            ModelState.AddModelError("", "Something went wrong while saving");
                                    //            return StatusCode(500, ModelState);
                                    //        }

                                    //        #region
                                    //        //var branchMapLog = _mapper.Map<BranchLog>(updatedBranch);
                                    //        //branchMapLog.BranchTempId = branchLogTempToDelete.Id;
                                    //        //branchMapLog.BranchId = 0;
                                    //        //branchMapLog.BranchCode = branchLogTempToDelete.BranchCode;
                                    //        //branchMapLog.BranchName = branchLogTempToDelete.BranchName;
                                    //        //branchMapLog.UpdatedBy = branchLogTempToDelete.UpdatedBy;
                                    //        //branchMapLog.Status = statusIdList.Approval;
                                    //        //branchMapLog.Action = branchLogTempToDelete.Action;
                                    //        //branchMapLog.ActionRemarks = "Rejected2";
                                    //        //branchMapLog.Remarks = branchLogTempToDelete.Remarks;
                                    //        #endregion

                                    //        var branchMapLog = _mapper.Map<BranchLog>(branchLogTempToDelete);
                                    //        branchMapLog.Id = 0;
                                    //        branchMapLog.BranchTempId = branchLogTempToDelete.Id;
                                    //        branchMapLog.Status = statusIdList.Approval;
                                    //        branchMapLog.Action = branchLogTempToDelete.Action;
                                    //        branchMapLog.ActionRemarks = statusIdList.ActionRemarks;

                                    //        if (!_branchrepository.CreateBranchLog(branchMapLog))
                                    //        {
                                    //            ModelState.AddModelError("", "Something went wrong while saving");
                                    //            return StatusCode(500, ModelState);
                                    //        }

                                    //        response = "Successfully Rejected";

                                    //        _flag = 0;
                                    //    }
                                    //    else if (branchLogTempToDelete.Action == "U")
                                    //    {
                                    //        #region
                                    //        //REJECT TIDAK LANGSUNG PENGARUH KE DATA UTAMA 2023-12-20
                                    //        //var branchMap = _mapper.Map<Branch>(updatedBranch);

                                    //        //branchMap.Id = branchLogTempToDelete.BranchId;
                                    //        //branchMap.BranchCode = branchLogTempToDelete.BranchCode;
                                    //        //branchMap.BranchName = branchLogTempToDelete.BranchName;
                                    //        //branchMap.UpdatedBy = branchLogTempToDelete.UpdatedBy;
                                    //        //branchMap.Status = "A";
                                    //        //branchMap.Action = "N";

                                    //        //if (!_branchrepository.UpdateBranch(branchMap))
                                    //        //{
                                    //        //    ModelState.AddModelError("", "Something went wrong updating branch");
                                    //        //    return StatusCode(500, ModelState);
                                    //        //}
                                    //        //REJECT TIDAK LANGSUNG PENGARUH KE DATA UTAMA 2023-12-20
                                    //        #endregion

                                    //        var branchMapTemp = _mapper.Map<BranchLogTemp>(branchLogTempToDelete);
                                    //        branchMapTemp.Action = branchLogTempToDelete.Action;
                                    //        branchMapTemp.Status = statusIdList.Approval;
                                    //        branchMapTemp.ActionRemarks = statusIdList.ActionRemarks;
                                    //        if (!_branchrepository.UpdateBranchTemp(branchMapTemp))
                                    //        {
                                    //            ModelState.AddModelError("", "Something went wrong while saving");
                                    //            return StatusCode(500, ModelState);
                                    //        }

                                    //        #region
                                    //        //var branchMapLog = _mapper.Map<BranchLog>(updatedBranch);
                                    //        //branchMapLog.BranchTempId = branchLogTempToDelete.Id;
                                    //        //branchMapLog.BranchId = branchLogTempToDelete.BranchId;
                                    //        //branchMapLog.BranchCode = branchLogTempToDelete.BranchCode;
                                    //        //branchMapLog.BranchName = branchLogTempToDelete.BranchName;
                                    //        //branchMapLog.UpdatedBy = branchLogTempToDelete.UpdatedBy;
                                    //        //branchMapLog.Status = statusIdList.Approval;
                                    //        //branchMapLog.Action = branchLogTempToDelete.Action;
                                    //        //_branchrepository.CreateBranchLog(branchMapLog);
                                    //        #endregion

                                    //        var branchMapLog = _mapper.Map<BranchLog>(branchLogTempToDelete);
                                    //        branchMapLog.Id = 0;
                                    //        branchMapLog.BranchTempId = branchLogTempToDelete.Id;
                                    //        branchMapLog.Status = statusIdList.Approval;
                                    //        branchMapLog.Action = branchLogTempToDelete.Action;
                                    //        branchMapLog.ActionRemarks = statusIdList.ActionRemarks;

                                    //        if (!_branchrepository.CreateBranchLog(branchMapLog))
                                    //        {
                                    //            ModelState.AddModelError("", "Something went wrong while saving");
                                    //            return StatusCode(500, ModelState);
                                    //        }

                                    //        response = "Successfully Rejected";

                                    //        _flag = 0;
                                    //    }
                                    //    else if (branchLogTempToDelete.Action == "D")
                                    //    {
                                    //        #region
                                    //        //var branchMap = _mapper.Map<Branch>(updatedBranch);

                                    //        //branchMap.Id = branchLogTempToDelete.BranchId;
                                    //        //branchMap.BranchCode = branchLogTempToDelete.BranchCode;
                                    //        //branchMap.BranchName = branchLogTempToDelete.BranchName;
                                    //        //branchMap.UpdatedBy = branchLogTempToDelete.UpdatedBy;
                                    //        //branchMap.Status = "A";
                                    //        //branchMap.Action = "N";

                                    //        //if (!_branchrepository.UpdateBranch(branchMap))
                                    //        //{
                                    //        //    ModelState.AddModelError("", "Something went wrong updating branch");
                                    //        //    return StatusCode(500, ModelState);
                                    //        //}
                                    //        #endregion
                                    //        var branchMapTemp = _mapper.Map<BranchLogTemp>(branchLogTempToDelete);
                                    //        branchMapTemp.Action = branchLogTempToDelete.Action;
                                    //        branchMapTemp.Status = statusIdList.Approval;
                                    //        branchMapTemp.ActionRemarks = statusIdList.ActionRemarks;
                                    //        if (!_branchrepository.UpdateBranchTemp(branchMapTemp))
                                    //        {
                                    //            ModelState.AddModelError("", "Something went wrong while saving");
                                    //            return StatusCode(500, ModelState);
                                    //        }
                                    //        #region
                                    //        //var branchMapLog = _mapper.Map<BranchLog>(updatedBranch);
                                    //        //branchMapLog.BranchTempId = branchLogTempToDelete.Id;
                                    //        //branchMapLog.BranchId = branchLogTempToDelete.BranchId;
                                    //        //branchMapLog.BranchCode = branchLogTempToDelete.BranchCode;
                                    //        //branchMapLog.BranchName = branchLogTempToDelete.BranchName;
                                    //        //branchMapLog.UpdatedBy = branchLogTempToDelete.UpdatedBy;
                                    //        //branchMapLog.Status = statusIdList.Approval;
                                    //        //branchMapLog.Action = branchLogTempToDelete.Action;
                                    //        //_branchrepository.CreateBranchLog(branchMapLog);
                                    //        #endregion
                                    //        var branchMapLog = _mapper.Map<BranchLog>(branchLogTempToDelete);
                                    //        branchMapLog.Id = 0;
                                    //        branchMapLog.BranchTempId = branchLogTempToDelete.Id;
                                    //        branchMapLog.Status = statusIdList.Approval;
                                    //        branchMapLog.Action = branchLogTempToDelete.Action;
                                    //        branchMapLog.ActionRemarks = statusIdList.ActionRemarks;

                                    //        if (!_branchrepository.CreateBranchLog(branchMapLog))
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

                                    if (!_branchrepository.DeleteBranchLogTemp(branchLogTempToDelete))
                                    {
                                        ModelState.AddModelError("", "Something went wrong deleting branch");
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

        [HttpGet("PendingBranch/")]
        //[Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Branch>))]
        public IActionResult GetPendingBranches([FromQuery] int limit, [FromQuery] int page, [FromQuery] string sortby, [FromQuery] string sortdesc, [FromQuery] string? keyword)
        {
            var pendingbranches = _mapper.Map<List<BranchDtoPending>>(_branchrepository.GetPendingBranches(limit, page, sortby, sortdesc, keyword));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var count = pendingbranches.Count();

            var count = 0;

            if (keyword == "")
            {
                count = _branchrepository.Getpendingdatacount();
            }
            else
            {
                count = _branchrepository.GetBranchesAdvancePending(keyword).Count;
            }

            var response = ApiResponseHelper.StdApiResponse("Data load success", pendingbranches, datacount: count);

            return Ok(response);
            //return CustomResult("Data load success", branches);
        }

        //DI COMMENT KARENA MAO ADA YANG PAKAI PAGING
        //[HttpGet("PendingBranch/")]
        ////[Authorize]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<Branch>))]
        //public IActionResult GetPendingBranches()
        //{
        //    var pendingbranches = _mapper.Map<List<BranchDtoPending>>(_branchrepository.GetPendingBranches());

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var count = _branchrepository.Getpendingdatacount();

        //    var response = ApiResponse("Data load success", pendingbranches, datacount: count);

        //    return Ok(response);
        //    //return CustomResult("Data load success", branches);
        //}
        //DI COMMENT KARENA MAO ADA YANG PAKAI PAGING

        [HttpGet("PendingBranch/{pendingId}")]
        //[Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Branch>))]
        public IActionResult GetPendingBranch(int pendingId)
        {
            if (!_branchrepository.BranchTempExists(pendingId))
            {
                return NotFound();
            }

            //var branches = _mapper.Map<BranchDtoPending>(_branchrepository.GetBranchLogTemp(pendingId));
            var branches = _mapper.Map<BranchDtoListTableJoinPending>(_branchrepository.BranchDtoTableJoinPending(pendingId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(branches);
        }

        //[HttpPost("process_ids/")]
        //public IActionResult ProcessIds([FromBody] StatusIdList statusIdList)
        //{
        //    try
        //    {
        //        // Process the list of IDs (for demonstration, just echoing back)
        //        foreach (var id in statusIdList.IdList)
        //        {
        //            //var existingRecord = dataStore.FirstOrDefault(record => record.Id == id);
        //            //if (existingRecord != null)
        //            //{
        //            //    // Assuming the update involves changing some property, like "Value"
        //            //    existingRecord.Value = "Updated Data";
        //            //}
        //        }

        //        var result = new { Message = "IDs received successfully",Status = statusIdList.Approval, IdList = statusIdList.IdList };
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { Error = ex.Message });
        //    }
        //}

        private object ApiResponse(string message, object data, int datacount)
        {
            var response = new
            {
                datacount,
                message,
                data
            };

            return response;
        }
    }
}
