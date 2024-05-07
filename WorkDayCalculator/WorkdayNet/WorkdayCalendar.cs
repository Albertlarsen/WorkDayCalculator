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

            return AdjustTime(incrementedDate);
        }
        private DateTime AddFractionTime(DateTime date, decimal fraction)
        {
            decimal inputTime = TimeWithMinutes(date.Hour, date.Minute);
            decimal workdayFractionHours = fraction * (workdayStop - workdayStart);
    
            DateTime newDate = DateTime.MinValue;
            if (fraction < 0)
            {
        
                if (inputTime < workdayStart || inputTime >= workdayStop)
                {
                    return CalculateSubtractedInregularTime(date, workdayFractionHours, null);
                }
                
                if (inputTime >= workdayStart && inputTime < workdayStop)
                {
                    var addValue = TimeWithMinutes(date.Hour, date.Minute) + workdayFractionHours;
                    if (inputTime + workdayFractionHours < workdayStart)
                    {
                        var remaningTime = workdayStart - (inputTime + workdayFractionHours);
                        newDate = date.Date.AddHours((double)addValue);
                        return CalculateSubtractedInregularTime(newDate, workdayFractionHours, remaningTime);
                    }

                    return FindNextWorkday(date.Date.AddHours((double)addValue));
                }
            }

            if (inputTime >= workdayStart && inputTime < workdayStop)
            {
                newDate = date.Date.AddHours((double)workdayStart + (double)(inputTime - workdayStart) + (double)workdayFractionHours);

                // will be negative if the the time is later than workdayStop
                var remainingTime = workdayStop - TimeWithMinutes(newDate.Hour, newDate.Minute);
                if (remainingTime < 0)
                {
                    newDate = CalculateInregularAddedTime(newDate, remainingTime: -remainingTime);
                }
                
                return FindNextWorkday(newDate);
            }

            if (inputTime >= workdayStop)
            {
                newDate = date.Date.AddDays(1).AddHours((double)workdayStart + (double)workdayFractionHours);
                
                return FindNextWorkday(newDate);
            }
            
            newDate = date.Date.AddHours((double)workdayStart + (double)workdayFractionHours);
    
            while (!IsWorkday(newDate))
            {
                newDate = newDate.AddDays(1);
            }
        
            return FindNextWorkday(newDate);
        }

        private DateTime CalculateSubtractedInregularTime(DateTime date, decimal calculatedFraction, decimal? remainingTime)
        {
            decimal time = TimeWithMinutes(date.Hour, date.Minute);
            DateTime newDate = DateTime.MinValue;
            
            if (remainingTime != null && date.Hour >= workdayStop)
            {
                newDate = date.Date.AddHours((double)workdayStop - (double)remainingTime);
                return newDate;
            }

            if (remainingTime != null && date.Hour < workdayStart)
            {
                newDate = date.Date.AddDays(-1).AddHours((double)workdayStop - (double)remainingTime);
                return newDate;
            }
            
            if (time < workdayStart && remainingTime == null)
            {
                newDate = date.Date.AddDays(-1).AddHours((double)workdayStart);
            }
        
            if (time >= workdayStop)
            {
                newDate = date.Date.AddHours((double)workdayStart);
            }
            
            var addValue = TimeWithMinutes(newDate.Hour, newDate.Minute) + calculatedFraction;
            var resultDate = newDate.AddHours((double)addValue);

            return FindPreviousWorkday(resultDate);
        }
        
        private DateTime CalculateInregularAddedTime(DateTime date, decimal remainingTime)
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
        
        private DateTime FindPreviousWorkday(DateTime date)
        {
            while (!IsWorkday(date))
            {
                date = date.AddDays(-1);
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
        
        private DateTime AdjustTime(DateTime date)
        {
            decimal currentTime = TimeWithMinutes(date.Hour, date.Minute);

            if (currentTime >= workdayStop)
            {
                date = date.Date.AddHours((double)workdayStart).AddDays(1);
                date = FindNextWorkday(date);
            }

            if (currentTime < workdayStart)
            {
                date = date.Date.AddHours((double)workdayStart);
                date = FindPreviousWorkday(date);
            }
            
            return date;
        }
        
        private static decimal TimeWithMinutes(int hour, int minute)
        {
            decimal minuteInHours = (decimal)minute / 60;
            decimal timeWithMinutes = hour + minuteInHours;
            return timeWithMinutes;
        }
    }
}
