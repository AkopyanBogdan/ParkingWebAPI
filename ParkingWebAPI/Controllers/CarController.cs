using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ParkingWebAPI.Library;

namespace ParkingWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class CarController : Controller
    {

        private Parking _db;
        public CarController(Parking db)
        {
            this._db = db;
        }
        // GET api/car
        //Список всіх машин
        [HttpGet]
        public JsonResult Get()
        {
            return Json(JsonConvert.SerializeObject(_db._cars));
        }

        // GET api/cars/AA1111
        //Деталі по одній машині
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            Car car = _db._cars.FirstOrDefault(x => x.Id == id);
            if (car == null)
                return NotFound();

            return Json(JsonConvert.SerializeObject(car));
        }

        // POST api/car
        //Додати машину 
        [HttpPost]
        public IActionResult Post([FromBody]Car car)
        {
            if (car == null)
            {
                return NotFound();
            }

            _db.AddCar(car);
            return Ok();
        }

        // DELETE api/car/AA1111
        //Видалити машину
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _db.RemoveCarById(id);
            return Ok();
        }
    }
}