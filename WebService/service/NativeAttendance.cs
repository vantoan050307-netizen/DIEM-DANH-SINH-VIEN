using System.Runtime.InteropServices;
using WebServiceDemo.Models;

namespace WebServiceDemo.Services;

/// <summary>
/// Tính thống kê chuyên cần. Ưu tiên gọi thư viện C++ (attendance_native),
/// nếu chưa build/copy thư viện thì tự chuyển sang bản C# để API vẫn chạy.
/// </summary>
public static class NativeAttendance
{
    private const string Lib = "attendance_native";

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    private static extern int att_calc_stats(
        int[] statuses, int count,
        out int present, out int absent, out int late, out int excused,
        out double rate);

    public static bool UsingNative { get; private set; } = true;

    public static AttendanceStats Calculate(IReadOnlyList<AttendanceStatus> list)
    {
        var arr = list.Select(s => (int)s).ToArray();

        if (UsingNative)
        {
            try
            {
                if (att_calc_stats(arr, arr.Length, out var p, out var a, out var l, out var e, out var rate) == 0)
                    return new AttendanceStats(arr.Length, p, a, l, e, Math.Round(rate, 2));
            }
            catch (Exception ex) when (ex is DllNotFoundException or EntryPointNotFoundException or BadImageFormatException)
            {
                UsingNative = false; // không có thư viện C++ -> dùng bản C#
            }
        }

        return CalculateManaged(arr);
    }

    private static AttendanceStats CalculateManaged(int[] arr)
    {
        int p = arr.Count(x => x == 1), a = arr.Count(x => x == 0),
            l = arr.Count(x => x == 2), e = arr.Count(x => x == 3);
        int counted = arr.Length - e;
        double rate = counted > 0 ? (p + l) * 100.0 / counted : 0;
        return new AttendanceStats(arr.Length, p, a, l, e, Math.Round(rate, 2));
    }
}
