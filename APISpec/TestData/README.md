HETS TEST DATA
=========

This note covers maintaining the test data for HETS and loading the test data into a fresh instance of the HETS database. If you are only responsible for resetting the database, you just need to review the next section (Requirements), and then jump down to the
sections on resetting the database and loading test data.

Table of Contents
=================

   * [HETS TEST DATA](#hets-test-data)
   * [Table of Contents](#table-of-contents)
      * [Requirements:](#requirements)
            * [Maintaining Test Data](#maintaining-test-data)
            * [Loading Test Data](#loading-test-data)
      * [Maintaining Test Data Instructions](#maintaining-test-data-instructions)
         * [Conventions in the Excel File:](#conventions-in-the-excel-file)
            * [Update.bat](#updatebat)
      * [Managing the Seeder data via OpenShift secrets](#managing-the-seeder-data-via-openshift-secrets)
      * [Resetting the Database and Reloading Test Data](#resetting-the-database-and-reloading-test-data)
         * [<em>Summary of the Process</em>](#summary-of-the-process)
         * [Detailed Steps to reset the Database for an environment](#detailed-steps-to-reset-the-database-for-an-environment)
         * [Detailed Steps: Reloading the Test Data](#detailed-steps-reloading-the-test-data)
         * [Verifying the Data](#verifying-the-data)
      * [Troubleshooting](#troubleshooting)

Requirements:
-------------

#### Maintaining Test Data

1. An Excel spreadsheet with a special macros to export worksheet data into csv.
2. Python 2.7 installed

#### Loading Test Data

1. An instance of the HETS Server application running in Development mode.
2. An account with Admin level access in the above instance of the HETS Server
3. Curl installed

Maintaining Test Data Instructions
--------------

1. Open the Excel spreadsheet located inside the "in" folder
2. Update as needed the test data.  See some details on the conventions in the Excel file.
3. Run the CTRL-SHIFT-V macro to export the contents of the spreadsheet to a series of CSV files.
   1. The macro walks through the worksheets in the host Excel Workbook and exports each tab with a ".csv" extension and saves it as a CSV file in the same folder as the Excel worksheet.
   2. By convention, a .gitignore entry ensures that the exported .csv files get pushed to the github repo.
4. Run the update.bat file to convert the data from .csv files to create a set of JSON files.

Those JSON files serve as inputs to corresponding bulk API endpoints.

### Conventions in the Excel File:

* The full set of conventions for the Excel will be documented separately in a separate github repo - *TO DO: Add reference*.
* The first column must be "File" - populate it with a common phrase. If some rows are NOT to be loaded, place a "-" in the column.
* Column names must match the data element names of the API to be used to load the test data.
* Columns with a "!" prefix are not included as element names in the JSON.
  * The purpose of the ! columns are to provide data for formulas in other columns that will be populated. For example:
     * A "!" column with the name of a localarea in text.
     * A "!" column that uses a lookup formula up to convert the text localarea name to the ID of the localarea name.  
     * A column called "localarea" that contains the ID of the localarea in a format suitable for loading into the JSON.
* Various techniques are used to create realistic looking data:
  * A "Names" worksheet that has a list of 150 names, addresses, etc.
  * Techniques for looking up ID for joins.
  * Formulii using Excel's "randbetween" function to randomize the data.
    * At some point, it's worth converting the data from formulii with random to hard coded so that test cases can be consistent.

#### Update.bat

The update.bat script calls a python script (csv2json.py) that iterates through the .csv files and generates a .json file.

## Managing the Seeder data via OpenShift secrets

Several tables (Users, District, Regions) are initially loaded into an empty database via a Seeder function in the Server code.  To update the data for those tables before reloading the data in the database:

Ready to reload the data.  Secrets process:

* Get copies of the secrets files from "server" node - `/secrets/*` (user.json, region.json, district.json)
  * Assume the files are in a folder named `hets-secret` for commands below
* Update the files as necessary - edit or regenerate. Ideally - use diff to check for differences.
* Run the following commands from the parent folder of the json files to delete the existing secrets and create new ones:
  * `oc delete secret hets-secret`
  * `oc secrets new hets-secret hets-secret/`
* Run the process below to reset the database.

Note that the seeder files are ONLY loaded when the database is empty. Updating the contents of the files without resetting the database will not affect the current data in the database.

## Resetting the Database and Reloading Test Data

### *Summary of the Process*

For the instance of the application to be reloaded with new test data:

1. Turn off the server - scale the replicas to 0
2. Log in to the Postgres container, drop the database, recreate it and grant rights to the new database.
3. Restart the server - scale the replicas to 1. This creates the database and then loads the seeder data needed to run the system.
4. Load the test data by executing the script - load-all.bat
5. Review the output of the batch process, server logs and the loaded data using the HETS app or the Swagger API. As necessary, adjust the test data or the script to correct any issues found.

### Detailed Steps to reset the Database for an environment

The following assumes you know OpenShift, have a command line open, have logged into OpenShift and are in the project in which you are resetting the data. *Be sure you are in the right project!!!*

1. Get the ID of the active replication controller (RC, rc) for the active `server` and `pdf` pods.
  1. Run the command `oc get rc` to get a list of RCs.
  2. Find the "server" and "pdf" RCs that have active containers (non-zero) - record those rc names (eg. server-141, pdf-14)
2. Get the name of the active postgres pod
  1. Run the command `oc get pods` and get the name of the postgres pod (e.g. postgresql-2-k0fql)
3. Scale down the server pod to 0:
   1. Run the command `oc scale --replicas=0 rc server-141 pdf-14`
4. Reset the database:
   1. Log into the postgres container: `oc rsh postgresql-2-k0fql`
   2. Run the command `psql -c "\du;"` to get a list of database users.
   2. Prepare (in a text editor), copy and then paste the following into the shell `psql -c "drop database hets;"; psql -c "create database hets;"; psql -c 'GRANT ALL ON DATABASE hets TO "user6DA";'; psql -c 'GRANT SELECT ON ALL TABLES IN SCHEMA publi c TO "shvj2me2";'; psql -c 'GRANT SELECT ON ALL TABLES IN SCHEMA public TO "gknjssqk";'; psql -c 'GRANT SELECT ON ALL TABLES IN SCHEMA public TO "gaj2bell";'`
      1. Replace the references to database users with those listed from running the commands. "user6DA" is replaced with the name of the database user known to the server, while the rest are individuals (such as from the MOTI Data Architecture group) that have read-only access to the database.
      2. The pasted string is series of Linux commands that execute the necessary sequence of database actions.
      2. Depending on the terminal you are using, the paste will not include a final "<CR>" and when you hit enter, the executions will occur.
      3. The feedback will be the results of the commands e.g. "Drop", "CREATE" and 1 or more "GRANT" lines.
5. Scale up the server pod to 1:
   1. Run the command `oc scale --replicas=1 rc server-141 pdf-14`
   2. Monitor the log of the new container until initialization is complete. It takes a little while as the database model is created and the seeder data is loaded. Watch for the following in the log: `Application started. Press Ctrl+C to shut down.`
6. As necessary, scale up the server pod further - likely to 2.

*NOTE*: You can perform all steps entirely within the OpenShift Console, including going into the Postgres container and clicking "Terminal" to execute the database commands.

### Detailed Steps: Reloading the Test Data

From the APISpec/Testdata folder, run the command:

* `load-all.bat <environment>`

Where <environment> is one of "dev", "test", or "prod" or the ID of the server

The `load-all.bat` script calls `load.bat`, which assumes a specific admin user is in the system. If not, change the user name in load.bat to one with admin access.

To run this step on test or prod, you must edit the Deployment Config for the "server" pod to set the environment variable `ASPNETCORE_ENVIRONMENT` to `Development` - AND CHANGE IT BACK AFTERWARDS!!  This setting allows users to access the server WITHOUT Siteminder.

*Aside: Accessing the database without Siteminder is OK in this case since what you are doing is loading the data with fresh data - there is no production data present. An intruder would still have to know the Siteminder ID of a registered user to gain access, and know about this temporary backdoor.*

### Verifying the Data

To verify the data:

1. Scan the output of "load-all.bat" to see that each command returned an HTTP Response of 2XX (200 or 204). If you see a stream of HTML in the output - there was a problem. Scan back to just before the HTML to see what API call caused the problem.
2. If it's likely the data is right (you are just tweaking the test data), the easiest way to verify the data is to log into the app and look at the data, including verifying what you changed.
3. If you want to review the data at each step. use the Swagger API explorer to run the API endpoints and scan the data.

## Troubleshooting

If you run into problems, the most likely cause is the JSON data being sent to the server is not valid, either because the JSON schema does not match what the server is expecting, or because invalid characters are in the JSON.

Often it is sufficient to review the Excel file for issues with a given API. Next, review the JSON and specifically, the API vs. the Swagger file for the API to make sure that all of the element names match. Failing that, look at the data in the Excel or JSON files that failed to load to see if there is a missing dependency. For example, the Equipment table is missing the required link to the DistrictEquipmentTable.

If you are a developer and have the application running locally within a .NET Core/Visual Studio environment, there is a tool, JSONValidator, included in the source for the API Server that can be used to validate the JSON data.  The JSONValidator tool accepts as input one JSON encoded object of a type support by the application. Since the JSON files produced by by the CSV to JSON process contain an array of objects, you will have to extract a single object from the vector and save that to a new file for validation.

OpenShift / SiteMinder have limits on processing time.  If you get 503 errors when uploading data, however the services are online, then you need to reduce the size of the bulk data json files.   Either adjust the excel file to not have so much data, or split the JSON into multiple files and alter load-all.bat to use the split files.
