var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles(); // Bật tính năng chạy file HTML tĩnh

// Danh sách sinh viên mẫu
var danhSachSinhVien = new List<WebService.SinhVien>
{
    new WebService.SinhVien { MaSV = "SV01", HoTen = "Nguyễn Văn A", Lop = "CNTT K15", DaDiemDanh = true },
    new WebService.SinhVien { MaSV = "SV02", HoTen = "Trần Thị B", Lop = "CNTT K15", DaDiemDanh = false },
    new WebService.SinhVien { MaSV = "SV03", HoTen = "Lê Văn C", Lop = "CNTT K15", DaDiemDanh = true }
};

// API lấy danh sách sinh viên
app.MapGet("/api/danhsach", () => danhSachSinhVien);

app.Run();