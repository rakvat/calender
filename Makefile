all: prepare libkalender kalender libtest

prepare:
	mkdir -p _builds

libkalender: ausgabe.cs hilfskonstrukte.cs
	gmcs -target:library ausgabe.cs eintrag.cs hilfskonstrukte.cs -out:_builds/libkalender.dll

kalender: main.cs 
	gmcs main.cs -r:_builds/libkalender.dll -out:_builds/kalender.exe

libtest: test.cs
	gmcs test.cs -target:library -pkg:nunit -r:_builds/libkalender.dll -out:_builds/test.dll

start:
	./_builds/kalender.exe

test:
	nunit-console _builds/test.dll

clean:
	rm -rf _builds
