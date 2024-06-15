using API_Dinamis.Data;
using API_Dinamis.Dto;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using API_Dinamis.Repository;
using API_Dinamis.Utilities;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace API_Dinamis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository _companyrepository;
        private readonly IMapper _mapper;
        private readonly IAuthRepository _authrepository;
        private readonly RegionUtils _regionUtils;
        private readonly DataContext _context;

        public CompanyController(ICompanyRepository companyRepository, IMapper mapper, IAuthRepository authrepository, RegionUtils regionUtils, DataContext context)
        {
            _companyrepository = companyRepository;
            _mapper = mapper;
            _authrepository = authrepository;
            _regionUtils = regionUtils;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Company>))]
        [ProducesResponseType(400)]
        public IActionResult GetCompany()
        {
            try
            {
                var company = _mapper.Map<CompanyDto>(_companyrepository.GetCompany());
                //var company = _companyrepository.GetCompany();

                if (company == null)
                {
                    return NotFound(); // Or return any other appropriate response
                }

                return Ok(company);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("PendingCompany")]
        public IActionResult GetCompanyPending()
        {
            try
            {
                var companyTempDto = _companyrepository.GetCompanyTemp();

                if (companyTempDto == null)
                {
                    return NotFound(); // Or return any other appropriate response
                }

                return Ok(companyTempDto);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateCompany([FromBody] CompanyForm dataForm)
        {
            var response = "Successfully Added to Pending list. Waiting for approval";
            //todo
            //check company id as resource (form / pending form) -> for pending data process
            if (dataForm == null)
                return BadRequest(ModelState);

            //check company temporary
            var companyTempExist = _companyrepository.GetCompanyTemp();
            if (companyTempExist != null && dataForm.Target != "temp")
            {
                return StatusCode(409, "Company has a pending information to be approved or rejected first.");
            }

            //check company data
            //if company data already exist, do update
            //if company data exist, do add new
            var dataMainExist = _companyrepository.GetCompany(); //for company process data
            string action = (dataMainExist != null) ? "U" : "C";
            int dataMainTempId = 0;
            if (dataForm.Target == "temp")
            {
                var dataTempExist = _companyrepository.GetCompanyTemp(); // for company temporary process data
                if (dataTempExist == null)
                {
                    return NotFound();
                }

                action = dataTempExist.Action;
                dataMainTempId = dataTempExist.ID;

                response = "Successfully Updated Pending Company";
            }


            //check city, city must be exist
            if (dataForm.CityId != null)
            {
                var cityExists = _regionUtils.CityExists(dataForm.CityId);
                if (!cityExists)
                {
                    return StatusCode(404, "City not found.");
                }
            }
            else
            {
                return StatusCode(404, "City is required.");
            }
            //check zipcode, zipcode must be exist, zipcode must be a part of city
            if (dataForm.ZipCodeId != null)
            {
                var zipCodeExists = _regionUtils.ZipCodeExists(dataForm.ZipCodeId, dataForm.CityId);
                if (!zipCodeExists)
                {
                    return StatusCode(404, "ZipCode not found.");
                }
            }
            else
            {
                return StatusCode(404, "ZipCode is required.");
            }
            //check country, country must be exist
            if (dataForm.CountryId != null)
            {
                var countryExists = _regionUtils.CountryExists(dataForm.CountryId);
                if (!countryExists)
                {
                    return StatusCode(404, "Country not found.");
                }
            }
            else
            {
                return StatusCode(404, "Country is required.");
            }

            /*add temporary - if four eyes = true start*/
            var dataMapLogTemp = _mapper.Map<CompanyLogTemp>(dataForm);
            if (companyTempExist != null)
            {
                dataMapLogTemp.CompanyId = companyTempExist.CompanyId;
            }
            
            dataMapLogTemp.Status = "NA";
            if (dataForm.Target == "temp")
            {
                dataMapLogTemp.Id = companyTempExist.ID;

                //for action target is temporary data, not main
                if (!_companyrepository.UpdateCompanyLogTemp(dataMapLogTemp))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
            }
            else
            {
                //for action target is main data, not temporary
                dataMapLogTemp.Id = 0;
                if (action == "U")
                {
                    dataMapLogTemp.CompanyId = dataForm.ID;
                }
                dataMapLogTemp.Action = action;
                if (!_companyrepository.CreateCompanyLogTemp(dataMapLogTemp))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
                dataMainTempId = dataMapLogTemp.Id;
            }

            /*add temporary - if four eyes = true end*/

            /*add log start*/
            var dataMapLog = _mapper.Map<CompanyLog>(dataForm);
            dataMapLog.Id = 0;
            if (action == "U" && dataForm.Target == "main")
            {
                dataMapLog.CompanyId = dataForm.ID;
            }
            dataMapLog.CompanyTempId = dataMainTempId;
            dataMapLog.Action = action;
            dataMapLog.Status = "NA";
            if (!_companyrepository.CreateCompanyLog(dataMapLog))
            {
                ModelState.AddModelError("", "Something went wrong while saving log");
                return StatusCode(500, ModelState);
            }
            /*add log end*/

            return Ok(response);
        }

        [HttpPost("approvalCompany")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult ApprovalCompanies([FromBody] SingleDataApproval statusIdList)
        {
            var response = "";
            int totalDataFailed = 0;
            int totalDataSuccess = 0;
            int totalDataProcess = 0;
            int totalDataMissed = 0;

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //checking username & password - useable
                    var accountChecking = _authrepository.CheckingAccount(statusIdList.UserName, statusIdList.Password);
                    if (!(bool)accountChecking.GetType().GetProperty("result").GetValue(accountChecking, null))
                    {
                        var message = accountChecking.GetType().GetProperty("message").GetValue(accountChecking, null);
                        ModelState.AddModelError("", (string)message);
                        return StatusCode(401, ModelState);
                    }

                    //checking id list - useable
                    /*
                    if (statusIdList.IdList == null || statusIdList.IdList.Count == 0)
                    {
                        ModelState.AddModelError("", "id list is required");
                        return BadRequest(ModelState);
                    }
                    */

                    var companyTempExist = _companyrepository.GetCompanyTemp();
                    if (companyTempExist != null)
                    {
                        totalDataProcess = 1; //using count if multiple
                        var dataForm = new CompanyForm();
                        if (statusIdList.Approval == "A")
                        {
                            int mainID = 0;
                            //approve data process
                            /*set main data start*/
                            if (companyTempExist.Action == "C" || companyTempExist.Action == "U")
                            {
                                var mainDataMap = _mapper.Map<Company>(dataForm);
                                mainDataMap.CompanyCode = companyTempExist.CompanyCode;
                                mainDataMap.CompanyName = companyTempExist.CompanyName;
                                mainDataMap.Npwp = companyTempExist.Npwp;
                                mainDataMap.NpwpDate = companyTempExist.NpwpDate;
                                mainDataMap.KseiCode = companyTempExist.KseiCode;
                                mainDataMap.SinvestMiCode = companyTempExist.SinvestMiCode;
                                mainDataMap.SinvestSaCode = companyTempExist.SinvestSaCode;
                                mainDataMap.Address = companyTempExist.Address;
                                mainDataMap.Address2 = companyTempExist.Address2;
                                mainDataMap.CityId = companyTempExist.CityId;
                                mainDataMap.ZipCodeId = companyTempExist.ZipCodeId;
                                mainDataMap.CountryId = companyTempExist.CountryId;
                                mainDataMap.Phone = companyTempExist.Phone;
                                mainDataMap.Fax = companyTempExist.Fax;
                                mainDataMap.Email = companyTempExist.Email;
                                mainDataMap.ContactPerson = companyTempExist.ContactPerson;
                                mainDataMap.UpdatedBy = companyTempExist.UpdatedBy;
                                mainDataMap.Remarks = companyTempExist.Remarks;
                                mainDataMap.Status = statusIdList.Approval;
                                mainDataMap.Action = "N";

                                if (companyTempExist.Action == "U")
                                {
                                    //update exist main data
                                    mainDataMap.Id = companyTempExist.CompanyId;
                                    mainID = companyTempExist.CompanyId;

                                    if (!_companyrepository.UpdateCompany(mainDataMap))
                                    {
                                        ModelState.AddModelError("", "Something went wrong updating company");
                                        return StatusCode(500, ModelState);
                                    }
                                }
                                if (companyTempExist.Action == "C")
                                {
                                    //create new main data
                                    mainID = mainDataMap.Id;
                                    if (!_companyrepository.CreateCompany(mainDataMap))
                                    {
                                        ModelState.AddModelError("", "Something went wrong creating company");
                                        return StatusCode(500, ModelState);
                                    }
                                }

                            }
                            if (companyTempExist.Action == "D")
                            {
                                //no action, company has no delete activity
                            }
                            /*set main data end*/

                            /*set log data start*/
                            var mainLogMap = _mapper.Map<CompanyLog>(companyTempExist);
                            mainLogMap.Id = 0;
                            mainLogMap.CompanyTempId = companyTempExist.ID;
                            mainLogMap.CompanyId = mainID;
                            mainLogMap.Status = statusIdList.Approval;
                            mainLogMap.ActionRemarks = statusIdList.ActionRemarks;
                            mainLogMap.Action = companyTempExist.Action;

                            if (!_companyrepository.CreateCompanyLog(mainLogMap))
                            {
                                ModelState.AddModelError("", "Something went wrong while saving");
                                return StatusCode(500, ModelState);
                            }
                            /*set log data end*/

                            totalDataSuccess++;

                            if (!ModelState.IsValid)
                                return BadRequest(ModelState);

                            /*delete temp log data start*/
                            var mainTempMap = _mapper.Map<CompanyLogTemp>(companyTempExist);
                            if (!_companyrepository.DeleteCompanyLogTemp(mainTempMap))
                            {
                                ModelState.AddModelError("", "Something went wrong deleting branch");
                            }
                            /*delete temp log data end*/

                        }
                        else if (statusIdList.Approval == "R")
                        {
                            if (statusIdList.ActionRemarks == null)
                            {
                                ModelState.AddModelError("", "remarks is required");
                                return BadRequest(ModelState);
                            }
                            //reject data process
                            /*update temp log data start*/
                            var mainTempMap = _mapper.Map<CompanyLogTemp>(companyTempExist);
                            mainTempMap.Status = statusIdList.Approval;
                            mainTempMap.ActionRemarks = statusIdList.ActionRemarks;
                            if (!_companyrepository.UpdateCompanyLogTemp(mainTempMap))
                            {
                                ModelState.AddModelError("", "Something went wrong while saving");
                                return StatusCode(500, ModelState);
                            }
                            /*update temp log data end*/

                            /*set log data start*/
                            var mainLogMap = _mapper.Map<CompanyLog>(companyTempExist);
                            mainLogMap.Id = 0;
                            mainLogMap.CompanyTempId = companyTempExist.ID;
                            mainLogMap.Status = statusIdList.Approval;
                            mainLogMap.Action = companyTempExist.Action;
                            mainLogMap.ActionRemarks = statusIdList.ActionRemarks;

                            //test

                            if (!_companyrepository.CreateCompanyLog(mainLogMap))
                            {
                                ModelState.AddModelError("", "Something went wrong while saving");
                                return StatusCode(500, ModelState);
                            }
                            /*set log data end*/

                            totalDataSuccess++;
                        }
                        else
                        {
                            ModelState.AddModelError("", "approval type is invalid");
                            return BadRequest(ModelState);
                        }

                        response = "Successfully Process. Data success : " + totalDataSuccess + ". Data failed : " + totalDataFailed + ". Total data missed : " + totalDataMissed + ". Total data process : " + totalDataProcess + ".";
                    }
                    else
                    {
                        return NotFound();
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



        //api for delete pending
        [HttpDelete("deleteCompanyTemp")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCompanyTemp([FromBody] ActionRemark Actremark)
        {
            var response = "";
            var companyTempExist = _companyrepository.GetCompanyTemp();
            if (companyTempExist != null)
            {
                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        /*set delete log start*/
                        var mainLogMap = _mapper.Map<CompanyLog>(companyTempExist);
                        mainLogMap.Id = 0;
                        mainLogMap.CompanyTempId = companyTempExist.ID;;
                        mainLogMap.ActionRemarks = Actremark.ActionRemarks;

                        if (!_companyrepository.CreateCompanyLog(mainLogMap))
                        {
                            ModelState.AddModelError("", "Something went wrong while saving log");
                            return StatusCode(500, ModelState);
                        }
                        /*set delete log end*/

                        /*set reset status & action main data start*/
                        var company = _companyrepository.GetCompany();
                        if (company != null)
                        {
                            var mainDataMap = _mapper.Map<Company>(company);
                            mainDataMap.Action = "N";
                            mainDataMap.Status = "A";
                            if (!_companyrepository.UpdateCompany(mainDataMap))
                            {
                                ModelState.AddModelError("", "Something went wrong while saving");
                                return StatusCode(500, ModelState);
                            }
                        }
                        /*set reset status & action main data end*/

                        /*delete temporary start*/
                        if (!ModelState.IsValid)
                            return BadRequest(ModelState);

                        var mainTempMap = _mapper.Map<CompanyLogTemp>(companyTempExist);
                        if (!_companyrepository.DeleteCompanyLogTemp(mainTempMap))
                        {
                            ModelState.AddModelError("", "Something went wrong deleting branch temp");
                            return StatusCode(500, ModelState);
                        }
                        /*delete temporary end*/

                        response = "Successfully Deleted Pending Company";
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
            else
            {
                return NotFound();
            }
        }
    }
}
