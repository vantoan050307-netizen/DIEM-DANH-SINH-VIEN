using Microsoft.AspNetCore.Mvc;
using WebServiceDemo.Services;

namespace WebServiceDemo.Controllers;

[ApiController]
[Route("api/classes")]
public class ClassesController(AttendanceStore store) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll() => Ok(store.GetClasses());

    [HttpGet("{id:int}/students")]
    public IActionResult GetStudents(int id)
        => store.GetClass(id) is null ? NotFound(new { message = "Không tìm thấy lớp" }) : Ok(store.GetStudents(id));
}
