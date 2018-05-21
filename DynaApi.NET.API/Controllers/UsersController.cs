using System.Linq;
using DynaApi.NET.API.Controllers.Base;
using DynaApi.NET.Interface.Entity;
using DynaApi.NET.Interface.Model;
using DynaApi.NET.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace DynaApi.NET.API.Controllers
{
    [Route("/api/[controller]")]
    public class UsersController : BaseController<IUserModel, IUser>
    {
        private readonly IModelHandler<IUserModel, IUser, IUserProfile> _handler;
        private const string GET_USER = "GetUser";
        
        public UsersController(IModelHandler<IUserModel, IUser, IUserProfile> handler)
        {
            _handler = handler;
        }

        [HttpPut]
        public IActionResult Create([FromBody] IUserModel value)
        {
            ExecuteAction(_handler.Insert, value);
            return CreatedAtRoute(GET_USER, new {id = value.UserID}, value);
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] IUserModel value) =>
            ExecuteAction(_handler.Delete, value, noContentResult: true);

        [HttpDelete("{id}")]
        public IActionResult Delete(long id) =>
            Delete(GetModel(id));

        [HttpGet("{id}", Name=GET_USER)]
        public IActionResult Get(long id) =>
            ExecuteFunction(_handler.Get, users => users?.FirstOrDefault(u => u.UserID == id));
        
        [HttpPut]
        public IActionResult Update([FromBody] IUserModel value) =>
            ExecuteAction(_handler.Update, value, noContentResult: true);

        private IUserModel GetModel(long id) =>
            _handler.Get<IUser>(users => users.FirstOrDefault(u => u.UserID == id));
    }
}