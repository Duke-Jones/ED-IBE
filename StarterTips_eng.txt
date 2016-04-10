! This is a automatically translated copy of "ReadMe_ger.txt" by Google Translator.
! If you want to translate this text to another language, 
! I suggest to use "ReadMe_ger.txt" as base for translation.


ED - Intelligent Board Computer Extension

Conditions:
- Win7 or higher, x64
- Microsoft dotNet 4.6
- Microsoft Visual Studio 2013 C ++ runtimes (https://www.microsoft.com/en-gb/download/details.aspx?id=40784)
- Visual Studio 2012 C ++ Redistributable (x86 and x64) (https://www.microsoft.com/en-us/download/details.aspx?id=30679)

Quick Guide / Starter Tips

1. installation
By default, the program installs to half in the Programs
directory (C: \ Program Files \ ED IBE) and the other half in the data directory
(C: \ Users \ {USER} \ AppData \ Local \ ED IBE). These paths can during installation
be adjusted. You can specify the same directory for both parties
(. Eg "F: \ ED IBE").

At the end of the installation should go up for a few moments a cmd window.
During this time the database structure is created. If this window does not appear,
something wrong. Possibly try re-installing in another destination directory.
A log of this is found in the data directory, if experience problems.
(ED IBE \ Database \ install.log).

Should a new installation must be performed (for example, during an update) recognizes
the installation routine an existing database based on the existence of "\ ED IBE \ Database \ data \ ibdata1" file.
The database will be maintained unchanged, unless explicitly during installation
the "Delete and re-create" is selected.
Case something goes wrong and you want to delete the database again, you have to
run the installer again and check the checkbox for "Delete and re-create."

2. Start
If ED IBE is started, the database engine is automatically started ( "mysqld.exe" in the Task Manager).
If ED IBE completed, and the database process is terminated (exception: was the Database Engine
available before the start of ED IBE.)

When you first start the currently known products, systems and stations from the supplied are
EDDN dump files imported and managed more required master data.
(See also under 3. "Import data of systems / stations / commodities from EDDB-Files")
Tip: best first to their own language change (Settings)

3. Import existing data from the old RegulatedNoise.
ED IBE has a menu item "Data" -> "Import". You have to "Import Old Data Files"
the databases are copied from the old RN.

"Import data of systems / stations / commodities from EDDB-Files" offers the possibility
newer dump files (which must first be downloaded from the EDDB page)
to import. Thereby, the station and system data are updated or
new products added. This import can be repeated as often makes
(Recommended every 1 to 4 weeks is enough perfectly - or when certain data is missing,
which have since been applied in the EDDB)
-> At the first start of ED IBE this import is performed automatically using the supplied EDDB dumps.
> During an update from ED IBE this data is also updated automatically.

With "Import RN CommandersLog Files" can split logfiles of RN are read again.
The prerequisite is that the individual files are "* .xml CommandersLog" named after the pattern.
Multiple import is prevented by comparing the time stamp.

4. translations
Under "Data" -> "Edit Localization" you can view translations, edit, import and export.
Also, the addition of another language is possible. For this purpose, export the translations, then in the csv file
the new voice messages hinzufügenund then reimport the modified CSV file.
Not existing translations of a new language to be supplemented with the English name
and can be corrected later.

5. Table Views
By right-clicking on the table header (upper left small field)
A dialog box opens, with the table view can be customized.
The width of the columns can also be customized by dragging and dropping.
By briefly opening the dialogue this column settings are then also taken.

6. ComboBoxes
While the combo boxes have fixed items. In many cases, however, one has the possibility
write any custom values ??(for example, in "Max. Trip Distance")

7. Service Form SQL
Below there is the possibility of directly depose sql commands.
This is intended for debugging and for service activities.
If you are not sure, then do not put your fingers on it.
It is possible to destroy internal data.
If multiple commands are entered, this can be targeted by selection
Choose to run.

8. EDDN
The EDDN interface is disabled by default. About Settings-> EDDN Interface
, the receipt and / or sending messages EDDN be activated.
To receive EDDN data automatically at the next start, must
"Import received data into database" and "auto start listening on program start"
to be activated. Please consider also Section 9
For sending data in the EDDN the same applies.

To filter out garbage is the window of valid data + - 5 minutes -
the time of your own computer should therefore reasonably agree. Multiple incoming
Data (station + goods) from EDDN also be filtered within a 5-minute period.

9. consideration of the systems / stations !!!!!!!
ED-IBE considered in the normal case, only data from systems that have already been visited itself!

However, further data be provided (via the manual CSV import or the EDDN interface).
Through this mechanism, we consciously decide against collecting price data of unknown systems,
but still receive updates of prices of visited systems over EDDN.

Who wants to have a lump sum to ALL data considered must activate it.
The switch for this is found in the Settings (-> Data filter) and can changed at any time
will.

Note: Those who imported his old RN data, adds the entries to the list of visited systems.
This is based on import, the "Commander's Log" and "Station History" of "RegulatedNoise DJ".
Google Übersetzer für Unternehmen:Translator ToolkitWebsite-Übersetzergoogle
Über Google ÜbersetzerCommunityMobil
Über GoogleDatenschutzerklärung & NutzungsbedingungenHilfeFeedback geben
