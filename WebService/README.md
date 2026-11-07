# WebServiceDemo – Web service điểm danh (C# + C++)

- **C# (ASP.NET Core 8)**: REST API, controllers, lưu trữ trong bộ nhớ.
- **C++**: thư viện `attendance_native` tính thống kê chuyên cần, được C# gọi qua P/Invoke.
  Nếu chưa build thư viện, API tự dùng bản C# tương đương (xem trường `engine` ở `/api/dashboard`).

## Build thư viện C++
    cd native
    cmake -S . -B build -DCMAKE_BUILD_TYPE=Release
    cmake --build build --config Release
Thư viện được xuất vào `bin-native/`, `dotnet build` sẽ tự copy cạnh file chạy.

## Chạy API
    dotnet run        # Swagger: /swagger, thử request trong WebServiceDemo.http

## Endpoint
| Method | URL | Mô tả |
|---|---|---|
| GET | /api/dashboard | Tổng quan |
| GET | /api/classes | Danh sách lớp |
| GET | /api/classes/{id}/students | Học viên của lớp |
| GET/POST | /api/students | Danh sách / thêm học viên |
| POST | /api/attendance | Điểm danh cả lớp theo ngày |
| GET | /api/attendance?classId=&date= | Xem điểm danh |
| GET | /api/attendance/students/{id}/stats | Thống kê 1 học viên |

Trạng thái: 0 vắng, 1 có mặt, 2 trễ, 3 có phép.
