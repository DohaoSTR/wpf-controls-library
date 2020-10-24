using System;

namespace ZdfFlatUI.Utils
{
    public class DateTimeHelper
    {
        public static bool MonthIsEqual(DateTime dt1, DateTime dt2)
        {
            bool flag = false;

            if (dt1.Year == dt2.Year && dt1.Month == dt2.Month)
            {
                flag = true;
            }

            return flag;
        }
    }
}
