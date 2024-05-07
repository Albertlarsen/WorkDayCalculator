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
        
        // 24-05-2004 22:00 with an addition of -1 work days is 24-05-2004 8:00
        var start11 = new DateTime(2004, 5, 24, 22, 00, 0);
        decimal increment11 = -1m;
        var incrementedDate11 = calendar.GetWorkdayIncrement(start11, increment11);
        Console.WriteLine(
            start11.ToString(format) +
            " with an addition of " +
            increment11 +
            " work days is " +
            incrementedDate11.ToString(format));
        
        // 24-05-2004 07:00 with an addition of 1 work days is 25-05-2004 8:00
        var start12 = new DateTime(2004, 5, 24, 07, 00, 0);
        decimal increment12 = -1m;
        var incrementedDate12 = calendar.GetWorkdayIncrement(start12, increment12);
        Console.WriteLine(
            start12.ToString(format) +
            " with an addition of " +
            increment12 +
            " work days is " +
            incrementedDate12.ToString(format));
        
        // 24-05-2004 22:00 with an addition of -2.25 work days is 20-05-2004 14:00
        var start3 = new DateTime(2004, 5, 24, 22, 00, 0);
        decimal increment3 = -2.25m;
        var incrementedDate3 = calendar.GetWorkdayIncrement(start3, increment3);
        Console.WriteLine(
            start3.ToString(format) +
            " with an addition of " +
            increment3 +
            " work days is " +
            incrementedDate3.ToString(format));
        
        // 24-05-2004 15:00 with an addition of 2.25 work days is 28-05-2004 09:00 pga helligdag
        var start2 = new DateTime(2004, 5, 24, 15, 0, 0);
        decimal increment2 = 2.25m;
        var incrementedDate2 = calendar.GetWorkdayIncrement(start2, increment2);
        Console.WriteLine(
            start2.ToString(format) +
            " with an addition of " +
            increment2 +
            " work days is " +
            incrementedDate2.ToString(format));
        
        // 24-05-2004 09:00 with an addition of -2.25 work days is 19-05-2004 15:00
        var start = new DateTime(2004, 5, 24, 09, 00, 0);
        decimal increment = -2.25m;
        var incrementedDate = calendar.GetWorkdayIncrement(start, increment);
        Console.WriteLine(
            start.ToString(format) +
            " with an addition of " +
            increment +
            " work days is " +
            incrementedDate.ToString(format));
        
        // 24-05-2004 15:00 with an addition of 1.25 work days is 26-05-2004 09:00
        var start5 = new DateTime(2004, 5, 24, 15, 00, 0);
        decimal increment5 = 1.25m;
        var incrementedDate5 = calendar.GetWorkdayIncrement(start5, increment5);
        Console.WriteLine(
            start5.ToString(format) +
            " with an addition of " +
            increment5 +
            " work days is " +
            incrementedDate5.ToString(format));
        
        // 24-05-2004 18:05 with an addition of -5.5 work days is 14-05-2004 12:00
        var start8 = new DateTime(2004, 5, 24, 18, 05, 0);
        decimal increment8 = -5.5m;
        var incrementedDate8 = calendar.GetWorkdayIncrement(start8, increment8);
        Console.WriteLine(
            start8.ToString(format) +
            " with an addition of " +
            increment8 +
            " work days is " +
            incrementedDate8.ToString(format));
        
        // 24-05-2004 19:03 with an addition of 44.723656 work days is 27-07-2004 13:47
        var start6 = new DateTime(2004, 5, 24, 19, 03, 0);
        decimal increment6 = 44.723656m;
        var incrementedDate6 = calendar.GetWorkdayIncrement(start6, increment6);
        Console.WriteLine(
            start6.ToString(format) +
            " with an addition of " +
            increment6 +
            " work days is " +
            incrementedDate6.ToString(format));

        // 24-05-2004 18:03 with an addition of -6.7470217 work days is 13-05-2004 10:02
        var start9 = new DateTime(2004, 5, 24, 18, 03, 0);
        decimal increment9 = -6.7470217m;
        var incrementedDate9 = calendar.GetWorkdayIncrement(start9, increment9);
        Console.WriteLine(
            start9.ToString(format) +
            " with an addition of " +
            increment9 +
            " work days is " +
            incrementedDate9.ToString(format));
        
        // 24-05-2004 08:03 with an addition of 12.782709 work days is 10-06-2004 14:18
        var start7 = new DateTime(2004, 5, 24, 08, 03, 0);
        decimal increment7 = 12.782709m;
        var incrementedDate7 = calendar.GetWorkdayIncrement(start7, increment7);
        Console.WriteLine(
            start7.ToString(format) +
            " with an addition of " +
            increment7 +
            " work days is " +
            incrementedDate7.ToString(format));
        
        // 24-05-2004 07:03 with an addition of 8.276628 work days is 04-06-2004 10:12
        var start10 = new DateTime(2004, 5, 24, 07, 03, 0);
        decimal increment10 = 8.276628m;
        var incrementedDate10 = calendar.GetWorkdayIncrement(start10, increment10);
        Console.WriteLine(
            start10.ToString(format) +
            " with an addition of " +
            increment10 +
            " work days is " +
            incrementedDate10.ToString(format));
    }
}