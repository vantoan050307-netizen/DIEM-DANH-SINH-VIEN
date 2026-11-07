using Microsoft.AspNetCore.Mvc;
using WebServiceDemo.Models;
using WebServiceDemo.Services;

namespace WebServiceDemo.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsController(AttendanceStore store) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll([FromQuery] int? classId) => Ok(store.GetStudents(classId));

    [HttpPost]
    public IActionResult Create(CreateStudentRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.FullName))
            return BadRequest(new { message = "Họ tên không được để trống" });
        if (store.GetClass(req.ClassId) is null)
            return BadRequest(new { message = "Lớp không tồn tại" });

        var s = store.AddStudent(req);
        return Created($"/api/students/{s.Id}", s);
    }
}
