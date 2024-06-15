using API_Dinamis.Dto;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using API_Dinamis.Repository;
using API_Dinamis.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection;

namespace API_Dinamis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _rolerepository;
        private readonly IMapper _mapper;
        private readonly IAuthRepository _authrepository;
        private readonly IPasswordHasher _passwordHasher;

        public RoleController(IRoleRepository roleRepository, IMapper mapper, IAuthRepository authrepository, IPasswordHasher passwordHasher)
        {
            _rolerepository = roleRepository;
            _mapper = mapper;
            _authrepository = authrepository;
            _passwordHasher = passwordHasher;
        }

        //[HttpGet("AllRole")]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<Role>))]
        //public IActionResult GetRole_All()
        //{
        //    var roles = _mapper.Map<List<RoleDto>>(_rolerepository.GetRoles_all());

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var response = ApiResponseHelper.StdApiResponse("Data load success", roles, datacount: roles.Count);

        //    return Ok(response);
        //}

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Role>))]
        public IActionResult GetRole([FromQuery] int? limit = 0, [FromQuery] int? page = 1, [FromQuery] string? sortby = "d", [FromQuery] string? sortdesc = "id", [FromQuery] string? keyword = "")
        {
            var roles = _mapper.Map<List<RoleDtoForm>>(_rolerepository.GetRoles(limit, page, sortby, sortdesc, keyword));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var count = 0;

            if (keyword == "")
            {
                count = _rolerepository.Getdatacount();
            }
            else
            {
                count = _rolerepository.GetRolesAdvance(keyword).Count;
            }

            var response = ApiResponseHelper.StdApiResponse("Data load success", roles, datacount: count);

            return Ok(response);
        }

        [HttpGet("{roleId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Role>))]
        [ProducesResponseType(400)]
        public IActionResult GetRole(int roleId)
        {
            var roles = _mapper.Map<RoleJoinDto>(_rolerepository.GetRolesJoin(roleId));

            var rolesdetail = _mapper.Map<List<RoleDetailJoinDto>>(_rolerepository.GetRolesDetailJoin(roles.Id));

            var result = _mapper.Map<RoleResultJoinDto>(roles);
            result.Id = roles.Id;
            result.Name = roles.Name;
            result.ModuleId = roles.ModuleId;
            result.ModuleName = roles.ModuleName;
            result.detail = rolesdetail;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateRole([FromBody] RoleResultDto RoleCreate)
        {
            if (RoleCreate == null)
                return BadRequest(ModelState);

            //==================== H E A D E R ==============================
            var roleMapLogTemp = _mapper.Map<RoleLogTemp>(RoleCreate);
            roleMapLogTemp.Action = "C";
            roleMapLogTemp.Status = "NA";

            if (!_rolerepository.CreateRoleHeaderLogTemp(roleMapLogTemp))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            var roleMapLog = _mapper.Map<RoleLog>(RoleCreate);
            roleMapLog.RoleTempId = roleMapLogTemp.Id;
            roleMapLog.Action = "C";
            roleMapLog.Status = "NA";
            if (!_rolerepository.CreateRoleHeaderLog(roleMapLog))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            //==================== H E A D E R ==============================


            //==================== D E T A I L ==============================
            List<RoleDetailLogTemp> roleDetailsLogTemp = new List<RoleDetailLogTemp>();
            List<RoleDetailLog> roleDetailsLog = new List<RoleDetailLog>();
            foreach (var detailDto in RoleCreate.detail)
            {
                var roleDetaillogtemp = _mapper.Map<RoleDetailLogTemp>(detailDto);
                roleDetaillogtemp.RoleTempId = roleMapLogTemp.Id;
                roleDetaillogtemp.UpdatedBy = roleMapLogTemp.UpdatedBy;
                roleDetaillogtemp.Action = "C";
                roleDetaillogtemp.ActionDetail = "C";
                roleDetailsLogTemp.Add(roleDetaillogtemp);

                if (!_rolerepository.CreateRoleDetailLogTemp(roleDetaillogtemp))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }

                var roleDetaillog = _mapper.Map<RoleDetailLog>(detailDto);
                roleDetaillog.RoleTempId = roleMapLogTemp.Id;
                roleDetaillog.RoleDetailTempId = roleDetaillogtemp.Id;
                roleDetaillog.UpdatedBy = roleMapLogTemp.UpdatedBy;
                roleDetaillog.Action = "C";
                roleDetaillog.ActionDetail = "C";
                roleDetailsLog.Add(roleDetaillog);

                if (!_rolerepository.CreateRoleDetailLog(roleDetaillog))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
            }

            //==================== D E T A I L ==============================
            //DIRECT
            //var roleMap = _mapper.Map<Role>(RoleCreate);

            //roleMap.Name = RoleCreate.Name;
            //roleMap.ModuleId = RoleCreate.ModuleId;
            //roleMap.remarks = RoleCreate.Remarks;
            //roleMap.UpdatedBy = RoleCreate.UpdatedBy;
            //roleMap.Action = "";
            //roleMap.Status = "";

            //if (!_rolerepository.CreateRoleHeader(roleMap))
            //{
            //    ModelState.AddModelError("", "Something went wrong while saving");
            //    return StatusCode(500, ModelState);
            //}

            //List<RoleDetail> roleDetails = new List<RoleDetail>();

            //foreach (var detailDto in RoleCreate.detail)
            //{
            //    var roleDetail = _mapper.Map<RoleDetail>(detailDto);
            //    roleDetail.RoleId = roleMap.Id; // Assuming roleMap.Id is set after CreateRoleHeader
            //    roleDetail.UpdatedBy = roleMap.UpdatedBy;
            //    roleDetails.Add(roleDetail);

            //    if (!_rolerepository.CreateRoleDetail(roleDetail))
            //    {
            //        ModelState.AddModelError("", "Something went wrong while saving");
            //        return StatusCode(500, ModelState);
            //    }
            //}
            //DIRECT

            return Ok("Successfully Added to Pending list. Waiting for approval");
        }

        [HttpDelete("{roleId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRole(int roleId)
        {
            if (!_rolerepository.RoleExists(roleId))
            {
                return NotFound();
            }

            //var rolestemp = _rolerepository.GetPendingRole_all()
            //.Where(c => c.RoleID == roleId && c.Action == "D")
            //.FirstOrDefault();

            //if (rolestemp != null)
            //{
            //    ModelState.AddModelError("", "Role already exists. Need approve");
            //    return StatusCode(409, ModelState);
            //}

            var roles = _rolerepository.GetRole(roleId);

            //==================== H E A D E R ==============================
            var roleMap = _mapper.Map<Role>(roles);
            roleMap.Action = "D";
            roleMap.Status = "NA";
            if (!_rolerepository.UpdateRole(roleMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            var roleMapLogTemp = _mapper.Map<RoleLogTemp>(roleMap);
            roleMapLogTemp.Id = 0;
            roleMapLogTemp.RoleId = roleId;
            roleMapLogTemp.Action = "D";
            roleMapLogTemp.Status = "NA";
            roleMapLogTemp.remarks = "";
            if (!_rolerepository.CreateRoleHeaderLogTemp(roleMapLogTemp))
            {
                ModelState.AddModelError("", "Something went wrong while saving log temp");
                return StatusCode(500, ModelState);
            }

            var roleMapLog = _mapper.Map<RoleLog>(roleMap);
            roleMapLog.Id = 0;
            roleMapLog.RoleId = roleId;
            roleMapLog.RoleTempId = roleMapLogTemp.Id;
            roleMapLog.Action = "D";
            roleMapLog.Status = "NA";
            roleMapLog.remarks = "";
            if (!_rolerepository.CreateRoleHeaderLog(roleMapLog))
            {
                ModelState.AddModelError("", "Something went wrong while saving log");
                return StatusCode(500, ModelState);
            }
            //==================== H E A D E R ==============================

            //==================== D E T A I L ==============================
            var rolesdetail = _mapper.Map<List<RoleDetail>>(_rolerepository.GetRolesDetail(roles.Id));

            foreach (var details in rolesdetail)
            {
                var rolesDetailMap = _mapper.Map<RoleDetail>(details);
                rolesDetailMap.Action = "D";
                rolesDetailMap.ActionDetail = "D";
                if (!_rolerepository.UpdateRoleDetail(rolesDetailMap))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }

                var rolesDetailMapLogTemp = _mapper.Map<RoleDetailLogTemp>(details);
                rolesDetailMapLogTemp.Id = 0;
                rolesDetailMapLogTemp.RoleId = roleId;
                rolesDetailMapLogTemp.RoleTempId = roleMapLogTemp.Id;
                rolesDetailMapLogTemp.RoleDetailId = rolesDetailMap.Id;
                rolesDetailMapLogTemp.Action = "D";
                rolesDetailMapLogTemp.ActionDetail = "D";
                if (!_rolerepository.CreateRoleDetailLogTemp(rolesDetailMapLogTemp))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }

                var rolesDetailMapLog = _mapper.Map<RoleDetailLog>(details);
                rolesDetailMapLog.Id = 0;
                rolesDetailMapLog.RoleId = roleId;
                rolesDetailMapLogTemp.RoleTempId = roleMapLogTemp.Id;
                rolesDetailMapLog.RoleDetailId = rolesDetailMap.Id;
                rolesDetailMapLog.RoleDetailTempId = rolesDetailMapLogTemp.Id;
                rolesDetailMapLog.Action = "D";
                rolesDetailMapLog.ActionDetail = "D";
                if (!_rolerepository.CreateRoleDetailLog(rolesDetailMapLog))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
            }
            //==================== D E T A I L ==============================

            return Ok("Successfully Added to Pending list. Waiting for approval");
        }

        [HttpPut("{roleId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateRole(int roleId, [FromBody] RoleResultDto updatedRole)
        {
            var roles = _rolerepository.GetRole(roleId);
            //==================== H E A D E R ==============================
            var roleMap = _mapper.Map<Role>(roles);
            roleMap.Action = "U";
            roleMap.Status = "NA";
            if (!_rolerepository.UpdateRole(roleMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            var roleMapLogTemp = _mapper.Map<RoleLogTemp>(updatedRole);
            roleMapLogTemp.Id = 0;
            roleMapLogTemp.RoleId = roleId;
            roleMapLogTemp.Action = "U";
            roleMapLogTemp.Status = "NA";
            roleMapLogTemp.remarks = "";
            if (!_rolerepository.CreateRoleHeaderLogTemp(roleMapLogTemp))
            {
                ModelState.AddModelError("", "Something went wrong while saving log temp");
                return StatusCode(500, ModelState);
            }

            var roleMapLog = _mapper.Map<RoleLog>(updatedRole);
            roleMapLog.Id = 0;
            roleMapLog.RoleId = roleId;
            roleMapLog.RoleTempId = roleMapLogTemp.Id;
            roleMapLog.Action = "U";
            roleMapLog.Status = "NA";
            roleMapLog.remarks = "";
            if (!_rolerepository.CreateRoleHeaderLog(roleMapLog))
            {
                ModelState.AddModelError("", "Something went wrong while saving log");
                return StatusCode(500, ModelState);
            }

            //==================== H E A D E R ==============================

            //==================== D E T A I L ==============================
            List<RoleDetailLogTemp> roleDetailsLogTemp = new List<RoleDetailLogTemp>();
            List<RoleDetailLog> roleDetailsLog = new List<RoleDetailLog>();
            foreach (var detailDto in updatedRole.detail)
            {
                if (detailDto.Id == 0) 
                {
                    var roleDetaillogtemp = _mapper.Map<RoleDetailLogTemp>(detailDto);
                    roleDetaillogtemp.RoleTempId = roleMapLogTemp.Id;
                    roleDetaillogtemp.UpdatedBy = roleMapLogTemp.UpdatedBy;
                    roleDetaillogtemp.Action = "C";
                    roleDetaillogtemp.ActionDetail = "C";
                    roleDetailsLogTemp.Add(roleDetaillogtemp);

                    if (!_rolerepository.CreateRoleDetailLogTemp(roleDetaillogtemp))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    var roleDetaillog = _mapper.Map<RoleDetailLog>(detailDto);
                    roleDetaillog.RoleTempId = roleMapLogTemp.Id;
                    roleDetaillog.RoleDetailTempId = roleDetaillogtemp.Id;
                    roleDetaillog.UpdatedBy = roleMapLogTemp.UpdatedBy;
                    roleDetaillog.Action = "C";
                    roleDetaillog.ActionDetail = "C";
                    roleDetailsLog.Add(roleDetaillog);

                    if (!_rolerepository.CreateRoleDetailLog(roleDetaillog))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }
                } 
                else
                {
                    var rolesDetailMap = _mapper.Map<RoleDetail>(_rolerepository.GetRoleDetail(detailDto.Id));
                    rolesDetailMap.Action = "U";
                    rolesDetailMap.ActionDetail = detailDto.ActionDetail;
                    if (!_rolerepository.UpdateRoleDetail(rolesDetailMap))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    var rolesDetailMapLogTemp = _mapper.Map<RoleDetailLogTemp>(rolesDetailMap);
                    rolesDetailMapLogTemp.Id = 0;
                    rolesDetailMapLogTemp.RoleId = roleId;
                    rolesDetailMapLogTemp.RoleTempId = roleMapLogTemp.Id;
                    rolesDetailMapLogTemp.RoleDetailId = rolesDetailMap.Id;
                    rolesDetailMapLogTemp.Run = detailDto.Run;
                    rolesDetailMapLogTemp.Add = detailDto.Add;
                    rolesDetailMapLogTemp.Delete = detailDto.Delete;
                    rolesDetailMapLogTemp.Update = detailDto.Update;
                    rolesDetailMapLogTemp.Action = "U";
                    rolesDetailMapLogTemp.ActionDetail = detailDto.ActionDetail;
                    if (!_rolerepository.CreateRoleDetailLogTemp(rolesDetailMapLogTemp))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }

                    var rolesDetailMapLog = _mapper.Map<RoleDetailLog>(rolesDetailMap);
                    rolesDetailMapLog.Id = 0;
                    rolesDetailMapLog.RoleId = roleId;
                    rolesDetailMapLogTemp.RoleTempId = roleMapLogTemp.Id;
                    rolesDetailMapLog.RoleDetailId = rolesDetailMap.Id;
                    rolesDetailMapLog.RoleDetailTempId = rolesDetailMapLogTemp.Id;
                    rolesDetailMapLog.Run = detailDto.Run;
                    rolesDetailMapLog.Add = detailDto.Add;
                    rolesDetailMapLog.Delete = detailDto.Delete;
                    rolesDetailMapLog.Update = detailDto.Update;
                    rolesDetailMapLog.Action = "U";
                    rolesDetailMapLog.ActionDetail = detailDto.ActionDetail;
                    if (!_rolerepository.CreateRoleDetailLog(rolesDetailMapLog))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }
                }
            }

            //foreach (var details in rolesdetail)
            //{
            //    var rolesDetailMap = _mapper.Map<RoleDetail>(details);
            //    rolesDetailMap.Action = "U";
            //    rolesDetailMap.ActionDetail = "U";
            //    if (!_rolerepository.UpdateRoleDetail(rolesDetailMap))
            //    {
            //        ModelState.AddModelError("", "Something went wrong while saving");
            //        return StatusCode(500, ModelState);
            //    }

            //    var rolesDetailMapLogTemp = _mapper.Map<RoleDetailLogTemp>(details);
            //    rolesDetailMapLogTemp.Id = 0;
            //    rolesDetailMapLogTemp.RoleId = roleId;
            //    rolesDetailMapLogTemp.RoleTempId = roleMapLogTemp.Id;
            //    rolesDetailMapLogTemp.RoleDetailId = rolesDetailMap.Id;
            //    rolesDetailMapLogTemp.Action = "U";
            //    rolesDetailMapLogTemp.ActionDetail = "U";
            //    if (!_rolerepository.CreateRoleDetailLogTemp(rolesDetailMapLogTemp))
            //    {
            //        ModelState.AddModelError("", "Something went wrong while saving");
            //        return StatusCode(500, ModelState);
            //    }

            //    var rolesDetailMapLog = _mapper.Map<RoleDetailLog>(details);
            //    rolesDetailMapLog.Id = 0;
            //    rolesDetailMapLog.RoleId = roleId;
            //    rolesDetailMapLogTemp.RoleTempId = roleMapLogTemp.Id;
            //    rolesDetailMapLog.RoleDetailId = rolesDetailMap.Id;
            //    rolesDetailMapLog.RoleDetailTempId = rolesDetailMapLogTemp.Id;
            //    rolesDetailMapLog.Action = "D";
            //    rolesDetailMapLog.ActionDetail = "D";
            //    if (!_rolerepository.CreateRoleDetailLog(rolesDetailMapLog))
            //    {
            //        ModelState.AddModelError("", "Something went wrong while saving");
            //        return StatusCode(500, ModelState);
            //    }
            //}
            //==================== D E T A I L ==============================
            return Ok("Successfully Added to Pending list. Waiting for approval");
        }

        [HttpPost("approvalRole/")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult ApprovalRoles([FromBody] StatusIdList statusIdList)
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
                        if (_rolerepository.RoleTempExists(Convert.ToInt32(id))) 
                        {
                            var roleLogTempToDelete = _rolerepository.GetRoleLogTemp(Convert.ToInt32(id));
                            var rolesdetailLogTempDelete = _mapper.Map<List<RoleDetailLogTemp>>(_rolerepository.GetRoleDetailLogTemp_roleid(roleLogTempToDelete.Id));
                            if (roleLogTempToDelete != null)
                            {
                                if (roleLogTempToDelete.Action == "C" || roleLogTempToDelete.Action == "U" || roleLogTempToDelete.Action == "D")
                                {
                                    var updatedRole = new RoleDto();
                                    if (roleLogTempToDelete.Action == "C")
                                    {
                                        var roleMap = _mapper.Map<Role>(updatedRole);
                                        //==================== H E A D E R ==============================
                                        roleMap.Name = roleLogTempToDelete.Name;
                                        roleMap.ModuleId = roleLogTempToDelete.ModuleId;
                                        roleMap.Status = statusIdList.Approval;
                                        roleMap.Action = "N";
                                        roleMap.UpdatedBy = roleLogTempToDelete.UpdatedBy;
                                        roleMap.remarks = roleLogTempToDelete.remarks;

                                        if (!_rolerepository.CreateRoleHeader(roleMap))
                                        {
                                            ModelState.AddModelError("", "Something went wrong while saving");
                                            return StatusCode(500, ModelState);
                                        }

                                        var roleMapLog = _mapper.Map<RoleLog>(updatedRole);
                                        roleMapLog.RoleTempId = roleLogTempToDelete.Id;
                                        roleMapLog.RoleId = roleMap.Id;
                                        roleMapLog.Name = roleLogTempToDelete.Name;
                                        roleMapLog.Status = statusIdList.Approval;
                                        roleMapLog.Action = "N";
                                        roleMapLog.UpdatedBy = roleLogTempToDelete.UpdatedBy;
                                        roleMapLog.remarks = roleLogTempToDelete.remarks;
                                        roleMapLog.ActionRemarks = statusIdList.ActionRemarks;

                                        if (!_rolerepository.CreateRoleHeaderLog(roleMapLog))
                                        {
                                            ModelState.AddModelError("", "Something went wrong while saving");
                                            return StatusCode(500, ModelState);
                                        }
                                        //==================== H E A D E R ==============================

                                        //==================== D E T A I L ==============================
                                        List<RoleDetail> roleDetails = new List<RoleDetail>();
                                        List<RoleDetailLog> roleDetailsLog = new List<RoleDetailLog>();
                                        foreach (var details in rolesdetailLogTempDelete)
                                        {
                                            var roleDetail = _mapper.Map<RoleDetail>(details);
                                            roleDetail.RoleId = roleMap.Id;
                                            roleDetail.UpdatedBy = details.UpdatedBy;
                                            roleDetail.Action = "N";
                                            roleDetail.ActionDetail = "N";
                                            roleDetails.Add(roleDetail);

                                            if (!_rolerepository.CreateRoleDetail(roleDetail))
                                            {
                                                ModelState.AddModelError("", "Something went wrong while saving");
                                                return StatusCode(500, ModelState);
                                            }

                                            var roleDetaillog = _mapper.Map<RoleDetailLog>(details);
                                            roleDetaillog.Id = 0;
                                            roleDetaillog.RoleId = roleMap.Id;
                                            roleDetaillog.RoleDetailId = roleDetail.Id;
                                            roleDetaillog.UpdatedBy = details.UpdatedBy;
                                            roleDetaillog.Action = "N";
                                            roleDetaillog.ActionDetail = "N";
                                            roleDetailsLog.Add(roleDetaillog);

                                            if (!_rolerepository.CreateRoleDetailLog(roleDetaillog))
                                            {
                                                ModelState.AddModelError("", "Something went wrong while saving");
                                                return StatusCode(500, ModelState);
                                            }
                                        }
                                        //==================== D E T A I L ==============================

                                        response = "Successfully Approve";
                                        _flag = 1;
                                    }
                                    else if (roleLogTempToDelete.Action == "D")
                                    {
                                        //==================== D E T A I L ==============================
                                        //List<RoleDetail> roleDetails = new List<RoleDetail>();
                                        List<RoleDetailLog> roleDetailsLog = new List<RoleDetailLog>();
                                        foreach (var details in rolesdetailLogTempDelete)
                                        {
                                            var getroleDetailToDelete = _rolerepository.GetRoleDetail_roleid(details.RoleId);

                                            if (!_rolerepository.DeleteRoleDetail(getroleDetailToDelete))
                                            {
                                                ModelState.AddModelError("", "Something went wrong deleting role");
                                            }

                                            var roleDetaillog = _mapper.Map<RoleDetailLog>(details);
                                            roleDetaillog.Id = 0;
                                            roleDetaillog.RoleId = details.RoleId;
                                            roleDetaillog.RoleDetailId = details.RoleDetailId;
                                            roleDetaillog.RoleDetailTempId = details.Id;
                                            roleDetaillog.UpdatedBy = details.UpdatedBy;
                                            roleDetaillog.Action = "N";
                                            roleDetaillog.ActionDetail = "N";
                                            roleDetailsLog.Add(roleDetaillog);

                                            if (!_rolerepository.CreateRoleDetailLog(roleDetaillog))
                                            {
                                                ModelState.AddModelError("", "Something went wrong while saving");
                                                return StatusCode(500, ModelState);
                                            }
                                        }
                                        //==================== D E T A I L ==============================

                                        //==================== H E A D E R ==============================
                                        var getroleToDelete = _rolerepository.GetRole(roleLogTempToDelete.RoleId);

                                        if (!_rolerepository.DeleteRole(getroleToDelete))
                                        {
                                            ModelState.AddModelError("", "Something went wrong deleting role");
                                        }

                                        var roleMapLog = _mapper.Map<RoleLog>(updatedRole);
                                        roleMapLog.RoleTempId = roleLogTempToDelete.Id;
                                        roleMapLog.RoleId = getroleToDelete.Id;
                                        roleMapLog.Name = roleLogTempToDelete.Name;
                                        roleMapLog.Status = statusIdList.Approval;
                                        roleMapLog.Action = "N";
                                        roleMapLog.UpdatedBy = roleLogTempToDelete.UpdatedBy;
                                        roleMapLog.remarks = roleLogTempToDelete.remarks;
                                        roleMapLog.ActionRemarks = statusIdList.ActionRemarks;

                                        if (!_rolerepository.CreateRoleHeaderLog(roleMapLog))
                                        {
                                            ModelState.AddModelError("", "Something went wrong while saving");
                                            return StatusCode(500, ModelState);
                                        }
                                        //==================== H E A D E R ==============================
                                        response = "Successfully Approve";
                                        _flag = 1;
                                    }
                                    else if (roleLogTempToDelete.Action == "U")
                                    {
                                        List<RoleDetail> roleDetails = new List<RoleDetail>();
                                        List<RoleDetailLog> roleDetailsLog = new List<RoleDetailLog>();
                                        //==================== D E T A I L ==============================
                                        foreach (var details in rolesdetailLogTempDelete)
                                        {
                                            if (details.ActionDetail == "U")
                                            {
                                                //var roleDetail = _mapper.Map<RoleDetail>(details);
                                                var roleDetail = _mapper.Map<RoleDetail>(_rolerepository.GetRoleDetail(details.RoleDetailId));
                                                roleDetail.UpdatedBy = details.UpdatedBy;
                                                roleDetail.Action = "N";
                                                roleDetail.ActionDetail = "N";
                                                roleDetail.Add = details.Add;
                                                roleDetail.Run = details.Run;
                                                roleDetail.Update = details.Update;
                                                roleDetail.Delete = details.Delete;
                                                roleDetails.Add(roleDetail);

                                                if (!_rolerepository.UpdateRoleDetail(roleDetail))
                                                {
                                                    ModelState.AddModelError("", "Something went wrong while saving");
                                                    return StatusCode(500, ModelState);
                                                }

                                                var roleDetaillog = _mapper.Map<RoleDetailLog>(details);
                                                roleDetaillog.Id = 0;
                                                roleDetaillog.RoleId = details.RoleId;
                                                roleDetaillog.RoleDetailId = details.RoleDetailId;
                                                roleDetaillog.RoleDetailTempId = details.Id;
                                                roleDetaillog.UpdatedBy = details.UpdatedBy;
                                                roleDetaillog.Action = "N";
                                                roleDetaillog.ActionDetail = "N";
                                                roleDetailsLog.Add(roleDetaillog);

                                                if (!_rolerepository.CreateRoleDetailLog(roleDetaillog))
                                                {
                                                    ModelState.AddModelError("", "Something went wrong while saving");
                                                    return StatusCode(500, ModelState);
                                                }
                                            }
                                            else if(details.ActionDetail == "D")
                                            {
                                                var getroleDetailToDelete = _rolerepository.GetRoleDetail(details.RoleDetailId);

                                                if (!_rolerepository.DeleteRoleDetail(getroleDetailToDelete))
                                                {
                                                    ModelState.AddModelError("", "Something went wrong deleting role");
                                                }

                                                var roleDetaillog = _mapper.Map<RoleDetailLog>(details);
                                                roleDetaillog.Id = 0;
                                                roleDetaillog.RoleId = details.RoleId;
                                                roleDetaillog.RoleDetailId = details.RoleDetailId;
                                                roleDetaillog.RoleDetailTempId = details.Id;
                                                roleDetaillog.UpdatedBy = details.UpdatedBy;
                                                roleDetaillog.Action = "U";
                                                roleDetaillog.ActionDetail = "D";
                                                roleDetailsLog.Add(roleDetaillog);

                                                if (!_rolerepository.CreateRoleDetailLog(roleDetaillog))
                                                {
                                                    ModelState.AddModelError("", "Something went wrong while saving");
                                                    return StatusCode(500, ModelState);
                                                }
                                            }
                                            else if (details.ActionDetail == "C")
                                            {
                                                var roleDetail = _mapper.Map<RoleDetail>(details);
                                                roleDetail.RoleId = roleLogTempToDelete.RoleId;
                                                roleDetail.UpdatedBy = details.UpdatedBy;
                                                roleDetail.Action = "N";
                                                roleDetail.ActionDetail = "N";
                                                roleDetails.Add(roleDetail);

                                                if (!_rolerepository.CreateRoleDetail(roleDetail))
                                                {
                                                    ModelState.AddModelError("", "Something went wrong while saving");
                                                    return StatusCode(500, ModelState);
                                                }

                                                var roleDetaillog = _mapper.Map<RoleDetailLog>(details);
                                                roleDetaillog.Id = 0;
                                                roleDetaillog.RoleId = roleLogTempToDelete.RoleId;
                                                roleDetaillog.RoleDetailId = roleDetail.Id;
                                                roleDetaillog.UpdatedBy = details.UpdatedBy;
                                                roleDetaillog.Action = "N";
                                                roleDetaillog.ActionDetail = "N";
                                                roleDetailsLog.Add(roleDetaillog);

                                                if (!_rolerepository.CreateRoleDetailLog(roleDetaillog))
                                                {
                                                    ModelState.AddModelError("", "Something went wrong while saving");
                                                    return StatusCode(500, ModelState);
                                                }
                                            }

                                        }
                                        //==================== D E T A I L ==============================

                                        //==================== H E A D E R ==============================
                                        var roles = _rolerepository.GetRole(roleLogTempToDelete.RoleId);
                                        var roleMap = _mapper.Map<Role>(roles);
                                        roleMap.Name = roleLogTempToDelete.Name;
                                        roleMap.ModuleId = roleLogTempToDelete.ModuleId;
                                        roleMap.Action = "N";
                                        roleMap.Status = "N";
                                        if (!_rolerepository.UpdateRole(roleMap))
                                        {
                                            ModelState.AddModelError("", "Something went wrong while saving");
                                            return StatusCode(500, ModelState);
                                        }

                                        var roleMapLog = _mapper.Map<RoleLog>(updatedRole);
                                        roleMapLog.RoleTempId = roleLogTempToDelete.Id;
                                        roleMapLog.RoleId = roleMap.Id;
                                        roleMapLog.Name = roleLogTempToDelete.Name;
                                        roleMapLog.Status = statusIdList.Approval;
                                        roleMapLog.Action = "N";
                                        roleMapLog.UpdatedBy = roleLogTempToDelete.UpdatedBy;
                                        roleMapLog.remarks = roleLogTempToDelete.remarks;
                                        roleMapLog.ActionRemarks = statusIdList.ActionRemarks;

                                        if (!_rolerepository.CreateRoleHeaderLog(roleMapLog))
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

                                    foreach (var details in rolesdetailLogTempDelete)
                                    {
                                        if (!_rolerepository.DeleteRoleDetailLogTemp(details))
                                        {
                                            ModelState.AddModelError("", "Something went wrong deleting role");
                                        }
                                    }

                                    if (!_rolerepository.DeleteRoleLogTemp(roleLogTempToDelete))
                                    {
                                        ModelState.AddModelError("", "Something went wrong deleting role");
                                    }
                                }
                            }
                            total_data_success = total_data_success + 1;
                        }
                    }
                }
                else if (statusIdList.Approval == "R")
                {

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

        [HttpGet("PendingRole/")]
        //[Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Role>))]
        public IActionResult GetPendingRoles([FromQuery] int limit, [FromQuery] int page, [FromQuery] string sortby, [FromQuery] string sortdesc, [FromQuery] string? keyword)
        {
            var pendingroles = _mapper.Map<List<RoleJoinDtoPending>>(_rolerepository.GetPendingRoles_Join(limit, page, sortby, sortdesc, keyword));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var count = 0;

            if (keyword == "")
            {
                count = _rolerepository.Getpendingdatacount();
            }
            else
            {
                count = _rolerepository.GetRolesAdvancePending(keyword).Count;
            }

            var response = ApiResponseHelper.StdApiResponse("Data load success", pendingroles, datacount: count);

            return Ok(response);
        }

        [HttpGet("PendingRole/{pendingId}")]
        //[Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Role>))]
        public IActionResult GetPendingRole(int pendingId)
        {
            if (!_rolerepository.RoleTempExists(pendingId))
            {
                return NotFound();
            }

            var roles = _mapper.Map<RoleJoinDtoPending>(_rolerepository.GetRolesJoinPending(pendingId));

            var rolesdetail = _mapper.Map<List<RoleDetailJoinDtoPending>>(_rolerepository.GetRolesDetailJoinPending(roles.Id));

            var result = _mapper.Map<RoleResultJoinDtoPending>(roles);
            result.Id = roles.Id;
            result.Name = roles.Name;
            result.ModuleId = roles.ModuleId;
            result.ModuleName = roles.ModuleName;
            result.detail = rolesdetail;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(result);
        }

        [HttpDelete("deleteRoleTemp/{pendingId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRoleTemp(int pendingId, [FromBody] ActionRemark Actremark)
        {
            if (!_rolerepository.RoleTempExists(pendingId))
            {
                return NotFound();
            }

            var response = "";
            var roleLogTempToDelete = _rolerepository.GetRoleLogTemp(pendingId);

            var roleMapLog = _mapper.Map<RoleLog>(roleLogTempToDelete);
            roleMapLog.Id = 0;
            roleMapLog.RoleTempId = roleLogTempToDelete.Id;
            roleMapLog.ActionRemarks = Actremark.ActionRemarks;
            if (!_rolerepository.CreateRoleHeaderLog(roleMapLog))
            {
                ModelState.AddModelError("", "Something went wrong while saving log");
                return StatusCode(500, ModelState);
            }

            if (_rolerepository.RoleExists(roleLogTempToDelete.RoleId))
            {
                var roles = _rolerepository.GetRole(roleLogTempToDelete.RoleId);

                var branchMap = _mapper.Map<Role>(roles);
                branchMap.Action = "N";
                branchMap.Status = "A";
                if (!_rolerepository.UpdateRole(branchMap))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
            }

            var rolesdetailLogTempDelete = _mapper.Map<List<RoleDetailLogTemp>>(_rolerepository.GetRoleDetailLogTemp_(roleLogTempToDelete.Id));

            List<RoleDetailLog> roleDetailsLog = new List<RoleDetailLog>();
            foreach (var details in rolesdetailLogTempDelete)
            {
                var getroleDetailToDelete = _rolerepository.GetRoleDetailLogTemp(details.Id);

                if (!_rolerepository.DeleteRoleDetailLogTemp(getroleDetailToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong deleting role");
                }

                var roleDetaillog = _mapper.Map<RoleDetailLog>(details);
                roleDetaillog.Id = 0;
                roleDetaillog.RoleId = details.RoleId;
                roleDetaillog.RoleDetailId = details.RoleDetailId;
                roleDetaillog.RoleDetailTempId = details.Id;
                roleDetaillog.UpdatedBy = details.UpdatedBy;
                roleDetaillog.Action = "N";
                roleDetaillog.ActionDetail = "N";
                roleDetailsLog.Add(roleDetaillog);

                if (!_rolerepository.CreateRoleDetailLog(roleDetaillog))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }

                if (_rolerepository.RoleDetailExists(roleLogTempToDelete.RoleId))
                {
                    var roles = _rolerepository.GetRoleDetail(roleLogTempToDelete.RoleId);

                    var branchMap = _mapper.Map<RoleDetail>(roles);
                    branchMap.Action = "N";
                    branchMap.ActionDetail = "A";
                    if (!_rolerepository.UpdateRoleDetail(branchMap))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }
                }
            }

            //DELETE TEMP
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_rolerepository.DeleteRoleLogTemp(roleLogTempToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting role temp");
                return StatusCode(500, ModelState);
            }
            //DELETE TEMP
            response = "Successfully Deleted Pending Role";
            return Ok(response);
            //return NoContent();
        }

        [HttpPut("updateRoleTemp/{pendingId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateRoleTemp(int pendingId, [FromBody] RoleResultJoinDtoPendingUpdate updatedTempRole)
        {
            if (!_rolerepository.RoleTempExists(pendingId))
            {
                return NotFound();
            }
            
            var roleLogTempToEdit = _rolerepository.GetRoleLogTemp(pendingId);
            var response = "";

            //==================== H E A D E R ==============================

            var roleMap = _mapper.Map<RoleLogTemp>(roleLogTempToEdit);
            roleMap.Name = updatedTempRole.Name;
            roleMap.ModuleId = updatedTempRole.ModuleId;
            roleMap.remarks = updatedTempRole.Remarks;
            roleMap.ActionRemarks = updatedTempRole.ActionRemarks;
            roleMap.Action = updatedTempRole.Action;
            roleMap.Status = "NA";

            if (!_rolerepository.UpdateRoleLogTemp(roleMap))
            {
                ModelState.AddModelError("", "Something went wrong deleting role");
            }

            var roleMapLog = _mapper.Map<RoleLog>(roleLogTempToEdit);
            roleMapLog.Id = 0;
            roleMapLog.RoleTempId = roleLogTempToEdit.Id;
            roleMapLog.ActionRemarks = updatedTempRole.ActionRemarks;

            if (!_rolerepository.CreateRoleHeaderLog(roleMapLog))
            {
                ModelState.AddModelError("", "Something went wrong while saving log");
                return StatusCode(500, ModelState);
            }

            //==================== H E A D E R ==============================

            //==================== D E T A I L ==============================

            var rolesdetailLogTempEdit = _mapper.Map<List<RoleDetailLogTemp>>(_rolerepository.GetRoleDetailLogTemp_(roleLogTempToEdit.Id));

            List<RoleDetailLogTemp> roleDetailsLogTemp = new List<RoleDetailLogTemp>();
            List<RoleDetailLog> roleDetailsLog = new List<RoleDetailLog>();

            foreach (var details in updatedTempRole.detail)
            {
                var getroleDetailToEdit = _rolerepository.GetRoleDetailLogTemp(details.Id);

                var roleMapDetailTemp = _mapper.Map<RoleDetailLogTemp>(getroleDetailToEdit);
                roleMapDetailTemp.MenuId = details.MenuId;
                roleMapDetailTemp.Run = details.Run;
                roleMapDetailTemp.Add = details.Add;
                roleMapDetailTemp.Update = details.Update;
                roleMapDetailTemp.Delete = details.Delete;
                roleMapDetailTemp.Action = details.Action;
                roleMapDetailTemp.ActionRemarks = details.ActionRemarks;
                roleMapDetailTemp.remarks = "NA";

                if (!_rolerepository.UpdateRoleDetailLogTemp(getroleDetailToEdit))
                {
                    ModelState.AddModelError("", "Something went wrong deleting role");
                }

                var roleDetaillog = _mapper.Map<RoleDetailLog>(details);
                roleDetaillog.Id = 0;
                roleDetaillog.RoleId = details.RoleId;
                roleDetaillog.RoleDetailId = details.RoleDetailId;
                roleDetaillog.RoleDetailTempId = details.Id;
                roleDetaillog.UpdatedBy = details.UpdatedBy;
                roleDetaillog.Action = "N";
                roleDetaillog.ActionDetail = "N";
                roleDetailsLog.Add(roleDetaillog);

                if (!_rolerepository.CreateRoleDetailLog(roleDetaillog))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
            }

            //==================== D E T A I L ==============================

            return Ok(response);
        }
    }
}
