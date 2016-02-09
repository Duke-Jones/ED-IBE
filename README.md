#ED-IBE
####(ED-Intelligent Boardcomputer Extension)  
for Elite Dangerous 

    Trading Tool
    Commanders Log
    view of systemdata
    systemdata based on EDDB
    built-in, quick OCR
    plausibilitycheck for scanned marketdata
    optional connection to EDDN (export and/or import)
    open source

ED-IBE is the successor of "**RegulatedNoise DJ-Version**".
Reason for re-creating RegulatedNoise was the inefficient
internal data structure. ED-IBE uses a minimized version 
of a **MySQL** database which is **much** more powerful. 
The database runs as a normal process (not as a service on you pc) 
\- and only if ED-IBE is running.

**Comparison to the old RegulatedNoise for the current release:**

**working:**
* PriceAnalysis
* Commanders Log
* getting market data by EDMC (fully integrated by EDMC commandline version, but has to be installed manually)
* overtaking existing data from the old RegulatedNoise

**improvements:**
* getting market data by EDMC (fully integrated by EDMC commandline version, but has to be installed manually)
* filter for landingpad size
* age of data in price analysis
* dynamic sorting of data tables
* data tables sortable by distance
* adjustable data tables
* much quicker
* autoevents for "Visited" and "Market data Collected" working again (with EDMC)

**not working yet:**
* view of systems and stations
* ocr (take EDMC instead)
* webserver
* EDDN interface
* no re-usable import and export of market data at the moment (for exchange with friends)

**Installation:**  
Get the latest release from  
	https://github.com/Duke-Jones/ED-IBE/releases.  
The exe file is a installer which guides you through the steps.  

**Forums:**  
on forums.frontier.co.uk (english forum):  
https://forums.frontier.co.uk/showthread.php?t=137732

on elitedangerous.de (deutsches Forum):  
http://www.elitedangerous.de/forum/viewtopic.php?f=66&t=6404&start=0
