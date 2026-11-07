#pragma once

#ifdef _WIN32
  #define ATT_API extern "C" __declspec(dllexport)
#else
  #define ATT_API extern "C" __attribute__((visibility("default")))
#endif

// Trạng thái: 0 = vắng, 1 = có mặt, 2 = đi trễ, 3 = có phép
// Tỷ lệ chuyên cần = (có mặt + trễ) / (tổng - có phép) * 100
// Trả về 0 nếu thành công, -1 nếu tham số sai.
ATT_API int att_calc_stats(const int* statuses, int count,
                           int* present, int* absent, int* late, int* excused,
                           double* rate);
