using System;
 
public class Calender
{
	static public void Main ()
	{
	    Console.WriteLine ("Hello Calender");
        Console.WriteLine("Das Programm berechnet Kalender für die Jahre 1583 bis 4000");
        Console.Write("Jahr eingeben ");

        CalenderEntry myEntry = new CalenderEntry(new DateTime(2012,3,3), "special day");
        myEntry.toString();
	}
 
}
