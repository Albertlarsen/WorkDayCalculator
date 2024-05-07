namespace WorkDayCalculator.WorkdayNet
{
    public class WorkdayCalendar : IWorkdayCalendar
    {
        private HashSet<DateTime> holidays = new HashSet<DateTime>();
        private HashSet<Tuple<int, int>> recurringHolidays = new HashSet<Tuple<int, int>>();
        private int startHours;
        private int startMinutes;
        private int stopHours;
        private int stopMinutes;
        decimal midnight = TimeWithMinutes(23, 59);
        decimal morning = TimeWithMinutes(0, 0);
        private decimal workdayStart;
        private decimal workdayStop;


        public void SetHoliday(DateTime date)
        {
            holidays.Add(date.Date);
        }

        public void SetRecurringHoliday(int month, int day)
        {
            recurringHolidays.Add(new Tuple<int, int>(month, day));
        }

        public void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes)
        {
            this.startHours = startHours;
            this.startMinutes = startMinutes;
            this.stopHours = stopHours;
            this.stopMinutes = stopMinutes;
            workdayStart = TimeWithMinutes(startHours, startMinutes);
            workdayStop = TimeWithMinutes(stopHours, stopMinutes);
        }

        public DateTime GetWorkdayIncrement(DateTime startDate, decimal incrementInWorkdays)
        {
            var incrementedDate = startDate;
            decimal increment = incrementInWorkdays;
            // will be 1 if 0/positive or -1 if negative
            decimal incrementDirection = increment >= 0 ? 1 : -1;

            while (increment != 0)
            {
                if (Math.Abs(increment) < 1)
                {
                    incrementedDate = AddFractionTime(incrementedDate, increment);
                    incrementDirection = increment;
                }
                else
                {
                    incrementedDate = incrementedDate.AddDays((double)incrementDirection);
                }
                
                if (IsWorkday(incrementedDate))
                {
                    increment -= incrementDirection;
                }
            }

            return incrementedDate;
        }
        private DateTime AddFractionTime(DateTime date, decimal fraction)
        {
            decimal inputTime = TimeWithMinutes(date.Hour, date.Minute);
            decimal workdayFractionHours = fraction * (workdayStop - workdayStart);
    
            if (fraction < 0)
            {
                DateTime newDate = DateTime.MinValue;
        
                if (inputTime < workdayStart || inputTime >= workdayStop)
                {
                    return CalculateInregularTime(date, workdayFractionHours, null);
                }
                
                if (inputTime >= workdayStart && inputTime < workdayStop)
                {
                    var addValue = TimeWithMinutes(date.Hour, date.Minute) + workdayFractionHours;
                    if (inputTime + workdayFractionHours < workdayStart)
                    {
                        var remaningTime = workdayStart - (inputTime + workdayFractionHours);
                        newDate = date.Date.AddHours((double)addValue);
                        return CalculateInregularTime(newDate, workdayFractionHours, remaningTime);
                    }

                    return FindNextWorkday(date.Date.AddHours((double)addValue));
                }
            }

            if (inputTime >= workdayStart && inputTime < workdayStop)
            {
                var resultDate2 = date.Date.AddHours((double)workdayStart + (double)(inputTime - workdayStart) + (double)workdayFractionHours);

                // will be negative if the the time is after workdayStop
                var remainingTime = workdayStop - TimeWithMinutes(resultDate2.Hour, resultDate2.Minute);
                if (remainingTime < 0)
                {
                    resultDate2 = CalculateInregularTime(resultDate2, remainingTime: -remainingTime);
                }
                
                return FindNextWorkday(resultDate2);
            }

            if (inputTime >= workdayStop)
            {
                var resultDate3 = date.Date.AddDays(1).AddHours((double)workdayStart + (double)workdayFractionHours);
                
                return FindNextWorkday(resultDate3);
            }
            
            var resultDate4 = date.Date.AddHours((double)workdayStart + (double)workdayFractionHours);
    
            while (!IsWorkday(resultDate4))
            {
                resultDate4 = resultDate4.AddDays(1);
            }
        
            return FindNextWorkday(resultDate4);
        }

        private DateTime CalculateInregularTime(DateTime date, decimal calculatedFraction, decimal? remainingTime)
        {
            decimal time = TimeWithMinutes(date.Hour, date.Minute);
            DateTime newDate = DateTime.MinValue;

            if (remainingTime != null)
            {
                newDate = date.Date.AddHours((double)workdayStop - (double)remainingTime);
                return newDate;
            }
            
            if (time < workdayStart && remainingTime == null)
            {
                newDate = date.Date.AddDays(-1).AddHours((double)workdayStart);
            }
        
            if (time >= workdayStop)
            {
                newDate = date.Date.AddHours(08);
            }
            
            var addValue = TimeWithMinutes(newDate.Hour, newDate.Minute) + calculatedFraction;
            var resultDate = newDate.AddHours((double)addValue);
            
            return FindNextWorkday(resultDate);
        }
        
        private DateTime CalculateInregularTime(DateTime date, decimal remainingTime)
        {
            DateTime newDate = DateTime.MinValue;

            newDate = date.Date.AddDays(1).AddHours((double)workdayStart + (double)remainingTime);
            return newDate;
        }

        private DateTime FindNextWorkday(DateTime date)
        {
            while (!IsWorkday(date))
            {
                date = date.AddDays(1);
            }
            
            return date;
        }

        private bool IsWorkday(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || holidays.Contains(date.Date))
            {
                return false;
            }

            foreach (var recurringHoliday in recurringHolidays)
            {
                if (recurringHoliday.Item1 == date.Month && recurringHoliday.Item2 == date.Day)
                {
                    return false;
                }
            }

            return true;
        }
        private static decimal TimeWithMinutes(int hour, int minute)
        {
            decimal minuteInHours = (decimal)minute / 60;
            decimal timeWithMinutes = hour + minuteInHours;
            return timeWithMinutes;
        }
    }
}
