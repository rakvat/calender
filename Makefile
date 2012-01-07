all: calender.exe 

calender.exe: main.cs
	gmcs main.cs

clean:
	rm -f *.exe
