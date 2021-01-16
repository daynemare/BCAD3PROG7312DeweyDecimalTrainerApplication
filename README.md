# BCAD3PROG7312DeweyDecimalTrainerApplication
# Project Name
 Dewey Decimal Trainer
# Version
 1.2
# Description
 The Dewey Decimal Trainer is software application written in C# and designed for a local library to make learning the dewey decimal classification system a fun 
 and engaging experience for the user through gamification. v1.2 added a feature that focuses on training library users on how to find a specific book call number in the dewey decimal classification hierarchy through a quiz. 
 

# Project Run Requirements
1.	Install the latest version of Visual Studio 2017 or 2019 IDE.

# Installation/Run Guide
 1.	Navigate to the project folder titled “DeweyDecimalTrainerApplication” - do not remove any files from this folder.
 2.     Inside this folder are 2 files "DeweyDecimalTrainerApplication"(the base application) and "DataAccessLayer" (database helper classes that connect to the app as a dll and assist with database operations). 
	Click on the "DeweyDecimalTrainerApplication" folder.
 3.	Once inside this folder, double click on the "DeweyDecimalTrainerApplication.sln" Visual Studio Solution file which should open the project solution in Visual Studio.
 4.     Restore Nuget Packages - Nuget Packages were deleted as they were over 100mbs which meant that the assignment could not be uplodaded on teams with them intact. However, Visual Studio should restore them once the project is opened. 
 5.	Once the project solution has loaded and Nuget packages have been restored, click green application play button labeled start to run the application.
 6.	If the steps were followed correctly the user should see the Login/Register screen at the center of their screen. They can now begin by using the application by signing in using the demo user
        account information listed below or they can create a new account through the registration page of the applciation. 

# Demo User Account Information

 Library Card ID number(Username):18002054
 Password: Test1234

# PC used for Prototype Development and Testing
OS Name					Microsoft Windows 10 Home
Version					10.0.18363 Build 18363
Other OS Description 			Not Available
OS Manufacturer				Microsoft Corporation
System Name				DESKTOP-3F3DGS8
System Manufacturer			HP
System Model				HP Pavilion Notebook
System Type				x64-based PC
System 					SKU	N9E13UA#ABA
Processor                               AMD A10-8700P Radeon R6, 10 Compute   Cores 4C+6G, 1800 Mhz, 2 Core(s), 4 Logical Processor(s)
BIOS Version/Date                       American Megatrends Inc. F.45,2017/04/28
SMBIOS Version				2.8
Embedded Controller Version		81.32
BIOS Mode				UEFI
BaseBoard Manufacturer			HP
BaseBoard Product				80B0
BaseBoard Version				81.32
Platform Role				Mobile
Secure Boot State				On
PCR7 Configuration			Elevation Required to View
Windows Directory				C:/WINDOWS
System Directory				C:/WINDOWS/system32
Boot Device				/Device/HarddiskVolume1
Locale					South Africa
Hardware Abstraction Layer			Version = "10.0.18362.752"
User Name				DESKTOP-3F3DGS8\Dayne
Time Zone				South Africa Standard Time
Installed Physical Memory (RAM)		8,00 GB
Total Physical Memory			7,45 GB

# Development Tools
•	Visual Studio (IDE) 
•	C# .Net framework 4.6.1 WPF(Written in)
•	SQLite.EF6(Database framework) with Dapper(object-relational mapping)

# Application Database 
-The applications local database file resides in the "Debug" folder of the base application 
 and is titled "DeweyDecimalTrainerApplicationDb.db". The database tables and its data can be viewed using a tool such as "SQLLiteStudio".

# Author
 Dayne Mare`
