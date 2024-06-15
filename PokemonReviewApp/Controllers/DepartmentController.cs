using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using API_Dinamis.Repository;
using System.Collections.Generic;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using API_Dinamis.Dto;
using Microsoft.EntityFrameworkCore;
using API_Dinamis.Data;

namespace API_Dinamis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentRepository _departmentrepository;
        private readonly IMapper _mapper;
        private readonly IAuthRepository _authrepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly DataContext _context;
        public DepartmentController(IDepartmentRepository departmentRepository, IMapper mapper, IAuthRepository authrepository, IPasswordHasher passwordHasher, DataContext context)
        {
            _departmentrepository = departmentRepository;
            _mapper = mapper;
            _authrepository = authrepository;
            _passwordHasher = passwordHasher;
            _context = context;
        }

        [HttpGet]
        //[Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Department>))]
        public IActionResult GetDepartment([FromQuery] int limit, [FromQuery] int page, [FromQuery] string sortby, [FromQuery] string sortdesc, [FromQuery] string? keyword)
        {
            var departments = _mapper.Map<List<DepartmentDto>>(_departmentrepository.GetDepartments(limit, page, sortby, sortdesc, keyword));

            var count = 0;

            if (keyword == "")
            {
                count = _departmentrepository.Getdatacount();
            }
            else
            {
                count = _departmentrepository.GetDepartmensAdvance(keyword).Count;
            }

            var response = ApiResponse("Data load success", departments, datacount: count);

            return Ok(response);
        }

        [HttpGet("{departmentId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Department>))]
        [ProducesResponseType(400)]
        public IActionResult GetDepartment(int departmentId)
        {
            if (!_departmentrepository.DepartmentExists(departmentId))
                return NotFound();

            var departments = _mapper.Map<DepartmentDtoListTableJoin>(_departmentrepository.DepartmentDtoTableJoin(departmentId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(departments);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateDepartment([FromBody] DepartmentDto departmentCreate)
        {
            if (departmentCreate == null)
                return BadRequest(ModelState);

            var departmentCreates = _departmentrepository.GetDepartments_all()
            .Where(c => c.DepartmentName.Trim().ToUpper() == departmentCreate.DepartmentName.TrimEnd().ToUpper() || c.DepartmentCode.TrimEnd().ToUpper() == departmentCreate.DepartmentCode.TrimEnd().ToUpper())
            .FirstOrDefault();

            if (departmentCreates != null)
            {
                ModelState.AddModelError("", "Department already exists");
                return StatusCode(409, ModelState);
            }

            var departmentstemp = _departmentrepository.GetPendingDepartments_all()
            .Where(c => (c.DepartmentName.Trim().ToUpper() == departmentCreate.DepartmentName.TrimEnd().ToUpper() || c.DepartmentCode.TrimEnd().ToUpper() == departmentCreate.DepartmentCode.TrimEnd().ToUpper()) && c.Action == "C")
            .FirstOrDefault();

            if (departmentstemp != null)
            {
                ModelState.AddModelError("", "Department already exists. Need approve");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //COBA MASUKIN KE TEMP DAN LOG DAHULU 2023-12-07
            //BISA DI PAKAI KALAU TIDAK PERLU APPROVAL
            //var departmentMap = _mapper.Map<Department>(departmentCreate);

            //if (!_departmentrepository.CreateDepartment(departmentMap))
            //{
            //    ModelState.AddModelError("", "Something went wrong while saving");
            //    return StatusCode(500, ModelState);
            //}
            //COBA MASUKIN KE TEMP DAN LOG DAHULU

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var departmentMapLogTemp = _mapper.Map<DepartmentLogTemp>(departmentCreate);
                    departmentMapLogTemp.Action = "C";
                    departmentMapLogTemp.Status = "NA";
                    if (!_departmentrepository.CreateDepartmentLogTemp(departmentMapLogTemp))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    var departmentMapLog = _mapper.Map<DepartmentLog>(departmentCreate);
                    departmentMapLog.DepartmentTempId = departmentMapLogTemp.Id;
                    departmentMapLog.Action = "C";
                    departmentMapLog.Status = "NA";
                    if (!_departmentrepository.CreateDepartmentLog(departmentMapLog))
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

        [HttpPut("{departmentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateDepartment(int departmentId, [FromBody] DepartmentDto updatedDepartment)
        {
            if (updatedDepartment == null)
                return BadRequest(ModelState);

            if (departmentId != updatedDepartment.Id)
                return BadRequest(ModelState);

            if (!_departmentrepository.DepartmentExists(departmentId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var departments_ = _departmentrepository.GetDepartments_all()
            .Where(c => (c.DepartmentName.Trim().ToUpper() == updatedDepartment.DepartmentName.TrimEnd().ToUpper() || c.DepartmentCode.TrimEnd().ToUpper() == updatedDepartment.DepartmentCode.TrimEnd().ToUpper()) && c.Id != departmentId)
            .FirstOrDefault();

            if (departments_ != null)
            {
                ModelState.AddModelError("", "Department already exists");
                return StatusCode(409, ModelState);
            }

            var departmentstemp_ = _departmentrepository.GetPendingDepartments_all()
            .Where(c => (c.DepartmentName.Trim().ToUpper() == updatedDepartment.DepartmentName.TrimEnd().ToUpper() || c.DepartmentCode.TrimEnd().ToUpper() == updatedDepartment.DepartmentCode.TrimEnd().ToUpper()) && c.DepartmentId != departmentId)
            .FirstOrDefault();

            if (departmentstemp_ != null)
            {
                ModelState.AddModelError("", "Department already exists. Need approve");
                return StatusCode(409, ModelState);
            }

            var departmentstemp = _departmentrepository.GetPendingDepartments_all()
            .Where(c => c.DepartmentId == departmentId && c.Action == "U")
            .FirstOrDefault();

            if (departmentstemp != null)
            {
                ModelState.AddModelError("", "Department already exists. Need approve");
                return StatusCode(409, ModelState);
            }

            //COBA MASUKIN KE TEMP DAN LOG DAHULU 2023-12-07
            //var departmentMap = _mapper.Map<Department>(updatedDepartment);

            ////if (!_departmentrepository.UpdateDepartment(departmentId, departmentMap))
            //if (!_departmentrepository.UpdateDepartment(departmentMap))
            //{
            //    ModelState.AddModelError("", "Something went wrong updating department");
            //    return StatusCode(500, ModelState);
            //}

            //return NoContent();
            //COBA MASUKIN KE TEMP DAN LOG DAHULU 2023-12-07

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var departments = _departmentrepository.GetDepartment(departmentId);

                    var departmentMap = _mapper.Map<Department>(departments);
                    departmentMap.Action = "U";
                    departmentMap.Status = "NA";
                    if (!_departmentrepository.UpdateDepartment(departmentMap))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    var departmentMapLogTemp = _mapper.Map<DepartmentLogTemp>(updatedDepartment);
                    departmentMapLogTemp.Id = 0;
                    departmentMapLogTemp.DepartmentId = departmentId;
                    departmentMapLogTemp.Action = "U";
                    departmentMapLogTemp.Status = "NA";
                    if (!_departmentrepository.CreateDepartmentLogTemp(departmentMapLogTemp))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log temp");
                        return StatusCode(500, ModelState);
                    }

                    var departmentMapLog = _mapper.Map<DepartmentLog>(updatedDepartment);
                    departmentMapLog.Id = 0;
                    departmentMapLog.DepartmentId = departmentId;
                    departmentMapLog.DepartmentTempId = departmentMapLogTemp.Id;
                    departmentMapLog.Action = "U";
                    departmentMapLog.Status = "NA";
                    if (!_departmentrepository.CreateDepartmentLog(departmentMapLog))
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

        [HttpDelete("{departmentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteDepartment(int departmentId)
        {
            if (!_departmentrepository.DepartmentExists(departmentId))
            {
                return NotFound();
            }

            var departmentestemp = _departmentrepository.GetPendingDepartments_all()
            .Where(c => c.DepartmentId == departmentId && c.Action == "D")
            .FirstOrDefault();

            if (departmentestemp != null)
            {
                ModelState.AddModelError("", "Department already exists. Need approve");
                return StatusCode(409, ModelState);
            }

            //COBA MASUKIN KE TEMP DAN LOG DAHULU 2023-12-07
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            //if (!_departmentrepository.DeleteDepartment(departmentToDelete))
            //{
            //    ModelState.AddModelError("", "Something went wrong deleting department");
            //}

            //return NoContent();
            //COBA MASUKIN KE TEMP DAN LOG DAHULU 2023-12-07

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var departments = _departmentrepository.GetDepartment(departmentId);

                    var departmentMap = _mapper.Map<Department>(departments);
                    departmentMap.Action = "D";
                    departmentMap.Status = "NA";
                    if (!_departmentrepository.UpdateDepartment(departmentMap))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    var departmentMapLogTemp = _mapper.Map<DepartmentLogTemp>(departmentMap);
                    departmentMapLogTemp.Id = 0;
                    departmentMapLogTemp.DepartmentId = departmentId;
                    departmentMapLogTemp.Action = "D";
                    departmentMapLogTemp.Status = "NA";
                    departmentMapLogTemp.Remarks = "";
                    if (!_departmentrepository.CreateDepartmentLogTemp(departmentMapLogTemp))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log temp");
                        return StatusCode(500, ModelState);
                    }

                    var departmentMapLog = _mapper.Map<DepartmentLog>(departmentMap);
                    departmentMapLog.Id = 0;
                    departmentMapLog.DepartmentId = departmentId;
                    departmentMapLog.DepartmentTempId = departmentMapLogTemp.Id;
                    departmentMapLog.Action = "D";
                    departmentMapLog.Status = "NA";
                    departmentMapLog.Remarks = "";
                    //_departmentrepository.CreateDepartmentLog(departmentMapLog);
                    if (!_departmentrepository.CreateDepartmentLog(departmentMapLog))
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

        [HttpDelete("deleteDepartmentTemp/{pendingId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteDepartmentTemp(int pendingId, [FromBody] ActionRemark Actremark)
        {
            if (!_departmentrepository.DepartmentTempExists(pendingId))
            {
                return NotFound();
            }

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var departmentLogTempToDelete = _departmentrepository.GetDepartmentLogTemp(pendingId);
                    var response = "";

                    var departmentMapLog = _mapper.Map<DepartmentLog>(departmentLogTempToDelete);
                    departmentMapLog.Id = 0;
                    departmentMapLog.DepartmentTempId = departmentLogTempToDelete.Id;
                    departmentMapLog.ActionRemarks = Actremark.ActionRemarks;
                    if (!_departmentrepository.CreateDepartmentLog(departmentMapLog))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log");
                        return StatusCode(500, ModelState);
                    }

                    if (_departmentrepository.DepartmentExists(departmentLogTempToDelete.DepartmentId))
                    {
                        var departments = _departmentrepository.GetDepartment(departmentLogTempToDelete.DepartmentId);

                        var departmentMap = _mapper.Map<Department>(departments);
                        departmentMap.Action = "N";
                        departmentMap.Status = "A";
                        if (!_departmentrepository.UpdateDepartment(departmentMap))
                        {
                            ModelState.AddModelError("", "Something went wrong while saving");
                            return StatusCode(500, ModelState);
                        }
                    }

                    //DELETE TEMP
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);

                    if (!_departmentrepository.DeleteDepartmentLogTemp(departmentLogTempToDelete))
                    {
                        ModelState.AddModelError("", "Something went wrong deleting department temp");
                        return StatusCode(500, ModelState);
                    }
                    //DELETE TEMP

                    dbContextTransaction.Commit();
                    response = "Successfully Deleted Pending Department";
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

        [HttpPut("editDepartmentTemp/{pendingId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult updateDepartmentTemp(int pendingId, [FromBody] DepartmentDtoPendingUpdate updateDepartmentLogTemp)
        {
            if (!_departmentrepository.DepartmentTempExists(pendingId))
            {
                return NotFound();
            }

            var departments_ = _departmentrepository.GetDepartments_all()
            .Where(c => (c.DepartmentName.Trim().ToUpper() == updateDepartmentLogTemp.DepartmentName.TrimEnd().ToUpper() || c.DepartmentCode.TrimEnd().ToUpper() == updateDepartmentLogTemp.DepartmentCode.TrimEnd().ToUpper()) && c.Id != updateDepartmentLogTemp.ID)
            .FirstOrDefault();

            if (departments_ != null)
            {
                ModelState.AddModelError("", "Department already exists");
                return StatusCode(409, ModelState);
            }

            var departmentstemp_ = _departmentrepository.GetPendingDepartments_all()
            .Where(c => (c.DepartmentName.Trim().ToUpper() == updateDepartmentLogTemp.DepartmentName.TrimEnd().ToUpper() || c.DepartmentCode.TrimEnd().ToUpper() == updateDepartmentLogTemp.DepartmentCode.TrimEnd().ToUpper()) && c.Id != pendingId)
            .FirstOrDefault();

            if (departmentstemp_ != null)
            {
                ModelState.AddModelError("", "Department already exists. Need approve");
                return StatusCode(409, ModelState);
            }

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var departmentLogTempToEdit = _departmentrepository.GetDepartmentLogTemp(pendingId);
                    var response = "";

                    var departmentMap = _mapper.Map<DepartmentLogTemp>(departmentLogTempToEdit);
                    departmentMap.DepartmentCode = updateDepartmentLogTemp.DepartmentCode;
                    departmentMap.DepartmentName = updateDepartmentLogTemp.DepartmentName;
                    departmentMap.UpdatedBy = updateDepartmentLogTemp.UpdatedBy;
                    departmentMap.Remarks = updateDepartmentLogTemp.Remarks;
                    departmentMap.ActionRemarks = updateDepartmentLogTemp.ActionRemarks;
                    departmentMap.Status = "NA";
                    if (!_departmentrepository.UpdateDepartmentTemp(departmentMap))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    var departmentMapLog = _mapper.Map<DepartmentLog>(departmentLogTempToEdit);
                    departmentMapLog.Id = 0;
                    departmentMapLog.DepartmentTempId = departmentLogTempToEdit.Id;
                    departmentMapLog.Action = departmentLogTempToEdit.Action;
                    departmentMapLog.Status = "NA";
                    departmentMapLog.ActionRemarks = updateDepartmentLogTemp.ActionRemarks;
                    if (!_departmentrepository.CreateDepartmentLog(departmentMapLog))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log");
                        return StatusCode(500, ModelState);
                    }

                    dbContextTransaction.Commit();
                    response = "Successfully Update Pending Department";
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

        [HttpPost("approvalDepartment/")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult ApprovalDepartments([FromBody] StatusIdList statusIdList)
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

                            if (_departmentrepository.DepartmentTempExists(Convert.ToInt32(id)))
                            {
                                var departmentLogTempToDelete = _departmentrepository.GetDepartmentLogTemp(Convert.ToInt32(id));
                                if (departmentLogTempToDelete != null)
                                {
                                    if (departmentLogTempToDelete.Action == "C" || departmentLogTempToDelete.Action == "U" || departmentLogTempToDelete.Action == "D")
                                    {
                                        //return StatusCode(500, departmentLogTempToDelete.Action);
                                        var updatedDepartment = new DepartmentDto();

                                        if (departmentLogTempToDelete.Action == "C")
                                        {
                                            //var updatedDepartment = new DepartmentDto();
                                            var departmentMap = _mapper.Map<Department>(updatedDepartment);

                                            departmentMap.DepartmentCode = departmentLogTempToDelete.DepartmentCode;
                                            departmentMap.DepartmentName = departmentLogTempToDelete.DepartmentName;
                                            departmentMap.BranchId = departmentLogTempToDelete.BranchId;
                                            departmentMap.UpdatedBy = departmentLogTempToDelete.UpdatedBy;
                                            departmentMap.Status = statusIdList.Approval;
                                            departmentMap.Action = "N";
                                            departmentMap.Remarks = departmentLogTempToDelete.Remarks;

                                            if (!_departmentrepository.CreateDepartment(departmentMap))
                                            {
                                                ModelState.AddModelError("", "Something went wrong while saving");
                                                return StatusCode(500, ModelState);
                                            }

                                            var departmentMapLog = _mapper.Map<DepartmentLog>(updatedDepartment);
                                            departmentMapLog.DepartmentTempId = departmentLogTempToDelete.Id;
                                            departmentMapLog.DepartmentId = departmentMap.Id;
                                            departmentMapLog.DepartmentCode = departmentLogTempToDelete.DepartmentCode;
                                            departmentMapLog.DepartmentName = departmentLogTempToDelete.DepartmentName;
                                            departmentMapLog.DepartmentId = departmentLogTempToDelete.DepartmentId;
                                            departmentMapLog.UpdatedBy = departmentLogTempToDelete.UpdatedBy;
                                            departmentMapLog.Status = statusIdList.Approval;
                                            departmentMapLog.Action = departmentLogTempToDelete.Action;
                                            departmentMapLog.Remarks = departmentLogTempToDelete.Remarks;
                                            departmentMapLog.ActionRemarks = statusIdList.ActionRemarks;
                                            if (!_departmentrepository.CreateDepartmentLog(departmentMapLog))
                                            {
                                                ModelState.AddModelError("", "Something went wrong while saving");
                                                return StatusCode(500, ModelState);
                                            }

                                            response = "Successfully Approve";

                                            _flag = 1;
                                        }
                                        else if (departmentLogTempToDelete.Action == "U")
                                        {
                                            //var updatedDepartment = new DepartmentDto();
                                            var departmentMap = _mapper.Map<Department>(updatedDepartment);

                                            departmentMap.Id = departmentLogTempToDelete.DepartmentId;
                                            departmentMap.DepartmentCode = departmentLogTempToDelete.DepartmentCode;
                                            departmentMap.DepartmentName = departmentLogTempToDelete.DepartmentName;
                                            departmentMap.BranchId = departmentLogTempToDelete.BranchId;
                                            departmentMap.UpdatedBy = departmentLogTempToDelete.UpdatedBy;
                                            departmentMap.Status = statusIdList.Approval;
                                            departmentMap.Action = "N";
                                            departmentMap.Remarks = departmentLogTempToDelete.Remarks;

                                            if (!_departmentrepository.UpdateDepartment(departmentMap))
                                            {
                                                ModelState.AddModelError("", "Something went wrong updating department");
                                                return StatusCode(500, ModelState);
                                            }

                                            var departmentMapLog = _mapper.Map<DepartmentLog>(updatedDepartment);
                                            departmentMapLog.DepartmentTempId = departmentLogTempToDelete.Id;
                                            departmentMapLog.DepartmentId = departmentMap.Id;
                                            departmentMapLog.DepartmentCode = departmentLogTempToDelete.DepartmentCode;
                                            departmentMapLog.DepartmentName = departmentLogTempToDelete.DepartmentName;
                                            departmentMapLog.DepartmentId = departmentLogTempToDelete.DepartmentId;
                                            departmentMapLog.UpdatedBy = departmentLogTempToDelete.UpdatedBy;
                                            departmentMapLog.Status = statusIdList.Approval;
                                            departmentMapLog.Action = departmentLogTempToDelete.Action;
                                            departmentMapLog.Remarks = departmentLogTempToDelete.Remarks;
                                            departmentMapLog.ActionRemarks = statusIdList.ActionRemarks;
                                            if (!_departmentrepository.CreateDepartmentLog(departmentMapLog))
                                            {
                                                ModelState.AddModelError("", "Something went wrong while saving");
                                                return StatusCode(500, ModelState);
                                            }

                                            response = "Successfully Approve";

                                            _flag = 1;
                                        }
                                        else if (departmentLogTempToDelete.Action == "D")
                                        {
                                            var getdepartmentToDelete = _departmentrepository.GetDepartment(departmentLogTempToDelete.DepartmentId);
                                            //var updatedDepartment = new DepartmentDto();

                                            if (!_departmentrepository.DeleteDepartment(getdepartmentToDelete))
                                            {
                                                ModelState.AddModelError("", "Something went wrong deleting department");
                                            }

                                            var departmentMapLog = _mapper.Map<DepartmentLog>(updatedDepartment);
                                            departmentMapLog.DepartmentTempId = departmentLogTempToDelete.Id;
                                            departmentMapLog.DepartmentId = getdepartmentToDelete.Id;
                                            departmentMapLog.DepartmentCode = departmentLogTempToDelete.DepartmentCode;
                                            departmentMapLog.DepartmentName = departmentLogTempToDelete.DepartmentName;
                                            departmentMapLog.DepartmentId = departmentLogTempToDelete.DepartmentId;
                                            departmentMapLog.UpdatedBy = departmentLogTempToDelete.UpdatedBy;
                                            departmentMapLog.Status = statusIdList.Approval;
                                            departmentMapLog.Action = departmentLogTempToDelete.Action;
                                            departmentMapLog.Remarks = departmentLogTempToDelete.Remarks;
                                            departmentMapLog.ActionRemarks = statusIdList.ActionRemarks;
                                            if (!_departmentrepository.CreateDepartmentLog(departmentMapLog))
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

                                        if (!_departmentrepository.DeleteDepartmentLogTemp(departmentLogTempToDelete))
                                        {
                                            ModelState.AddModelError("", "Something went wrong deleting department");
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
                            if (_departmentrepository.DepartmentTempExists(Convert.ToInt32(id)))
                            {
                                var departmentLogTempToDelete = _departmentrepository.GetDepartmentLogTemp(Convert.ToInt32(id));
                                if (departmentLogTempToDelete.Action == "C" || departmentLogTempToDelete.Action == "U" || departmentLogTempToDelete.Action == "D")
                                {
                                    var departmentMapTemp = _mapper.Map<DepartmentLogTemp>(departmentLogTempToDelete);
                                    #region
                                    //departmentMap.Id = Convert.ToInt32(id);
                                    //departmentMap.DepartmentId = 0;
                                    //departmentMap.DepartmentCode = departmentLogTempToDelete.DepartmentCode;
                                    //departmentMap.DepartmentName = departmentLogTempToDelete.DepartmentName;
                                    //departmentMap.UpdatedBy = departmentLogTempToDelete.UpdatedBy;
                                    #endregion
                                    departmentMapTemp.Action = departmentLogTempToDelete.Action;
                                    departmentMapTemp.Status = statusIdList.Approval;
                                    departmentMapTemp.ActionRemarks = statusIdList.ActionRemarks;
                                    if (!_departmentrepository.UpdateDepartmentTemp(departmentMapTemp))
                                    {
                                        ModelState.AddModelError("", "Something went wrong while saving");
                                        return StatusCode(500, ModelState);
                                    }

                                    #region
                                    //var departmentMapLog = _mapper.Map<DepartmentLog>(updatedDepartment);
                                    //departmentMapLog.DepartmentTempId = departmentLogTempToDelete.Id;
                                    //departmentMapLog.DepartmentId = 0;
                                    //departmentMapLog.DepartmentCode = departmentLogTempToDelete.DepartmentCode;
                                    //departmentMapLog.DepartmentName = departmentLogTempToDelete.DepartmentName;
                                    //departmentMapLog.UpdatedBy = departmentLogTempToDelete.UpdatedBy;
                                    //departmentMapLog.Status = statusIdList.Approval;
                                    //departmentMapLog.Action = departmentLogTempToDelete.Action;
                                    //departmentMapLog.ActionRemarks = "Rejected2";
                                    //departmentMapLog.Remarks = departmentLogTempToDelete.Remarks;
                                    #endregion

                                    var departmentMapLog = _mapper.Map<DepartmentLog>(departmentLogTempToDelete);
                                    departmentMapLog.Id = 0;
                                    departmentMapLog.DepartmentTempId = departmentLogTempToDelete.Id;
                                    departmentMapLog.Status = statusIdList.Approval;
                                    departmentMapLog.Action = departmentLogTempToDelete.Action;
                                    departmentMapLog.ActionRemarks = statusIdList.ActionRemarks;

                                    if (!_departmentrepository.CreateDepartmentLog(departmentMapLog))
                                    {
                                        ModelState.AddModelError("", "Something went wrong while saving");
                                        return StatusCode(500, ModelState);
                                    }

                                    response = "Successfully Rejected";

                                    _flag = 0;
                                    #region
                                    //    var updatedDepartment = new DepartmentDto();

                                    //    if (departmentLogTempToDelete.Action == "C")
                                    //    {
                                    //        var departmentMapTemp = _mapper.Map<DepartmentLogTemp>(departmentLogTempToDelete);
                                    //        #region
                                    //        //departmentMap.Id = Convert.ToInt32(id);
                                    //        //departmentMap.DepartmentId = 0;
                                    //        //departmentMap.DepartmentCode = departmentLogTempToDelete.DepartmentCode;
                                    //        //departmentMap.DepartmentName = departmentLogTempToDelete.DepartmentName;
                                    //        //departmentMap.UpdatedBy = departmentLogTempToDelete.UpdatedBy;
                                    //        #endregion
                                    //        departmentMapTemp.Action = departmentLogTempToDelete.Action;
                                    //        departmentMapTemp.Status = statusIdList.Approval;
                                    //        departmentMapTemp.ActionRemarks = statusIdList.ActionRemarks;
                                    //        if (!_departmentrepository.UpdateDepartmentTemp(departmentMapTemp))
                                    //        {
                                    //            ModelState.AddModelError("", "Something went wrong while saving");
                                    //            return StatusCode(500, ModelState);
                                    //        }

                                    //        #region
                                    //        //var departmentMapLog = _mapper.Map<DepartmentLog>(updatedDepartment);
                                    //        //departmentMapLog.DepartmentTempId = departmentLogTempToDelete.Id;
                                    //        //departmentMapLog.DepartmentId = 0;
                                    //        //departmentMapLog.DepartmentCode = departmentLogTempToDelete.DepartmentCode;
                                    //        //departmentMapLog.DepartmentName = departmentLogTempToDelete.DepartmentName;
                                    //        //departmentMapLog.UpdatedBy = departmentLogTempToDelete.UpdatedBy;
                                    //        //departmentMapLog.Status = statusIdList.Approval;
                                    //        //departmentMapLog.Action = departmentLogTempToDelete.Action;
                                    //        //departmentMapLog.ActionRemarks = "Rejected2";
                                    //        //departmentMapLog.Remarks = departmentLogTempToDelete.Remarks;
                                    //        #endregion

                                    //        var departmentMapLog = _mapper.Map<DepartmentLog>(departmentLogTempToDelete);
                                    //        departmentMapLog.Id = 0;
                                    //        departmentMapLog.DepartmentTempId = departmentLogTempToDelete.Id;
                                    //        departmentMapLog.Status = statusIdList.Approval;
                                    //        departmentMapLog.Action = departmentLogTempToDelete.Action;
                                    //        departmentMapLog.ActionRemarks = statusIdList.ActionRemarks;

                                    //        if (!_departmentrepository.CreateDepartmentLog(departmentMapLog))
                                    //        {
                                    //            ModelState.AddModelError("", "Something went wrong while saving");
                                    //            return StatusCode(500, ModelState);
                                    //        }

                                    //        response = "Successfully Rejected";

                                    //        _flag = 0;
                                    //    }
                                    //    else if (departmentLogTempToDelete.Action == "U")
                                    //    {
                                    //        #region
                                    //        //REJECT TIDAK LANGSUNG PENGARUH KE DATA UTAMA 2023-12-20
                                    //        //var departmentMap = _mapper.Map<Department>(updatedDepartment);

                                    //        //departmentMap.Id = departmentLogTempToDelete.DepartmentId;
                                    //        //departmentMap.DepartmentCode = departmentLogTempToDelete.DepartmentCode;
                                    //        //departmentMap.DepartmentName = departmentLogTempToDelete.DepartmentName;
                                    //        //departmentMap.UpdatedBy = departmentLogTempToDelete.UpdatedBy;
                                    //        //departmentMap.Status = "A";
                                    //        //departmentMap.Action = "N";

                                    //        //if (!_departmentrepository.UpdateDepartment(departmentMap))
                                    //        //{
                                    //        //    ModelState.AddModelError("", "Something went wrong updating department");
                                    //        //    return StatusCode(500, ModelState);
                                    //        //}
                                    //        //REJECT TIDAK LANGSUNG PENGARUH KE DATA UTAMA 2023-12-20
                                    //        #endregion

                                    //        var departmentMapTemp = _mapper.Map<DepartmentLogTemp>(departmentLogTempToDelete);
                                    //        departmentMapTemp.Action = departmentLogTempToDelete.Action;
                                    //        departmentMapTemp.Status = statusIdList.Approval;
                                    //        departmentMapTemp.ActionRemarks = statusIdList.ActionRemarks;
                                    //        if (!_departmentrepository.UpdateDepartmentTemp(departmentMapTemp))
                                    //        {
                                    //            ModelState.AddModelError("", "Something went wrong while saving");
                                    //            return StatusCode(500, ModelState);
                                    //        }

                                    //        #region
                                    //        //var departmentMapLog = _mapper.Map<DepartmentLog>(updatedDepartment);
                                    //        //departmentMapLog.DepartmentTempId = departmentLogTempToDelete.Id;
                                    //        //departmentMapLog.DepartmentId = departmentLogTempToDelete.DepartmentId;
                                    //        //departmentMapLog.DepartmentCode = departmentLogTempToDelete.DepartmentCode;
                                    //        //departmentMapLog.DepartmentName = departmentLogTempToDelete.DepartmentName;
                                    //        //departmentMapLog.UpdatedBy = departmentLogTempToDelete.UpdatedBy;
                                    //        //departmentMapLog.Status = statusIdList.Approval;
                                    //        //departmentMapLog.Action = departmentLogTempToDelete.Action;
                                    //        //_departmentrepository.CreateDepartmentLog(departmentMapLog);
                                    //        #endregion

                                    //        var departmentMapLog = _mapper.Map<DepartmentLog>(departmentLogTempToDelete);
                                    //        departmentMapLog.Id = 0;
                                    //        departmentMapLog.DepartmentTempId = departmentLogTempToDelete.Id;
                                    //        departmentMapLog.Status = statusIdList.Approval;
                                    //        departmentMapLog.Action = departmentLogTempToDelete.Action;
                                    //        departmentMapLog.ActionRemarks = statusIdList.ActionRemarks;

                                    //        if (!_departmentrepository.CreateDepartmentLog(departmentMapLog))
                                    //        {
                                    //            ModelState.AddModelError("", "Something went wrong while saving");
                                    //            return StatusCode(500, ModelState);
                                    //        }

                                    //        response = "Successfully Rejected";

                                    //        _flag = 0;
                                    //    }
                                    //    else if (departmentLogTempToDelete.Action == "D")
                                    //    {
                                    //        #region
                                    //        //var departmentMap = _mapper.Map<Department>(updatedDepartment);

                                    //        //departmentMap.Id = departmentLogTempToDelete.DepartmentId;
                                    //        //departmentMap.DepartmentCode = departmentLogTempToDelete.DepartmentCode;
                                    //        //departmentMap.DepartmentName = departmentLogTempToDelete.DepartmentName;
                                    //        //departmentMap.UpdatedBy = departmentLogTempToDelete.UpdatedBy;
                                    //        //departmentMap.Status = "A";
                                    //        //departmentMap.Action = "N";

                                    //        //if (!_departmentrepository.UpdateDepartment(departmentMap))
                                    //        //{
                                    //        //    ModelState.AddModelError("", "Something went wrong updating department");
                                    //        //    return StatusCode(500, ModelState);
                                    //        //}
                                    //        #endregion
                                    //        var departmentMapTemp = _mapper.Map<DepartmentLogTemp>(departmentLogTempToDelete);
                                    //        departmentMapTemp.Action = departmentLogTempToDelete.Action;
                                    //        departmentMapTemp.Status = statusIdList.Approval;
                                    //        departmentMapTemp.ActionRemarks = statusIdList.ActionRemarks;
                                    //        if (!_departmentrepository.UpdateDepartmentTemp(departmentMapTemp))
                                    //        {
                                    //            ModelState.AddModelError("", "Something went wrong while saving");
                                    //            return StatusCode(500, ModelState);
                                    //        }
                                    //        #region
                                    //        //var departmentMapLog = _mapper.Map<DepartmentLog>(updatedDepartment);
                                    //        //departmentMapLog.DepartmentTempId = departmentLogTempToDelete.Id;
                                    //        //departmentMapLog.DepartmentId = departmentLogTempToDelete.DepartmentId;
                                    //        //departmentMapLog.DepartmentCode = departmentLogTempToDelete.DepartmentCode;
                                    //        //departmentMapLog.DepartmentName = departmentLogTempToDelete.DepartmentName;
                                    //        //departmentMapLog.UpdatedBy = departmentLogTempToDelete.UpdatedBy;
                                    //        //departmentMapLog.Status = statusIdList.Approval;
                                    //        //departmentMapLog.Action = departmentLogTempToDelete.Action;
                                    //        //_departmentrepository.CreateDepartmentLog(departmentMapLog);
                                    //        #endregion
                                    //        var departmentMapLog = _mapper.Map<DepartmentLog>(departmentLogTempToDelete);
                                    //        departmentMapLog.Id = 0;
                                    //        departmentMapLog.DepartmentTempId = departmentLogTempToDelete.Id;
                                    //        departmentMapLog.Status = statusIdList.Approval;
                                    //        departmentMapLog.Action = departmentLogTempToDelete.Action;
                                    //        departmentMapLog.ActionRemarks = statusIdList.ActionRemarks;

                                    //        if (!_departmentrepository.CreateDepartmentLog(departmentMapLog))
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

                                    if (!_departmentrepository.DeleteDepartmentLogTemp(departmentLogTempToDelete))
                                    {
                                        ModelState.AddModelError("", "Something went wrong deleting department");
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

        [HttpGet("PendingDepartment/")]
        //[Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Department>))]
        public IActionResult GetPendingDepartments([FromQuery] int limit, [FromQuery] int page, [FromQuery] string sortby, [FromQuery] string sortdesc, [FromQuery] string? keyword)
        {
            var pendingdepartments = _mapper.Map<List<DepartmentDtoPending>>(_departmentrepository.GetPendingDepartments(limit, page, sortby, sortdesc, keyword));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var count = pendingdepartments.Count();

            var count = 0;

            if (keyword == "")
            {
                count = _departmentrepository.Getpendingdatacount();
            }
            else
            {
                count = _departmentrepository.GetDepartmentsAdvancePending(keyword).Count;
            }

            var response = ApiResponse("Data load success", pendingdepartments, datacount: count);

            return Ok(response);
            //return CustomResult("Data load success", departments);
        }

        [HttpGet("PendingDepartment/{pendingId}")]
        //[Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Department>))]
        public IActionResult GetPendingDepartment(int pendingId)
        {
            if (!_departmentrepository.DepartmentTempExists(pendingId))
            {
                return NotFound();
            }

            //var departments = _mapper.Map<DepartmentDtoPending>(_departmentrepository.GetDepartmentLogTemp(pendingId));
            var departments = _mapper.Map<DepartmentDtoListTableJoinPending>(_departmentrepository.DepartmentDtoTableJoinPending(pendingId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(departments);
        }

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
