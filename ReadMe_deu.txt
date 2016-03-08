ED - Intelligent Boardcomputer Extension

Voraussetzungen:
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
Während dieser Zeit wird die Datenbankstruktur erstellt. Wenn dieses Fenster nicht erscheint, 
stimmt etwas nicht. Evtl. die Installation in einem anderen Zielverzeichnis erneut versuchen.
Ein Log hiervon ist im Data-Verzeichnis zu finden, falls hierbei Probleme auftreten.
(ED-IBE\Database\install.log). 

Sollte eine erneute Installation durchgeführt werden (z. B. bei einem Update) erkennt 
die Installationsroutine eine bestehende Datenbank anhand der Existenz der Datei "\ED-IBE\Database\data\ibdata1".
Die Datenbank wird dann unverändert beibehalten, solange bei der Installation nicht explizit 
das "Löschen und Neuerstellen" ausgewählt wird. 
Fall etwas schiefgeht und man die Datenbank einmal löschen möchte, mus man 
die Installation erneut durchführen und die Checkbox zum "Löschen und Neuerstellen" markieren.

2. Starten
Wenn ED-IBE gestartet wird, wird automatisch der Datenbankprozess mitgestartet ("mysqld.exe" im Taskmanager).
Wird ED-IBE beendet, wird auch der Datenbankprozess mit beendet (Ausnahme: Der Datenbankprozess war 
schon vor dem Start von ED-IBE vorhanden.)

Beim ersten Start werden auch die derzeit bekannten Waren, Systeme und Stationen aus den mitgelieferten
EDDN-Dumpfiles importiert und weitere benötigte Stammdaten angelegt. 
(siehe hierzu auch unter 3. : "Import data of systems/stations/commodities from EDDB-Files") 
Tipp: am besten als erstes auf die eigene Sprache umstellen (Settings)

3. Import bestehender Daten aus dem alten RegulatedNoise.
ED-IBE besitzt ein Menüpunkt "Data"->"Import". Dort können über "Import Old Datafiles" 
die Datenbestände aus dem alten RN übernommen werden.

"Import data of systems/stations/commodities from EDDB-Files" bietet die Möglichkeit
neuere Dumpfiles (die zunächst von der EDDB-Seite heruntergeladen werden müssen)
zu importieren. Hierdurch werden die Stations- und Systemdaten aktualisiert oder
neue Waren hinzugefügt. Dieser Import kann beliebig oft wiederholt werden macht
(Empfehlung: alle 1 bis 4 Wochen reicht vollkommen - oder wenn bestimmte Daten fehlen, 
die inzwischen in der EDDB angelegt worden sind)
->Bei ersten Start von ED-IBE wird dieser Import automatisch mit den mitgelieferten EDDB-Dumps ausgeführt. 

Mit "Import RN-CommandersLog Files" können gesplittete Logfiles von RN wieder eingelesen werden.
Voraussetzung ist, dass die einzelnen Dateien nach dem Muster "CommandersLog*.xml" benannt sind.
Mehrfachimport wird durch Abgleich des Zeitstempels verhindert.

4. Übersetzungen
Unter "Data"->"Edit Localization" kann man die Übersetzungen ansehen, editieren, importieren und exportieren.
Auch das Hinzufügen einer weiteren Sprache ist möglich. Hierzu die Übersetzungen exportieren, dann in der csv-Datei 
die neuen Spracheinträge hinzufügenund danach die geänderte csv-Datei wieder importieren. 
Nicht vorhandenen Übersetzungen einer neuen Sprache werden mit der englischen Bezeichnung ergänzt 
und können dann später korrigiert werden.

5. Tabellenansichten
Durch einen Rechtsklick auf den Tabellenheader (linkes oberes kleines Feld)
öffnet sich ein Dialog, mit dem die Tabellenansicht angepasst werden kann.
Die Breite der Spalten können auch per Drag'n'Drop angepasst werden.
Durch ein kurzes Öffnen des Dialogs werden diese Spalteneinstellungen dann auch übernommen.
(bekannter Bug: die Tabelle darf beim Einstellen nicht leer sein)

6. Comboboxen
Die ComboBoxen habe zwar feste Einträge. In vielen Fällen hat man aber die Möglichkeit 
beliebige eigene Werte einzutragen (z.B. bei "Max. Trip Distance")

7. Service-Formular SQL
Unter gibt es die Möglichkeit direkt Sql-Befehle abzusetzen.
Diese Möglichkeit wurde zur Fehlersuche und für Service-Tätigkeiten geschaffen.
Wenn man sich nicht sicher ist, dann lieber die Finger davon lassen.
Es besteht die Möglichkeit, interne Daten zu zerstören.
Wenn mehrere Befehle eingetragen sind, kann man diese durch Selektion gezielt
zum Ausführen auswählen.





