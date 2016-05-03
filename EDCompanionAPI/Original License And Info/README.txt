# Elite Companion API (from https://github.com/ZNS/EliteCompanionAPI)

This is a class library in c# for accessing the official api exposed by the Elite Dangerous Companion. This not an official library and is not endorsed by Frontier developments.

The library allows to manage multiple logins. Information about each login is stored on disk and the password is encrypted using the machine key. NOTE: cookie data is not encrypted at this point. Profiles will be stored at path ".\" by default. This can be overriden by adding an AppSetting with key "zns.elitecompanion.datapath", allows absolute and relative paths.

On first login an email will be sent from Frontier with a verification code. This can then be verified using the library.

Profile data is returned as raw json and is cached for 60 seconds. This means you cannot pull the server for fresh data more often than that.

An example on how to use the library can be seen in the EliteCompanionCommand-app.
