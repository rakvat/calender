using System;

class CalenderEntry {
    private DateTime _date;
    private string _title;

    public CalenderEntry(DateTime theDate, string theTitle) {
        _date = theDate;
        _title = theTitle;
    }

    public void toString() {
        Console.WriteLine(_date.ToShortDateString() + ": " + _title);
    }
}
