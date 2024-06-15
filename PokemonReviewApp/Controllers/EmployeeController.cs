using API_Dinamis.Data;
using API_Dinamis.Dto;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using API_Dinamis.Repository;
using API_Dinamis.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection;

namespace API_Dinamis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeerepository;
        private readonly IMapper _mapper;
        private readonly IAuthRepository _authrepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly DataContext _context;

        public EmployeeController(IEmployeeRepository employeerepository, IMapper mapper, IAuthRepository authrepository, IPasswordHasher passwordHasher, DataContext context)
        {
            _employeerepository = employeerepository;
            _mapper = mapper;
            _authrepository = authrepository;
            _passwordHasher = passwordHasher;
            _context = context;
        }

        [HttpGet("AllEmployee")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Employee>))]
        public IActionResult GetEmployee_All()
        {
            var employees = _mapper.Map<List<EmployeeDto>>(_employeerepository.GetEmployees_all());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = ApiResponseHelper.StdApiResponse("Data load success", employees, datacount: employees.Count);

            return Ok(response);
        }

        [HttpGet("getEmployees/")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Employee>))]
        public IActionResult GetEmployee([FromQuery] int? limit = 0, [FromQuery] int? page = 1, [FromQuery] string? sortby = "d", [FromQuery] string? sortdesc = "id", [FromQuery] string? keyword = "")
        {
            var employees = _mapper.Map<List<EmployeeDtoForm>>(_employeerepository.GetEmployees(limit, page, sortby, sortdesc, keyword));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var count = 0;

            if (keyword == "")
            {
                count = _employeerepository.Getdatacount();
            }
            else
            {
                count = _employeerepository.GetEmployeesAdvance(keyword).Count;
            }

            var response = ApiResponseHelper.StdApiResponse("Data load success", employees, datacount: count);

            return Ok(response);
        }

        [HttpGet("{employeeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Employee>))]
        [ProducesResponseType(400)]
        public IActionResult GetEmployee(int employeeId)
        {
            if (!_employeerepository.EmployeeExists(employeeId))
                return NotFound();

            var employees = _mapper.Map<EmployeeDto>(_employeerepository.GetEmployee(employeeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(employees);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateEmployee([FromBody] EmployeeResultDto employeeCreate)
        {
            if (employeeCreate == null)
                return BadRequest(ModelState);

            var employees = _employeerepository.GetEmployees_all()
            .Where(c => c.Name.Trim().ToUpper() == employeeCreate.Name.TrimEnd().ToUpper())
            .FirstOrDefault();

            if (employees != null)
            {
                ModelState.AddModelError("", "Employee already exists");
                return StatusCode(409, ModelState);
            }

            var employeestemp = _employeerepository.GetPendingemployees_all()
            .Where(c => (c.Name.Trim().ToUpper() == employeeCreate.Name.TrimEnd().ToUpper()) && c.Action == "C")
            .FirstOrDefault();

            if (employeestemp != null)
            {
                ModelState.AddModelError("", "Employee already exists. Need approve");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //COBA MASUKIN KE TEMP DAN LOG DAHULU 2023-12-07
            //var employeeMap = _mapper.Map<Employee>(employeeCreate);

            //if (!_employeerepository.CreateEmployee(employeeMap))
            //{
            //    ModelState.AddModelError("", "Something went wrong while saving");
            //    return StatusCode(500, ModelState);
            //}
            //COBA MASUKIN KE TEMP DAN LOG DAHULU

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //==================== H E A D E R ==============================
                    var employeeMapLogTemp = _mapper.Map<EmployeeLogTemp>(employeeCreate);
                    employeeMapLogTemp.Action = "C";
                    employeeMapLogTemp.Status = "NA";
                    if (!_employeerepository.CreateEmployeeHeaderLogTemp(employeeMapLogTemp))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    var employeeMapLog = _mapper.Map<EmployeeLog>(employeeCreate);
                    employeeMapLog.EmployeeTempId = employeeMapLogTemp.Id;
                    employeeMapLog.Action = "C";
                    employeeMapLog.Status = "NA";
                    if (!_employeerepository.CreateEmployeeHeaderLog(employeeMapLog))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }
                    //==================== H E A D E R ==============================

                    //==================== D E T A I L ==============================
                    //List<EmployeeDetailLogTemp> employeeDetailsLogTemp = new List<EmployeeDetailLogTemp>();
                    //List<EmployeeDetailLog> employeeDetailsLog = new List<EmployeeDetailLog>();
                    foreach (var detailDto in employeeCreate.detail)
                    {
                        var employeeDetaillogtemp = _mapper.Map<EmployeeDetailLogTemp>(detailDto);
                        employeeDetaillogtemp.EmployeeTempId = employeeMapLogTemp.Id;
                        employeeDetaillogtemp.UpdatedBy = employeeMapLogTemp.UpdatedBy;
                        employeeDetaillogtemp.Action = "C";
                        employeeDetaillogtemp.ActionDetail = "C";
                        //employeeDetailsLogTemp.Add(employeeDetaillogtemp);

                        if (!_employeerepository.CreateEmployeeDetailLogTemp(employeeDetaillogtemp))
                        {
                            ModelState.AddModelError("", "Something went wrong while saving");
                            return StatusCode(500, ModelState);
                        }

                        var employeeDetaillog = _mapper.Map<EmployeeDetailLog>(detailDto);
                        employeeDetaillog.EmployeeTempId = employeeMapLogTemp.Id;
                        employeeDetaillog.EmployeeDetailTempId = employeeDetaillogtemp.Id;
                        employeeDetaillog.UpdatedBy = employeeMapLogTemp.UpdatedBy;
                        employeeDetaillog.Action = "C";
                        employeeDetaillog.ActionDetail = "C";
                        //employeeDetailsLog.Add(employeeDetaillog);

                        if (!_employeerepository.CreateEmployeeDetailLog(employeeDetaillog))
                        {
                            ModelState.AddModelError("", "Something went wrong while saving");
                            return StatusCode(500, ModelState);
                        }
                    }
                    //==================== D E T A I L ==============================

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

        [HttpDelete("{employeeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteEmployee(int employeeId)
        {
            if (!_employeerepository.EmployeeExists(employeeId))
            {
                return NotFound();
            }

            //var employeestemp = _employeerepository.GetPendingEmployee_all()
            //.Where(c => c.EmployeeID == employeeId && c.Action == "D")
            //.FirstOrDefault();

            //if (employeestemp != null)
            //{
            //    ModelState.AddModelError("", "Employee already exists. Need approve");
            //    return StatusCode(409, ModelState);
            //}

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var employees = _employeerepository.GetEmployee(employeeId);

                    //==================== H E A D E R ==============================
                    var employeeMap = _mapper.Map<Employee>(employees);
                    employeeMap.Action = "D";
                    employeeMap.Status = "NA";
                    if (!_employeerepository.UpdateEmployee(employeeMap))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    var employeeMapLogTemp = _mapper.Map<EmployeeLogTemp>(employeeMap);
                    employeeMapLogTemp.Id = 0;
                    employeeMapLogTemp.EmployeeId = employeeId;
                    employeeMapLogTemp.Action = "D";
                    employeeMapLogTemp.Status = "NA";
                    employeeMapLogTemp.Remarks = "";
                    if (!_employeerepository.CreateEmployeeHeaderLogTemp(employeeMapLogTemp))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log temp");
                        return StatusCode(500, ModelState);
                    }

                    var employeeMapLog = _mapper.Map<EmployeeLog>(employeeMap);
                    employeeMapLog.Id = 0;
                    employeeMapLog.EmployeeId = employeeId;
                    employeeMapLog.EmployeeTempId = employeeMapLogTemp.Id;
                    employeeMapLog.Action = "D";
                    employeeMapLog.Status = "NA";
                    employeeMapLog.Remarks = "";
                    if (!_employeerepository.CreateEmployeeHeaderLog(employeeMapLog))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log");
                        return StatusCode(500, ModelState);
                    }
                    //==================== H E A D E R ==============================

                    //==================== D E T A I L ==============================
                    var employeesdetail = _mapper.Map<List<EmployeeDetail>>(_employeerepository.GetEmployeesDetail(employees.Id));

                    foreach (var details in employeesdetail)
                    {
                        var employeesDetailMap = _mapper.Map<EmployeeDetail>(details);
                        employeesDetailMap.Action = "D";
                        employeesDetailMap.ActionDetail = "D";
                        if (!_employeerepository.UpdateEmployeeDetail(employeesDetailMap))
                        {
                            ModelState.AddModelError("", "Something went wrong while saving");
                            return StatusCode(500, ModelState);
                        }

                        var employeesDetailMapLogTemp = _mapper.Map<EmployeeDetailLogTemp>(details);
                        employeesDetailMapLogTemp.Id = 0;
                        employeesDetailMapLogTemp.EmployeeId = employeeId;
                        employeesDetailMapLogTemp.EmployeeTempId = employeeMapLogTemp.Id;
                        employeesDetailMapLogTemp.EmployeeDetailId = employeesDetailMap.Id;
                        employeesDetailMapLogTemp.Action = "D";
                        employeesDetailMapLogTemp.ActionDetail = "D";
                        if (!_employeerepository.CreateEmployeeDetailLogTemp(employeesDetailMapLogTemp))
                        {
                            ModelState.AddModelError("", "Something went wrong while saving");
                            return StatusCode(500, ModelState);
                        }

                        var employeesDetailMapLog = _mapper.Map<EmployeeDetailLog>(details);
                        employeesDetailMapLog.Id = 0;
                        employeesDetailMapLog.EmployeeId = employeeId;
                        employeesDetailMapLogTemp.EmployeeTempId = employeeMapLogTemp.Id;
                        employeesDetailMapLog.EmployeeDetailId = employeesDetailMap.Id;
                        employeesDetailMapLog.EmployeeDetailTempId = employeesDetailMapLogTemp.Id;
                        employeesDetailMapLog.Action = "D";
                        employeesDetailMapLog.ActionDetail = "D";
                        if (!_employeerepository.CreateEmployeeDetailLog(employeesDetailMapLog))
                        {
                            ModelState.AddModelError("", "Something went wrong while saving");
                            return StatusCode(500, ModelState);
                        }
                    }
                    //==================== D E T A I L ==============================

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

        [HttpPut("{employeeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateEmployee(int employeeId, [FromBody] EmployeeResultDto updatedEmployee)
        {
            var employees = _employeerepository.GetEmployee(employeeId);
            //==================== H E A D E R ==============================
            var employeeMap = _mapper.Map<Employee>(employees);
            employeeMap.Action = "U";
            employeeMap.Status = "NA";
            if (!_employeerepository.UpdateEmployee(employeeMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //==================== H E A D E R ==============================
                    var employeeMapLogTemp = _mapper.Map<EmployeeLogTemp>(updatedEmployee);
                    employeeMapLogTemp.Id = 0;
                    employeeMapLogTemp.EmployeeId = employeeId;
                    employeeMapLogTemp.Action = "U";
                    employeeMapLogTemp.Status = "NA";
                    employeeMapLogTemp.Remarks = "";
                    if (!_employeerepository.CreateEmployeeHeaderLogTemp(employeeMapLogTemp))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log temp");
                        return StatusCode(500, ModelState);
                    }

                    var employeeMapLog = _mapper.Map<EmployeeLog>(updatedEmployee);
                    employeeMapLog.Id = 0;
                    employeeMapLog.EmployeeId = employeeId;
                    employeeMapLog.EmployeeTempId = employeeMapLogTemp.Id;
                    employeeMapLog.Action = "U";
                    employeeMapLog.Status = "NA";
                    employeeMapLog.Remarks = "";
                    if (!_employeerepository.CreateEmployeeHeaderLog(employeeMapLog))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving log");
                        return StatusCode(500, ModelState);
                    }

                    //==================== H E A D E R ==============================

                    //==================== D E T A I L ==============================
                    List<EmployeeDetailLogTemp> employeeDetailsLogTemp = new List<EmployeeDetailLogTemp>();
                    List<EmployeeDetailLog> employeeDetailsLog = new List<EmployeeDetailLog>();
                    foreach (var detailDto in updatedEmployee.detail)
                    {
                        if (detailDto.Id == 0)
                        {
                            var employeeDetaillogtemp = _mapper.Map<EmployeeDetailLogTemp>(detailDto);
                            employeeDetaillogtemp.EmployeeTempId = employeeMapLogTemp.Id;
                            employeeDetaillogtemp.UpdatedBy = employeeMapLogTemp.UpdatedBy;
                            employeeDetaillogtemp.Action = "C";
                            employeeDetaillogtemp.ActionDetail = "C";
                            employeeDetailsLogTemp.Add(employeeDetaillogtemp);

                            if (!_employeerepository.CreateEmployeeDetailLogTemp(employeeDetaillogtemp))
                            {
                                ModelState.AddModelError("", "Something went wrong while saving");
                                return StatusCode(500, ModelState);
                            }

                            var employeeDetaillog = _mapper.Map<EmployeeDetailLog>(detailDto);
                            employeeDetaillog.EmployeeTempId = employeeMapLogTemp.Id;
                            employeeDetaillog.EmployeeDetailTempId = employeeDetaillogtemp.Id;
                            employeeDetaillog.UpdatedBy = employeeMapLogTemp.UpdatedBy;
                            employeeDetaillog.Action = "C";
                            employeeDetaillog.ActionDetail = "C";
                            employeeDetailsLog.Add(employeeDetaillog);

                            if (!_employeerepository.CreateEmployeeDetailLog(employeeDetaillog))
                            {
                                ModelState.AddModelError("", "Something went wrong while saving");
                                return StatusCode(500, ModelState);
                            }
                        }
                        else
                        {
                            var employeesDetailMap = _mapper.Map<EmployeeDetail>(_employeerepository.GetEmployeeDetail(detailDto.Id));
                            employeesDetailMap.Action = "U";
                            employeesDetailMap.ActionDetail = detailDto.ActionDetail;
                            if (!_employeerepository.UpdateEmployeeDetail(employeesDetailMap))
                            {
                                ModelState.AddModelError("", "Something went wrong while saving");
                                return StatusCode(500, ModelState);
                            }

                            var employeesDetailMapLogTemp = _mapper.Map<EmployeeDetailLogTemp>(employeesDetailMap);
                            employeesDetailMapLogTemp.Id = 0;
                            employeesDetailMapLogTemp.EmployeeId = employeeId;
                            employeesDetailMapLogTemp.EmployeeTempId = employeeMapLogTemp.Id;
                            employeesDetailMapLogTemp.EmployeeDetailId = employeesDetailMap.Id;
                            employeesDetailMapLogTemp.Action = "U";
                            employeesDetailMapLogTemp.ActionDetail = detailDto.ActionDetail;
                            if (!_employeerepository.CreateEmployeeDetailLogTemp(employeesDetailMapLogTemp))
                            {
                                ModelState.AddModelError("", "Something went wrong while saving");
                                return StatusCode(500, ModelState);
                            }

                            var employeesDetailMapLog = _mapper.Map<EmployeeDetailLog>(employeesDetailMap);
                            employeesDetailMapLog.Id = 0;
                            employeesDetailMapLog.EmployeeId = employeeId;
                            employeesDetailMapLogTemp.EmployeeTempId = employeeMapLogTemp.Id;
                            employeesDetailMapLog.EmployeeDetailId = employeesDetailMap.Id;
                            employeesDetailMapLog.EmployeeDetailTempId = employeesDetailMapLogTemp.Id;
                            employeesDetailMapLog.Action = "U";
                            employeesDetailMapLog.ActionDetail = detailDto.ActionDetail;
                            if (!_employeerepository.CreateEmployeeDetailLog(employeesDetailMapLog))
                            {
                                ModelState.AddModelError("", "Something went wrong while saving");
                                return StatusCode(500, ModelState);
                            }
                        }
                    }
                    //==================== D E T A I L ==============================
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

        [HttpPost("approvalEmployee/")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult ApprovalEmployees([FromBody] StatusIdList statusIdList)
        {
            var response = "";
            int total_data_failed = 0;
            int total_data_success = 0;

            try
            {
                //var auth_id_hash = _authrepository.Getid2(statusIdList.UserName);

                //if (auth_id_hash == null)
                //{
                //    //throw new Exception("username or password invalid");
                //    ModelState.AddModelError("", "username or password invalid");
                //    return StatusCode(401, ModelState);
                //}

                //var result = _passwordHasher.Verify(auth_id_hash.Password, statusIdList.Password);

                //if (!result)
                //{
                //    //throw new Exception("username or password invalid");
                //    ModelState.AddModelError("", "username or password invalid");
                //    return StatusCode(401, ModelState);
                //}

                if (statusIdList.Approval == "A")
                {
                    foreach (var id in statusIdList.IdList)
                    {
                        int _flag = 0;
                        if (_employeerepository.EmployeeTempExists(Convert.ToInt32(id)))
                        {
                            var employeeLogTempToDelete = _employeerepository.GetEmployeeLogTemp(Convert.ToInt32(id));
                            var employeesdetailLogTempDelete = _mapper.Map<List<EmployeeDetailLogTemp>>(_employeerepository.GetEmployeeDetailLogTemp_employeeid(employeeLogTempToDelete.Id));
                            if (employeeLogTempToDelete != null)
                            {
                                if (employeeLogTempToDelete.Action == "C" || employeeLogTempToDelete.Action == "U" || employeeLogTempToDelete.Action == "D")
                                {
                                    var updatedEmployee = new EmployeeDto();
                                    if (employeeLogTempToDelete.Action == "C")
                                    {
                                        var employeeMap = _mapper.Map<Employee>(updatedEmployee);
                                        //var employeeMap = _mapper.Map<Employee>(employeeLogTempToDelete);
                                        //==================== H E A D E R ==============================
                                        employeeMap = _mapper.Map<Employee>(employeeLogTempToDelete);
                                        employeeMap.Id = 0;
                                        employeeMap.Status = statusIdList.Approval;
                                        employeeMap.Action = "N";
                                        //employeeMap.UpdatedBy = employeeLogTempToDelete.UpdatedBy;
                                        //employeeMap.Remarks = employeeLogTempToDelete.Remarks;
                                        employeeMap.LastUpdate = DateTime.Now;

                                        if (!_employeerepository.CreateEmployeeHeader(employeeMap))
                                        {
                                            ModelState.AddModelError("", "Something went wrong while saving");
                                            return StatusCode(500, ModelState);
                                        }

                                        var employeeMapLog = _mapper.Map<EmployeeLog>(updatedEmployee);
                                        employeeMapLog = _mapper.Map<EmployeeLog>(employeeLogTempToDelete);
                                        employeeMapLog.Id = 0;
                                        employeeMapLog.EmployeeTempId = employeeLogTempToDelete.Id;
                                        employeeMapLog.EmployeeId = employeeMap.Id;
                                        employeeMapLog.Status = statusIdList.Approval;
                                        employeeMapLog.Action = "N";
                                        //employeeMapLog.UpdatedBy = employeeLogTempToDelete.UpdatedBy;
                                        //employeeMapLog.Remarks = employeeLogTempToDelete.Remarks;
                                        employeeMapLog.ActionRemarks = statusIdList.ActionRemarks;
                                        employeeMapLog.LastUpdate = DateTime.Now;

                                        if (!_employeerepository.CreateEmployeeHeaderLog(employeeMapLog))
                                        {
                                            ModelState.AddModelError("", "Something went wrong while saving");
                                            return StatusCode(500, ModelState);
                                        }
                                        //==================== H E A D E R ==============================

                                        //==================== D E T A I L ==============================
                                        List<EmployeeDetail> employeeDetails = new List<EmployeeDetail>();
                                        List<EmployeeDetailLog> employeeDetailsLog = new List<EmployeeDetailLog>();
                                        foreach (var details in employeesdetailLogTempDelete)
                                        {
                                            var employeeDetail = _mapper.Map<EmployeeDetail>(details);
                                            employeeDetail.Id = 0;
                                            employeeDetail.EmployeeId = employeeMap.Id;
                                            employeeDetail.UpdatedBy = details.UpdatedBy;
                                            employeeDetail.Action = "N";
                                            employeeDetail.ActionDetail = "N";
                                            employeeDetail.LastUpdate = DateTime.Now;
                                            employeeDetails.Add(employeeDetail);

                                            if (!_employeerepository.CreateEmployeeDetail(employeeDetail))
                                            {
                                                ModelState.AddModelError("", "Something went wrong while saving");
                                                return StatusCode(500, ModelState);
                                            }

                                            var employeeDetaillog = _mapper.Map<EmployeeDetailLog>(details);
                                            employeeDetaillog.Id = 0;
                                            employeeDetaillog.EmployeeId = employeeMap.Id;
                                            employeeDetaillog.EmployeeDetailId = employeeDetail.Id;
                                            employeeDetaillog.UpdatedBy = details.UpdatedBy;
                                            employeeDetaillog.Action = "N";
                                            employeeDetaillog.ActionDetail = "N";
                                            employeeDetailsLog.Add(employeeDetaillog);

                                            if (!_employeerepository.CreateEmployeeDetailLog(employeeDetaillog))
                                            {
                                                ModelState.AddModelError("", "Something went wrong while saving");
                                                return StatusCode(500, ModelState);
                                            }
                                        }
                                        //==================== D E T A I L ==============================

                                        response = "Successfully Approve";
                                        _flag = 1;
                                    }
                                    else if (employeeLogTempToDelete.Action == "D")
                                    {
                                        //==================== D E T A I L ==============================
                                        //List<EmployeeDetail> employeeDetails = new List<EmployeeDetail>();
                                        List<EmployeeDetailLog> employeeDetailsLog = new List<EmployeeDetailLog>();
                                        foreach (var details in employeesdetailLogTempDelete)
                                        {
                                            var getemployeeDetailToDelete = _employeerepository.GetEmployeeDetail_employeeid(details.EmployeeId);

                                            if (!_employeerepository.DeleteEmployeeDetail(getemployeeDetailToDelete))
                                            {
                                                ModelState.AddModelError("", "Something went wrong deleting employee");
                                            }

                                            var employeeDetaillog = _mapper.Map<EmployeeDetailLog>(details);
                                            employeeDetaillog.Id = 0;
                                            employeeDetaillog.EmployeeId = details.EmployeeId;
                                            employeeDetaillog.EmployeeDetailId = details.EmployeeDetailId;
                                            employeeDetaillog.EmployeeDetailTempId = details.Id;
                                            employeeDetaillog.UpdatedBy = details.UpdatedBy;
                                            employeeDetaillog.Action = "N";
                                            employeeDetaillog.ActionDetail = "N";
                                            employeeDetailsLog.Add(employeeDetaillog);

                                            if (!_employeerepository.CreateEmployeeDetailLog(employeeDetaillog))
                                            {
                                                ModelState.AddModelError("", "Something went wrong while saving");
                                                return StatusCode(500, ModelState);
                                            }
                                        }
                                        //==================== D E T A I L ==============================

                                        //==================== H E A D E R ==============================
                                        var getemployeeToDelete = _employeerepository.GetEmployee(employeeLogTempToDelete.EmployeeId);

                                        if (!_employeerepository.DeleteEmployee(getemployeeToDelete))
                                        {
                                            ModelState.AddModelError("", "Something went wrong deleting employee");
                                        }

                                        var employeeMapLog = _mapper.Map<EmployeeLog>(updatedEmployee);
                                        employeeMapLog = _mapper.Map<EmployeeLog>(employeeLogTempToDelete);
                                        employeeMapLog.Id = 0;
                                        employeeMapLog.EmployeeTempId = employeeLogTempToDelete.Id;
                                        employeeMapLog.EmployeeId = getemployeeToDelete.Id;
                                        //employeeMapLog.Name = employeeLogTempToDelete.Name;
                                        employeeMapLog.Status = statusIdList.Approval;
                                        employeeMapLog.Action = "N";
                                        employeeMapLog.UpdatedBy = employeeLogTempToDelete.UpdatedBy;
                                        employeeMapLog.Remarks = employeeLogTempToDelete.Remarks;
                                        employeeMapLog.ActionRemarks = statusIdList.ActionRemarks;

                                        if (!_employeerepository.CreateEmployeeHeaderLog(employeeMapLog))
                                        {
                                            ModelState.AddModelError("", "Something went wrong while saving");
                                            return StatusCode(500, ModelState);
                                        }
                                        //==================== H E A D E R ==============================
                                        response = "Successfully Approve";
                                        _flag = 1;
                                    }
                                    else if (employeeLogTempToDelete.Action == "U")
                                    {
                                        List<EmployeeDetail> employeeDetails = new List<EmployeeDetail>();
                                        List<EmployeeDetailLog> employeeDetailsLog = new List<EmployeeDetailLog>();
                                        //==================== D E T A I L ==============================
                                        foreach (var details in employeesdetailLogTempDelete)
                                        {
                                            if (details.ActionDetail == "U")
                                            {
                                                //var employeeDetail = _mapper.Map<EmployeeDetail>(details);
                                                var employeeDetail = _mapper.Map<EmployeeDetail>(_employeerepository.GetEmployeeDetail(details.EmployeeDetailId));
                                                employeeDetail.UpdatedBy = details.UpdatedBy;
                                                employeeDetail.Action = "N";
                                                employeeDetail.ActionDetail = "N";
                                                employeeDetails.Add(employeeDetail);

                                                if (!_employeerepository.UpdateEmployeeDetail(employeeDetail))
                                                {
                                                    ModelState.AddModelError("", "Something went wrong while saving");
                                                    return StatusCode(500, ModelState);
                                                }

                                                var employeeDetaillog = _mapper.Map<EmployeeDetailLog>(details);
                                                employeeDetaillog.Id = 0;
                                                employeeDetaillog.EmployeeId = details.EmployeeId;
                                                employeeDetaillog.EmployeeDetailId = details.EmployeeDetailId;
                                                employeeDetaillog.EmployeeDetailTempId = details.Id;
                                                employeeDetaillog.UpdatedBy = details.UpdatedBy;
                                                employeeDetaillog.Action = "N";
                                                employeeDetaillog.ActionDetail = "N";
                                                employeeDetailsLog.Add(employeeDetaillog);

                                                if (!_employeerepository.CreateEmployeeDetailLog(employeeDetaillog))
                                                {
                                                    ModelState.AddModelError("", "Something went wrong while saving");
                                                    return StatusCode(500, ModelState);
                                                }
                                            }
                                            else if (details.ActionDetail == "D")
                                            {
                                                var getemployeeDetailToDelete = _employeerepository.GetEmployeeDetail(details.EmployeeDetailId);

                                                if (!_employeerepository.DeleteEmployeeDetail(getemployeeDetailToDelete))
                                                {
                                                    ModelState.AddModelError("", "Something went wrong deleting employee");
                                                }

                                                var employeeDetaillog = _mapper.Map<EmployeeDetailLog>(details);
                                                employeeDetaillog.Id = 0;
                                                employeeDetaillog.EmployeeId = details.EmployeeId;
                                                employeeDetaillog.EmployeeDetailId = details.EmployeeDetailId;
                                                employeeDetaillog.EmployeeDetailTempId = details.Id;
                                                employeeDetaillog.UpdatedBy = details.UpdatedBy;
                                                employeeDetaillog.Action = "U";
                                                employeeDetaillog.ActionDetail = "D";
                                                employeeDetailsLog.Add(employeeDetaillog);

                                                if (!_employeerepository.CreateEmployeeDetailLog(employeeDetaillog))
                                                {
                                                    ModelState.AddModelError("", "Something went wrong while saving");
                                                    return StatusCode(500, ModelState);
                                                }
                                            }
                                            else if (details.ActionDetail == "C")
                                            {
                                                var employeeDetail = _mapper.Map<EmployeeDetail>(details);
                                                employeeDetail.EmployeeId = employeeLogTempToDelete.EmployeeId;
                                                employeeDetail.Id = 0;
                                                employeeDetail.UpdatedBy = details.UpdatedBy;
                                                employeeDetail.Action = "N";
                                                employeeDetail.ActionDetail = "N";
                                                employeeDetails.Add(employeeDetail);

                                                if (!_employeerepository.CreateEmployeeDetail(employeeDetail))
                                                {
                                                    ModelState.AddModelError("", "Something went wrong while saving");
                                                    return StatusCode(500, ModelState);
                                                }

                                                var employeeDetaillog = _mapper.Map<EmployeeDetailLog>(details);
                                                employeeDetaillog.Id = 0;
                                                employeeDetaillog.EmployeeId = employeeLogTempToDelete.EmployeeId;
                                                employeeDetaillog.EmployeeDetailId = employeeDetail.Id;
                                                employeeDetaillog.UpdatedBy = details.UpdatedBy;
                                                employeeDetaillog.Action = "N";
                                                employeeDetaillog.ActionDetail = "N";
                                                employeeDetailsLog.Add(employeeDetaillog);

                                                if (!_employeerepository.CreateEmployeeDetailLog(employeeDetaillog))
                                                {
                                                    ModelState.AddModelError("", "Something went wrong while saving");
                                                    return StatusCode(500, ModelState);
                                                }
                                            }

                                        }
                                        //==================== D E T A I L ==============================

                                        //==================== H E A D E R ==============================

                                        var employeeMap = _employeerepository.GetEmployee(employeeLogTempToDelete.EmployeeId);

                                        _mapper.Map(employeeLogTempToDelete, employeeMap);
                                        employeeMap.Id = employeeLogTempToDelete.EmployeeId;
                                        employeeMap.Action = "N";
                                        employeeMap.Status = "N";

                                        if (!_employeerepository.UpdateEmployee(employeeMap))
                                        {
                                            ModelState.AddModelError("", "Something went wrong while saving");
                                            return StatusCode(500, ModelState);
                                        }

                                        var employeeMapLog = _mapper.Map<EmployeeLog>(updatedEmployee);
                                        employeeMapLog = _mapper.Map<EmployeeLog>(employeeLogTempToDelete);
                                        employeeMapLog.Id=0;
                                        employeeMapLog.EmployeeTempId = employeeLogTempToDelete.Id;
                                        employeeMapLog.EmployeeId = employeeMap.Id;
                                        employeeMapLog.Status = statusIdList.Approval;
                                        employeeMapLog.Action = "N";
                                        employeeMapLog.UpdatedBy = employeeLogTempToDelete.UpdatedBy;
                                        employeeMapLog.Remarks = employeeLogTempToDelete.Remarks;
                                        employeeMapLog.ActionRemarks = statusIdList.ActionRemarks;

                                        if (!_employeerepository.CreateEmployeeHeaderLog(employeeMapLog))
                                        {
                                            ModelState.AddModelError("", "Something went wrong while saving");
                                            return StatusCode(500, ModelState);
                                        }
                                        //==================== H E A D E R ==============================

                                        response = "Successfully Approve";
                                        _flag = 1;
                                    }
                                }
                                if (_flag == 1)
                                {
                                    if (!ModelState.IsValid)
                                        return BadRequest(ModelState);

                                    foreach (var details in employeesdetailLogTempDelete)
                                    {
                                        if (!_employeerepository.DeleteEmployeeDetailLogTemp(details))
                                        {
                                            ModelState.AddModelError("", "Something went wrong deleting employee");
                                        }
                                    }

                                    if (!_employeerepository.DeleteEmployeeLogTemp(employeeLogTempToDelete))
                                    {
                                        ModelState.AddModelError("", "Something went wrong deleting employee");
                                    }
                                }
                            }
                            total_data_success = total_data_success + 1;
                        }
                    }
                }
                else if (statusIdList.Approval == "R")
                {
                    foreach (var id in statusIdList.IdList)
                    {
                        int _flag = 0;
                        if (_employeerepository.EmployeeTempExists(Convert.ToInt32(id)))
                        {
                            var employeeLogTempToDelete = _employeerepository.GetEmployeeLogTemp(Convert.ToInt32(id));
                            var employeesdetailLogTempDelete = _mapper.Map<List<EmployeeDetailLogTemp>>(_employeerepository.GetEmployeeDetailLogTemp_employeeid(employeeLogTempToDelete.Id));
                            if (employeeLogTempToDelete != null)
                            {
                                if (employeeLogTempToDelete.Action == "C" || employeeLogTempToDelete.Action == "U" || employeeLogTempToDelete.Action == "D")
                                {
                                    var updatedEmployee = new EmployeeDto();
                                    var employeeMap = _mapper.Map<EmployeeLogTemp>(employeeLogTempToDelete);
                                    //==================== H E A D E R ==============================
                                    employeeMap.Action = employeeLogTempToDelete.Action;
                                    employeeMap.Status = statusIdList.Approval;
                                    employeeMap.ActionRemarks = statusIdList.ActionRemarks;
                                    employeeMap.LastUpdate = DateTime.Now;

                                    if (!_employeerepository.UpdateEmployeeHeaderTemp(employeeMap))
                                    {
                                        ModelState.AddModelError("", "Something went wrong while saving");
                                        return StatusCode(500, ModelState);
                                    }

                                    var employeeMapLog = _mapper.Map<EmployeeLog>(employeeLogTempToDelete);
                                    employeeMapLog.Id = 0;
                                    employeeMapLog.EmployeeTempId = employeeLogTempToDelete.Id;
                                    employeeMapLog.Status = statusIdList.Approval;
                                    //employeeMapLog.Action = employeeLogTempToDelete.Action;
                                    employeeMapLog.ActionRemarks = statusIdList.ActionRemarks;
                                    employeeMapLog.LastUpdate = DateTime.Now;

                                    if (!_employeerepository.CreateEmployeeHeaderLog(employeeMapLog))
                                    {
                                        ModelState.AddModelError("", "Something went wrong while saving");
                                        return StatusCode(500, ModelState);
                                    }
                                    //==================== H E A D E R ==============================

                                    //==================== D E T A I L ==============================
                                    foreach (var details in employeesdetailLogTempDelete)
                                    {
                                        var employeeDetail = _mapper.Map<EmployeeDetailLogTemp>(details);
                                        //employeeDetail.Id = 0;
                                        employeeDetail.EmployeeId = employeeMap.Id;
                                        employeeDetail.UpdatedBy = details.UpdatedBy;
                                        //employeeDetail.Action = "N";
                                        //employeeDetail.ActionDetail = "N";
                                        employeeDetail.LastUpdate = DateTime.Now;

                                        if (!_employeerepository.UpdateEmployeeDetailLogTemp(employeeDetail))
                                        {
                                            ModelState.AddModelError("", "Something went wrong while saving");
                                            return StatusCode(500, ModelState);
                                        }

                                        var employeeDetaillog = _mapper.Map<EmployeeDetailLog>(details);
                                        employeeDetaillog.Id = 0;
                                        employeeDetaillog.EmployeeId = employeeMap.Id;
                                        employeeDetaillog.EmployeeDetailId = employeeDetail.Id;
                                        employeeDetaillog.UpdatedBy = details.UpdatedBy;
                                        //employeeDetaillog.Action = "N";
                                        //employeeDetaillog.ActionDetail = "N";

                                        if (!_employeerepository.CreateEmployeeDetailLog(employeeDetaillog))
                                        {
                                            ModelState.AddModelError("", "Something went wrong while saving");
                                            return StatusCode(500, ModelState);
                                        }
                                    }
                                    //==================== D E T A I L ==============================

                                    response = "Successfully Reject";
                                    _flag = 1;
                                }
                            }
                            total_data_success = total_data_success + 1;
                        }
                    }
                }
                response = response + ". Data success : " + total_data_success + ". Data failed : " + total_data_failed + ".";
                return Ok(response);
            }
            catch (Exception ex)
            {
                //dbContextTransaction.Rollback();
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");

                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("InnerExceptionMessage", $"Inner Exception: {ex.InnerException.Message}");
                }

                return StatusCode(500, ModelState);
            }
        }

        [HttpGet("PendingEmployee/")]
        //[Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Employee>))]
        public IActionResult GetPendingEmployees([FromQuery] int limit, [FromQuery] int page, [FromQuery] string sortby, [FromQuery] string sortdesc, [FromQuery] string? keyword)
        {
            var pendingemployees = _mapper.Map<List<EmployeeJoinDtoPending>>(_employeerepository.GetPendingEmployees(limit, page, sortby, sortdesc, keyword));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var count = 0;

            if (keyword == "")
            {
                count = _employeerepository.Getpendingdatacount();
            }
            else
            {
                count = _employeerepository.GetEmployeesAdvancePending(keyword).Count;
            }

            var response = ApiResponseHelper.StdApiResponse("Data load success", pendingemployees, datacount: count);

            return Ok(response);
        }

        [HttpGet("PendingEmployee/{pendingId}")]
        //[Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Employee>))]
        public IActionResult GetPendingEmployee(int pendingId)
        {
            if (!_employeerepository.EmployeeTempExists(pendingId))
            {
                return NotFound();
            }

            var employees = _mapper.Map<EmployeeJoinDtoPending>(_employeerepository.GetEmployeesJoinPending(pendingId));

            var employeesdetail = _mapper.Map<List<EmployeeDetailJoinDtoPending>>(_employeerepository.GetEmployeesDetailJoinPending(employees.Id));

            var result = _mapper.Map<EmployeeResultJoinDtoPending>(employees);
            result.Id = employees.Id;
            result.Name = employees.Name;
            result.detail = employeesdetail;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(result);
        }

        [HttpDelete("deleteEmployeeTemp/{pendingId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteEmployeeTemp(int pendingId, [FromBody] ActionRemark Actremark)
        {
            if (!_employeerepository.EmployeeTempExists(pendingId))
            {
                return NotFound();
            }

            var response = "";
            var employeeLogTempToDelete = _employeerepository.GetEmployeeLogTemp(pendingId);

            var employeeMapLog = _mapper.Map<EmployeeLog>(employeeLogTempToDelete);
            employeeMapLog.Id = 0;
            employeeMapLog.EmployeeTempId = employeeLogTempToDelete.Id;
            employeeMapLog.ActionRemarks = Actremark.ActionRemarks;
            if (!_employeerepository.CreateEmployeeHeaderLog(employeeMapLog))
            {
                ModelState.AddModelError("", "Something went wrong while saving log");
                return StatusCode(500, ModelState);
            }

            if (_employeerepository.EmployeeExists(employeeLogTempToDelete.EmployeeId))
            {
                var employees = _employeerepository.GetEmployee(employeeLogTempToDelete.EmployeeId);

                var branchMap = _mapper.Map<Employee>(employees);
                branchMap.Action = "N";
                branchMap.Status = "A";
                if (!_employeerepository.UpdateEmployee(branchMap))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
            }

            var employeesdetailLogTempDelete = _mapper.Map<List<EmployeeDetailLogTemp>>(_employeerepository.GetEmployeeDetailLogTemp_(employeeLogTempToDelete.Id));

            List<EmployeeDetailLog> employeeDetailsLog = new List<EmployeeDetailLog>();
            foreach (var details in employeesdetailLogTempDelete)
            {
                var getemployeeDetailToDelete = _employeerepository.GetEmployeeDetailLogTemp(details.Id);

                if (!_employeerepository.DeleteEmployeeDetailLogTemp(getemployeeDetailToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong deleting employee");
                }

                var employeeDetaillog = _mapper.Map<EmployeeDetailLog>(details);
                employeeDetaillog.Id = 0;
                employeeDetaillog.EmployeeId = details.EmployeeId;
                employeeDetaillog.EmployeeDetailId = details.EmployeeDetailId;
                employeeDetaillog.EmployeeDetailTempId = details.Id;
                employeeDetaillog.UpdatedBy = details.UpdatedBy;
                employeeDetaillog.Action = "N";
                employeeDetaillog.ActionDetail = "N";
                employeeDetailsLog.Add(employeeDetaillog);

                if (!_employeerepository.CreateEmployeeDetailLog(employeeDetaillog))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }

                if (_employeerepository.EmployeeDetailExists(employeeLogTempToDelete.EmployeeId))
                {
                    var employees = _employeerepository.GetEmployeeDetail(employeeLogTempToDelete.EmployeeId);

                    var branchMap = _mapper.Map<EmployeeDetail>(employees);
                    branchMap.Action = "N";
                    branchMap.ActionDetail = "A";
                    if (!_employeerepository.UpdateEmployeeDetail(branchMap))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }
                }
            }

            //DELETE TEMP
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_employeerepository.DeleteEmployeeLogTemp(employeeLogTempToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting employee temp");
                return StatusCode(500, ModelState);
            }
            //DELETE TEMP
            response = "Successfully Deleted Pending Employee";
            return Ok(response);
            //return NoContent();
        }

        [HttpPut("updateEmployeeTemp/{pendingId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateEmployeeTemp(int pendingId, [FromBody] EmployeeResultJoinDtoPendingUpdate updatedTempEmployee)
        {
            if (!_employeerepository.EmployeeTempExists(pendingId))
            {
                return NotFound();
            }

            var employeeLogTempToEdit = _employeerepository.GetEmployeeLogTemp(pendingId);
            var response = "";

            //==================== H E A D E R ==============================

            var employeeMap = _mapper.Map<EmployeeLogTemp>(employeeLogTempToEdit);
            employeeMap.Name = updatedTempEmployee.Name;
            employeeMap.Remarks = updatedTempEmployee.Remarks;
            employeeMap.ActionRemarks = updatedTempEmployee.ActionRemarks;
            employeeMap.Action = updatedTempEmployee.Action;
            employeeMap.Status = "NA";

            if (!_employeerepository.UpdateEmployeeHeaderTemp(employeeMap))
            {
                ModelState.AddModelError("", "Something went wrong deleting employee");
            }

            var employeeMapLog = _mapper.Map<EmployeeLog>(employeeLogTempToEdit);
            employeeMapLog.Id = 0;
            employeeMapLog.EmployeeTempId = employeeLogTempToEdit.Id;
            employeeMapLog.ActionRemarks = updatedTempEmployee.ActionRemarks;

            if (!_employeerepository.CreateEmployeeHeaderLog(employeeMapLog))
            {
                ModelState.AddModelError("", "Something went wrong while saving log");
                return StatusCode(500, ModelState);
            }

            //==================== H E A D E R ==============================

            //==================== D E T A I L ==============================

            var employeesdetailLogTempEdit = _mapper.Map<List<EmployeeDetailLogTemp>>(_employeerepository.GetEmployeeDetailLogTemp_(employeeLogTempToEdit.Id));

            List<EmployeeDetailLogTemp> employeeDetailsLogTemp = new List<EmployeeDetailLogTemp>();
            List<EmployeeDetailLog> employeeDetailsLog = new List<EmployeeDetailLog>();

            foreach (var details in updatedTempEmployee.detail)
            {
                var getemployeeDetailToEdit = _employeerepository.GetEmployeeDetailLogTemp(details.Id);

                var employeeMapDetailTemp = _mapper.Map<EmployeeDetailLogTemp>(getemployeeDetailToEdit);
                employeeMapDetailTemp.LicenseId = details.LicenseId;
                employeeMapDetailTemp.Action = details.Action;
                employeeMapDetailTemp.ActionRemarks = details.ActionRemarks;
                employeeMapDetailTemp.Remarks = "NA";

                if (!_employeerepository.UpdateEmployeeDetailLogTemp(getemployeeDetailToEdit))
                {
                    ModelState.AddModelError("", "Something went wrong deleting employee");
                }

                var employeeDetaillog = _mapper.Map<EmployeeDetailLog>(details);
                employeeDetaillog.Id = 0;
                employeeDetaillog.EmployeeId = details.EmployeeId;
                employeeDetaillog.EmployeeDetailId = details.EmployeeDetailId;
                employeeDetaillog.EmployeeDetailTempId = details.Id;
                employeeDetaillog.UpdatedBy = details.UpdatedBy;
                employeeDetaillog.Action = "N";
                employeeDetaillog.ActionDetail = "N";
                employeeDetailsLog.Add(employeeDetaillog);

                if (!_employeerepository.CreateEmployeeDetailLog(employeeDetaillog))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
            }

            //==================== D E T A I L ==============================

            return Ok(response);
        }

        /*
        [HttpPost("approvalEmployee/")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult ApprovalEmployees([FromBody] StatusIdList statusIdList)
        {
            int total_data_failed = 0;
            int total_data_success = 0;
            try
            {
                if (statusIdList.Approval == "A")
                {
                    foreach (var id in statusIdList.IdList)
                    {
                        if (_employeerepository.EmployeeTempExists(Convert.ToInt32(id)))
                        {
                            var employeeLogTempToDelete = _employeerepository.GetEmployeeLogTemp(Convert.ToInt32(id));
                            var employeeDetailLogTempToDelete = _employeerepository.GetEmployeeDetailLogTemp_employeeid(employeeLogTempToDelete.Id);

                            if (employeeLogTempToDelete != null)
                            {
                                var updateResult = HandleEmployeeUpdate(statusIdList, employeeLogTempToDelete, employeeDetailLogTempToDelete.ToList());

                                if (updateResult)
                                {
                                    total_data_success++;
                                }
                                else
                                {
                                    total_data_failed++;
                                }
                            }
                        }
                    }
                }
                else if (statusIdList.Approval == "R")
                {

                }
                string response = $"Successfully processed approval. Data success: {total_data_success}. Data failed: {total_data_failed}.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");

                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("InnerExceptionMessage", $"Inner Exception: {ex.InnerException.Message}");
                }

                return StatusCode(500, ModelState);
            }
        }

        private bool HandleEmployeeUpdate(StatusIdList statusIdList, EmployeeLogTemp employeeLogTemp, List<EmployeeDetailLogTemp> employeeDetailLogTemps)
        {
            try
            {
                if (employeeLogTemp.Action == "C")
                {
                    return CreateEmployee(statusIdList, employeeLogTemp, employeeDetailLogTemps);
                }
                else if (employeeLogTemp.Action == "U")
                {
                    return UpdateEmployee(statusIdList, employeeLogTemp, employeeDetailLogTemps);
                }
                else if (employeeLogTemp.Action == "D")
                {
                    return DeleteEmployee(statusIdList, employeeLogTemp, employeeDetailLogTemps);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while processing action {employeeLogTemp.Action}: {ex.Message}");
            }

            return false;
        }

        private bool CreateEmployee(StatusIdList statusIdList, EmployeeLogTemp employeeLogTemp, List<EmployeeDetailLogTemp> employeeDetailLogTemps)
        {
            var employee = _mapper.Map<Employee>(employeeLogTemp);
            employee.Id = 0;
            employee.Status = statusIdList.Approval;
            employee.Action = "N";
            employee.LastUpdate = DateTime.Now;

            if (!_employeerepository.CreateEmployeeHeader(employee))
            {
                ModelState.AddModelError("", "Something went wrong while creating employee header.");
                return false;
            }

            var employeeLog = _mapper.Map<EmployeeLog>(employeeLogTemp);
            employeeLog.Id = 0;
            employeeLog.EmployeeTempId = employeeLogTemp.Id;
            employeeLog.EmployeeId = employee.Id;
            employeeLog.Status = statusIdList.Approval;
            employeeLog.Action = "N";
            employeeLog.ActionRemarks = statusIdList.ActionRemarks;
            employeeLog.LastUpdate = DateTime.Now;

            if (!_employeerepository.CreateEmployeeHeaderLog(employeeLog))
            {
                ModelState.AddModelError("", "Something went wrong while creating employee log.");
                return false;
            }

            foreach (var detailLog in employeeDetailLogTemps)
            {
                var employeeDetail = _mapper.Map<EmployeeDetail>(detailLog);
                employeeDetail.Id = 0;
                employeeDetail.EmployeeId = employee.Id;
                employeeDetail.LastUpdate = DateTime.Now;

                if (!_employeerepository.CreateEmployeeDetail(employeeDetail))
                {
                    ModelState.AddModelError("", "Something went wrong while creating employee detail.");
                    return false;
                }

                var employeeDetailLog = _mapper.Map<EmployeeDetailLog>(detailLog);
                employeeDetailLog.Id = 0;
                employeeDetailLog.EmployeeId = employee.Id;
                employeeDetailLog.EmployeeDetailId = employeeDetail.Id;
                employeeDetailLog.LastUpdate = DateTime.Now;

                if (!_employeerepository.CreateEmployeeDetailLog(employeeDetailLog))
                {
                    ModelState.AddModelError("", "Something went wrong while creating employee detail log.");
                    return false;
                }
            }

            _employeerepository.DeleteEmployeeLogTemp(employeeLogTemp);
            foreach (var detailLog in employeeDetailLogTemps)
            {
                _employeerepository.DeleteEmployeeDetailLogTemp(detailLog);
            }

            return true;
        }

        private bool UpdateEmployee(StatusIdList statusIdList, EmployeeLogTemp employeeLogTemp, List<EmployeeDetailLogTemp> employeeDetailLogTemps)
        {
            var existingEmployee = _employeerepository.GetEmployee(employeeLogTemp.EmployeeId);

            if (existingEmployee == null)
            {
                ModelState.AddModelError("", "Employee not found.");
                return false;
            }

            _mapper.Map(employeeLogTemp, existingEmployee);
            existingEmployee.Id = employeeLogTemp.EmployeeId;
            existingEmployee.Action = "N";
            existingEmployee.Status = "N";

            try
            {
                if (!_employeerepository.UpdateEmployee(existingEmployee))
                {
                    ModelState.AddModelError("", "Something went wrong while updating employee.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while processing action {employeeLogTemp.Action}: {ex.Message}");
                return false;
            }


            foreach (var detailLog in employeeDetailLogTemps)
            {
                if (detailLog.ActionDetail == "U")
                {
                    var existingDetail = _employeerepository.GetEmployeeDetail(detailLog.EmployeeDetailId);

                    if (existingDetail == null)
                    {
                        ModelState.AddModelError("", "Employee detail not found.");
                        return false;
                    }

                    _mapper.Map(detailLog, existingDetail);
                    existingDetail.Action = "N";
                    existingDetail.ActionDetail = "N";

                    if (!_employeerepository.UpdateEmployeeDetail(existingDetail))
                    {
                        ModelState.AddModelError("", "Something went wrong while updating employee detail.");
                        return false;
                    }
                }
                else if (detailLog.ActionDetail == "D")
                {
                    var existingDetail = _employeerepository.GetEmployeeDetail(detailLog.EmployeeDetailId);

                    if (existingDetail == null)
                    {
                        ModelState.AddModelError("", "Employee detail not found.");
                        return false;
                    }

                    if (!_employeerepository.DeleteEmployeeDetail(existingDetail))
                    {
                        ModelState.AddModelError("", "Something went wrong while deleting employee detail.");
                        return false;
                    }
                }
                else if (detailLog.ActionDetail == "C")
                {
                    var newDetail = _mapper.Map<EmployeeDetail>(detailLog);
                    newDetail.EmployeeId = employeeLogTemp.EmployeeId;
                    newDetail.Id = 0;
                    newDetail.LastUpdate = DateTime.Now;

                    if (!_employeerepository.CreateEmployeeDetail(newDetail))
                    {
                        ModelState.AddModelError("", "Something went wrong while creating employee detail.");
                        return false;
                    }
                }

                var employeeDetailLog = _mapper.Map<EmployeeDetailLog>(detailLog);
                employeeDetailLog.Id = 0;
                employeeDetailLog.EmployeeId = employeeLogTemp.EmployeeId;
                employeeDetailLog.LastUpdate = DateTime.Now;

                if (!_employeerepository.CreateEmployeeDetailLog(employeeDetailLog))
                {
                    ModelState.AddModelError("", "Something went wrong while creating employee detail log.");
                    return false;
                }
            }

            var employeeLog = _mapper.Map<EmployeeLog>(employeeLogTemp);
            employeeLog.Id = 0;
            employeeLog.EmployeeTempId = employeeLogTemp.Id;
            employeeLog.EmployeeId = existingEmployee.Id;
            employeeLog.Status = statusIdList.Approval;
            employeeLog.Action = "N";
            employeeLog.ActionRemarks = statusIdList.ActionRemarks;
            employeeLog.LastUpdate = DateTime.Now;

            if (!_employeerepository.CreateEmployeeHeaderLog(employeeLog))
            {
                ModelState.AddModelError("", "Something went wrong while creating employee log.");
                return false;
            }

            _employeerepository.DeleteEmployeeLogTemp(employeeLogTemp);
            foreach (var detailLog in employeeDetailLogTemps)
            {
                _employeerepository.DeleteEmployeeDetailLogTemp(detailLog);
            }

            return true;
        }

        private bool DeleteEmployee(StatusIdList statusIdList, EmployeeLogTemp employeeLogTemp, List<EmployeeDetailLogTemp> employeeDetailLogTemps)
        {
            var existingEmployee = _employeerepository.GetEmployee(employeeLogTemp.EmployeeId);

            if (existingEmployee == null)
            {
                ModelState.AddModelError("", "Employee not found.");
                return false;
            }

            foreach (var detailLog in employeeDetailLogTemps)
            {
                var existingDetail = _employeerepository.GetEmployeeDetail(detailLog.EmployeeDetailId);

                if (existingDetail == null)
                {
                    ModelState.AddModelError("", "Employee detail not found.");
                    return false;
                }

                if (!_employeerepository.DeleteEmployeeDetail(existingDetail))
                {
                    ModelState.AddModelError("", "Something went wrong while deleting employee detail.");
                    return false;
                }

                var employeeDetailLog = _mapper.Map<EmployeeDetailLog>(detailLog);
                employeeDetailLog.Id = 0;
                employeeDetailLog.EmployeeId = employeeLogTemp.EmployeeId;
                employeeDetailLog.LastUpdate = DateTime.Now;

                if (!_employeerepository.CreateEmployeeDetailLog(employeeDetailLog))
                {
                    ModelState.AddModelError("", "Something went wrong while creating employee detail log.");
                    return false;
                }
            }

            if (!_employeerepository.DeleteEmployee(existingEmployee))
            {
                ModelState.AddModelError("", "Something went wrong while deleting employee.");
                return false;
            }

            var employeeLog = _mapper.Map<EmployeeLog>(employeeLogTemp);
            employeeLog.Id = 0;
            employeeLog.EmployeeTempId = employeeLogTemp.Id;
            employeeLog.EmployeeId = existingEmployee.Id;
            employeeLog.Status = statusIdList.Approval;
            employeeLog.Action = "N";
            employeeLog.ActionRemarks = statusIdList.ActionRemarks;
            employeeLog.LastUpdate = DateTime.Now;

            if (!_employeerepository.CreateEmployeeHeaderLog(employeeLog))
            {
                ModelState.AddModelError("", "Something went wrong while creating employee log.");
                return false;
            }

            _employeerepository.DeleteEmployeeLogTemp(employeeLogTemp);
            foreach (var detailLog in employeeDetailLogTemps)
            {
                _employeerepository.DeleteEmployeeDetailLogTemp(detailLog);
            }

            return true;
        }
        */
    }
}
