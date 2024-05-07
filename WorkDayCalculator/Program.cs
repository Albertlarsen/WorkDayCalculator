// See https://aka.ms/new-console-template for more information
using System;
using WorkDayCalculator.WorkdayNet;

class Program
{
    static void Main(string[] args)
    {
        IWorkdayCalendar calendar = new WorkdayCalendar();
        calendar.SetWorkdayStartAndStop(8, 0, 16, 0);
        calendar.SetRecurringHoliday(5, 17);
        calendar.SetHoliday(new DateTime(2004, 5, 27));
        
        string format = "dd-MM-yyyy HH:mm";
        
        var start8 = new DateTime(2004, 5, 24, 18, 05, 0);
        decimal increment8 = -5.5m;
        var incrementedDate8 = calendar.GetWorkdayIncrement(start8, increment8);
        Console.WriteLine(
            start8.ToString(format) +
            " with an addition of " +
            increment8 +
            " work days is " +
            incrementedDate8.ToString(format));
    }
}