#include "attendance_stats.h"

ATT_API int att_calc_stats(const int* statuses, int count,
                           int* present, int* absent, int* late, int* excused,
                           double* rate)
{
    if (count < 0 || (count > 0 && statuses == nullptr) ||
        !present || !absent || !late || !excused || !rate)
        return -1;

    int p = 0, a = 0, l = 0, e = 0;
    for (int i = 0; i < count; ++i) {
        switch (statuses[i]) {
            case 0: ++a; break;
            case 1: ++p; break;
            case 2: ++l; break;
            case 3: ++e; break;
            default: return -1;
        }
    }

    *present = p; *absent = a; *late = l; *excused = e;
    const int counted = count - e;
    *rate = counted > 0 ? (static_cast<double>(p + l) * 100.0) / counted : 0.0;
    return 0;
}
