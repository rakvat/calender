all: prepare libkalender kalender libtest

prepare:
	mkdir -p _builds

libkalender: kalenderjahr.cs ausgabe.cs hilfskonstrukte.cs
	gmcs -target:library kalenderjahr.cs ausgabe.cs eintrag.cs hilfskonstrukte.cs -out:_builds/libkalender.dll

kalender: main.cs 
	gmcs main.cs -r:_builds/libkalender.dll -out:_builds/kalender.exe

libtest: test.cs
	gmcs test.cs -target:library -pkg:nunit -r:_builds/libkalender.dll -out:_builds/test.dll

start:
	./_builds/kalender.exe

debug:
	./_builds/kalender.exe debug

runtest:
	rm -rf _builds/testfixtures
	cd _builds && ln -s ../testfixtures . && cd ..
	nunit-console _builds/test.dll
	rm -rf _builds/testfixtures

clean:
	rm -rf _builds
