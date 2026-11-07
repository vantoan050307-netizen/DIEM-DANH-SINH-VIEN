namespace WebServiceDemo.Models;

public enum AttendanceStatus
{
    Absent = 0,   // vắng
    Present = 1,  // có mặt
    Late = 2,     // đi trễ
    Excused = 3   // có phép
}

public record ClassRoom(int Id, string Name);

public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public int ClassId { get; set; }
}

public class AttendanceRecord
{
    public int StudentId { get; set; }
    public int ClassId { get; set; }
    public DateOnly Date { get; set; }
    public AttendanceStatus Status { get; set; }
}

// ----- DTO -----
public record CreateStudentRequest(string FullName, string Email, int ClassId);

public record MarkItem(int StudentId, AttendanceStatus Status);

public record MarkAttendanceRequest(int ClassId, DateOnly Date, List<MarkItem> Records);

public record AttendanceStats(int Total, int Present, int Absent, int Late, int Excused, double Rate);
