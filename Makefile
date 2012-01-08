all: calender.exe 

calender.exe: main.cs 
	gmcs main.cs CalenderEntry.cs -out:calender.exe

clean:
	rm -f *.exe
