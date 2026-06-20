using VDCD.BL.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDCD.Common.DTOs;
using VDCD.Common.Model;
using VDCD.Common.Resources;

namespace VDCD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnitsController : ControllerBase
    {
        private readonly IBaseBL<Unit> _unitService;

        public UnitsController(IBaseBL<Unit> unitService)
        {
            _unitService = unitService;
        }

        /// <summary>
        /// Lấy tất cả đơn vị tính
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Unit>>> GetAllUnits()
        {
            try
            {
                var units = await _unitService.GetAllAsync();
                return Ok(units);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResult 
                {
                    DevMsg = ex.Message,
                    UserMsg = Resource1.Exception,
                    MoreInfo = ex.Data,
                });
            }
        }

        /// <summary>
        /// Lấy đơn vị tính theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Unit>> GetUnitById(Guid id)
        {
            try
            {
                var unit = await _unitService.GetByIdAsync(id);
                if (unit == null)
                {
                    return StatusCode(404, new ErrorResult
                    {
                        DevMsg = "Không tìm thấy đơn vị tính",
                        UserMsg = Resource1.Exception,
                    });
                }
                return Ok(unit);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResult
                {
                    DevMsg = ex.Message,
                    UserMsg = Resource1.Exception,
                    MoreInfo = ex.Data,
                });
            }
        }
    }
}
