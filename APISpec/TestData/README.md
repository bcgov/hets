TEST DATA
=========
----------

Requirements:
-------------

  1. An instance of the HETS Server application running in Development mode
  2. An account with Admin level access in the above instance of the HETS Server
  3. Node.js version of mustache installed
  4. Python 2.7 installed
  5. Curl installed 

Basic Instructions
--------------

1. Open the Excel spreadsheet located inside the "in" folder
2. Run the CTRL-SHIFT-V macro to export the contents of the spreadsheet
3. Run update.bat 
4. Edit the load-all.bat file, setting the user and server hostname appropriately
5. Run load-all.bat
6. Review the output of the batch process and server logs and correct any issues found 

Detailed Description
-----------------
1. The process starts with Excel.  
	1. A macro available at CTRL-SHIFT-V is used to export from Excel to CSV
	2. The macro loops through the sheets in the workbook and saves each with a name ending in .csv to the file system.
2. update.bat converts the CSV files to JSON using a small python program
3.  These JSON files are then fed to the bulk upload services using curl


Troubleshooting
---------------
If you run into problems, the most common cause is the JSON data being sent to the server is not valid, either because the JSON schema does not match what the server is expecting, or because invalid characters are in the JSON.

There is a tool, JSONValidator, included in the source for the API Server that can be used to validate the JSON data.  The JSONValidator tool accepts as input one JSON encoded object of a type support by the application; since the JSON files produced by update.bat contain several items in a vector, you will have to extract a single object from the vector and save that to a new file for validation.

OpenShift / SiteMinder also have limits on processing time.  If you get 503 errors when uploading data, however the services are online, then you need to reduce the size of the bulk data json files.   Either adjust the excel file to not have so much data, or split the JSON into multiple files and alter load-all.bat to use the split files.
