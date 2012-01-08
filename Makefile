all: kalender.exe 

kalender.exe: main.cs ausgabe.cs hilfskonstrukte.cs
	gmcs main.cs ausgabe.cs hilfskonstrukte.cs -out:kalender.exe

clean:
	rm -f *.exe
