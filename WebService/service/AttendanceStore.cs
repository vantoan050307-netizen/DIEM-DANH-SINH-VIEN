using WebServiceDemo.Models;

namespace WebServiceDemo.Services;

/// <summary>Kho dữ liệu trong bộ nhớ (thay bằng EF Core + DB khi cần lưu thật).</summary>
public class AttendanceStore
{
    private readonly object _lock = new();
    private readonly List<ClassRoom> _classes = new()
    {
        new(1, "HSK 3 - Lớp nâng cao 2"),
        new(2, "HSK 2 - Lớp cơ bản 1")
    };
    private readonly List<Student> _students = new()
    {
        new() { Id = 1, FullName = "Lê Văn Tuấn",       Email = "levantuan@educore.test",    ClassId = 1 },
        new() { Id = 2, FullName = "Lưu Thị Quỳnh",     Email = "luuthiquynh@educore.test",  ClassId = 1 },
        new() { Id = 3, FullName = "Nguyễn Thị Thanh",  Email = "nguyenthanh@educore.test",  ClassId = 1 }
    };
    private readonly List<AttendanceRecord> _records = new();
    private int _nextStudentId = 4;

    public List<ClassRoom> GetClasses() { lock (_lock) return _classes.ToList(); }

    public ClassRoom? GetClass(int id) { lock (_lock) return _classes.FirstOrDefault(c => c.Id == id); }

    public List<Student> GetStudents(int? classId = null)
    {
        lock (_lock) return _students.Where(s => classId == null || s.ClassId == classId).ToList();
    }

    public Student? GetStudent(int id) { lock (_lock) return _students.FirstOrDefault(s => s.Id == id); }

    public Student AddStudent(CreateStudentRequest r)
    {
        lock (_lock)
        {
            var s = new Student { Id = _nextStudentId++, FullName = r.FullName.Trim(), Email = r.Email.Trim(), ClassId = r.ClassId };
            _students.Add(s);
            return s;
        }
    }

    /// <summary>Ghi điểm danh; nếu học viên đã có bản ghi ngày đó thì cập nhật.</summary>
    public void Mark(int classId, DateOnly date, IEnumerable<MarkItem> items)
    {
        lock (_lock)
        {
            foreach (var it in items)
            {
                var existing = _records.FirstOrDefault(r => r.ClassId == classId && r.StudentId == it.StudentId && r.Date == date);
                if (existing != null) existing.Status = it.Status;
                else _records.Add(new AttendanceRecord { ClassId = classId, StudentId = it.StudentId, Date = date, Status = it.Status });
            }
        }
    }

    public List<AttendanceRecord> GetRecords(int? classId = null, DateOnly? date = null, int? studentId = null)
    {
        lock (_lock)
            return _records.Where(r => (classId == null || r.ClassId == classId)
                                    && (date == null || r.Date == date)
                                    && (studentId == null || r.StudentId == studentId)).ToList();
    }
}
