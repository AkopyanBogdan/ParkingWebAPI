using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingWebAPI.Library;

namespace ParkingWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ParkingController : Controller
    {

        private Parking _db;
        public ParkingController(Parking db)
        {
            this._db = db;
        }
        // GET api/parking
        //Кількість вільних місць
        [HttpGet]

        public JsonResult Get()
        {
            return Json(new { Balance = _db.Balance });
        }
        // GET api/parking/message
        //Кількість вільних місць
        //Кількість зайнятих місць 
        [HttpGet("{id}")]
        public JsonResult Get(string id)
        {
            int busyPlaces;
            if (id == "free")
                return Json(new { freePlaces = _db.CountOfFreePlaces() });

            return Json(busyPlaces = Settings._parkingSpace - _db.CountOfFreePlaces());
        }
    }
}