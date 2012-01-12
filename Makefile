all: kalenderlibarary.dll kalender.exe 

kalenderlibarary.dll: ausgabe.cs hilfskonstrukte.cs
	gmcs -target:library ausgabe.cs hilfskonstrukte.cs -out:kalenderlibarary.dll

kalender.exe: main.cs 
	gmcs main.cs -r:kalenderlibarary.dll -out:kalender.exe

clean:
	rm -f *.exe *.dll
