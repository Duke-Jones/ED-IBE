ED - Intelligent Boardcomputer Extension

Vorraussetzungen:
- Win7 oder höher, x64
- Microsoft dotNet 4.6
- Microsoft Visual Studio 2013 C++ Runtimes (https://www.microsoft.com/en-gb/download/details.aspx?id=40784)
- Visual Studio 2012 C++ Redistributables (x86 und x64) (https://www.microsoft.com/en-us/download/details.aspx?id=30679)

Kurzanleitung/Starter-Tipps

1. Installation 
Standardmäßig installiert sich das Programm zu Hälfte in das Programme-
verzeichnis (C:\Program Files\ED-IBE) und zur Hälfte in das Data-Verzeichnis
(C:\Users\{USER}\AppData\Local\ED-IBE). Diese Pfade können während der Installation 
angepasst werden. Es kann auch für beide Teile das gleiche Verzeichnis (z. B. "F:\ED-IBE") angegeben werden.

Am Ende der Installation sollte für ein paar Augenblicke ein cmd-Fenster aufgehen.
Während dieser Zeit wird die Datenbankstruktur erstellt. 
Ein Log hiervon ist im Data-Verzeichnis zu finden, falls hierbei Probleme auftreten.
(ED-IBE\Database\install.log)

Sollte eine erneute Installation durchgeführt werden (z. B. bei einem Update) erkennt 
die Installationsroutine eine bestehende Datenbank anhand der Existenz der Datei "\ED-IBE\Database\data\ibdata1".
Die Datenbank wird dann unverändert beibehalten, solange bei der Installation nicht explizit 
das Löschen und Neuerstellen ausgewählt wird. 

2. Starten
Wenn ED-IBE gestartet wird, wird automatisch der Datenbankprozess mitgestartet ("mysqld.exe" im Taskmanager).
Wird ED-IBE beendet, wird auch der Datenbankprozess mit beendet (Ausnahme: Der Datenbankprozess war 
schon vor dem Start von ED-IBE vorhanden.)

Beim ersten Start werden auch die derzeit bekannten Waren, Systeme und Stationen aus den mitgelieferten
EDDN-Dumpfiles importiert und weitere benötigte Stammdaten angelegt. 

3. Import bestehender Daten aus dem alten RegulatedNoise.
ED-IBE besitz ein Menüpunkt "Data"->"Import". Dort können über "Import Old Datafiles" 
die Datenbestände aus dem alten RN übernommen werden.

"Reset Database" löscht den gesamten Datenbestand aus der Datenbank.

"Import data of systems/stations/commodities from EDDB-Files" bietet die Möglichkeit
neuere Dumpfiles (die zunächst von der EDDB-Seite heruntergeladen werden müssen)
zu importieren. Dieser Import kann beliebig oft wiederholt werden.

Mit "Import RN-CommandersLog Files" können gesplittete Logfiles von RN wieder eingelesen werden.
Vorraussetzung ist, dass die einzelnen Dateien nach dem Muster "CommandersLog*.xml" benannt sind.
Mehrfachimport wird durch Abgleich des Zeitstempels verhindert.

3. Tabellenansichten
Durch einen Rechtsklick auf den Tabellenheader (linkes oberes kleines Feld)
öffnet sich ein Dialog, mit dem die Tabellenansicht angepasst werden kann.
Die breiten der Spalten können auch per Drag'n'Drop angepasst werden.
Durch ein kurzes Öffnen des Dialogs werden diese Spalteneinstellungen auch übernommen.






