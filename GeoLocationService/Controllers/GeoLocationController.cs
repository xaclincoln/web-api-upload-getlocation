using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoLocationService.Controllers
{

    public class GeoLocationUploadRequest
    {
        /// <summary>
        /// 设备序列号
        /// </summary>
        public string DeviceSN { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }
    }

    class GeoLocationRecord : GeoLocationUploadRequest
    {
        public DateTime Timestamp { get; set; }
    }

    [Route("api/[controller]/[action]")]
    public class GeoLocationController : Controller
    {
        static ConcurrentQueue<GeoLocationRecord> _inMemoryRecords = new ConcurrentQueue<GeoLocationRecord>();

        [HttpPost]
        public IActionResult Upload(GeoLocationUploadRequest model)
        {
            _inMemoryRecords.Enqueue(new GeoLocationRecord
            {
                DeviceSN = model.DeviceSN,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                Timestamp = DateTime.Now,
            });
            return Ok();
        }

        [HttpGet]
        public IActionResult ReadAll()
        {
            return Json(_inMemoryRecords.AsEnumerable());
        }
    }
}
