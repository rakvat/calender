all: kalender.exe 

kalender.exe: main.cs ausgabe.cs
	gmcs main.cs ausgabe.cs -out:kalender.exe

clean:
	rm -f *.exe
