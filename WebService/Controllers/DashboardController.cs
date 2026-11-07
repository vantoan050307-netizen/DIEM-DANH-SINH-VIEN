using Microsoft.AspNetCore.Mvc;
using WebServiceDemo.Services;

namespace WebServiceDemo.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController(AttendanceStore store) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var all = store.GetRecords();
        var overall = NativeAttendance.Calculate(all.Select(r => r.Status).ToList());

        var topClasses = store.GetClasses().Select(c =>
        {
            var st = NativeAttendance.Calculate(store.GetRecords(classId: c.Id).Select(r => r.Status).ToList());
            return new { name = c.Name, total = st.Total, present = st.Present + st.Late, rate = st.Rate };
        }).OrderByDescending(x => x.rate).ToList();

        var topStudents = store.GetStudents().Select(s =>
        {
            var recs = store.GetRecords(studentId: s.Id);
            var st = NativeAttendance.Calculate(recs.Select(r => r.Status).ToList());
            return new { s.FullName, s.Email, st.Rate, Ratio = $"{st.Present + st.Late}/{st.Total - st.Excused}" };
        }).OrderByDescending(x => x.Rate).Take(10)
          .Select((x, i) => new { rank = i + 1, name = x.FullName, email = x.Email, rate = x.Rate, ratio = x.Ratio });

        return Ok(new
        {
            title = "Tổng quan điểm danh",
            subtitle = "Quản lý và theo dõi điểm danh học viên",
            totalStudents = store.GetStudents().Count,
            activeClasses = store.GetClasses().Count,
            attendanceChecks = all.Select(r => (r.ClassId, r.Date)).Distinct().Count(),
            averageRate = overall.Rate,
            present = overall.Present + overall.Late,
            absent = overall.Absent,
            engine = NativeAttendance.UsingNative ? "C++ native" : "C# fallback",
            topClasses,
            topStudents
        });
    }
}
