using Microsoft.AspNetCore.Mvc;
using WebServiceDemo.Models;
using WebServiceDemo.Services;

namespace WebServiceDemo.Controllers;

[ApiController]
[Route("api/attendance")]
public class AttendanceController(AttendanceStore store) : ControllerBase
{
    // Điểm danh cả lớp theo ngày (gọi lại cùng ngày sẽ cập nhật)
    [HttpPost]
    public IActionResult Mark(MarkAttendanceRequest req)
    {
        if (store.GetClass(req.ClassId) is null)
            return BadRequest(new { message = "Lớp không tồn tại" });
        if (req.Records is null || req.Records.Count == 0)
            return BadRequest(new { message = "Danh sách điểm danh trống" });

        var classStudents = store.GetStudents(req.ClassId).Select(s => s.Id).ToHashSet();
        var invalid = req.Records.Where(r => !classStudents.Contains(r.StudentId)).Select(r => r.StudentId).ToList();
        if (invalid.Count > 0)
            return BadRequest(new { message = "Học viên không thuộc lớp", studentIds = invalid });
        if (req.Records.Any(r => !Enum.IsDefined(r.Status)))
            return BadRequest(new { message = "Trạng thái không hợp lệ (0-3)" });

        store.Mark(req.ClassId, req.Date, req.Records);
        return Ok(new { saved = req.Records.Count, stats = Stats(store.GetRecords(req.ClassId, req.Date)) });
    }

    [HttpGet]
    public IActionResult Get([FromQuery] int classId, [FromQuery] DateOnly date)
    {
        var records = store.GetRecords(classId, date);
        return Ok(new { classId, date, records, stats = Stats(records) });
    }

    [HttpGet("students/{studentId:int}/stats")]
    public IActionResult StudentStats(int studentId)
    {
        var student = store.GetStudent(studentId);
        if (student is null) return NotFound(new { message = "Không tìm thấy học viên" });
        return Ok(new { student, stats = Stats(store.GetRecords(studentId: studentId)) });
    }

    private static AttendanceStats Stats(IEnumerable<AttendanceRecord> records)
        => NativeAttendance.Calculate(records.Select(r => r.Status).ToList());
}
