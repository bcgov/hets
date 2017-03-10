Hired Equipment Tracking System (HETS)
======================

# JSON Validator #

This tool can be used to validate data that will be uploaded to any of the API services.  It is useful if you have data that does not follow the schema, and you need to correct the data.

To use:

1) Install .NET Core 1.1 (the same as required to run the application).  
2) To run from the command line, execute the following command from the project folder:

	dotnet run

This will show the help for the program, including a list of the object types supported.

3) Execute with the parameters <Type> <Filename> to parse the file.

For example, to parse a file containing a record of type Equipment, do the following:
    
    dotnet run Equipment equipment.json
    
Sample output, from an invalid file:
    
    Validating file equipment.json for type HETSAPI.Models.Equipment
    Filename equipment.json does not contain a valid object of type HETSAPI.Models.Equipment
    Errors:
    Cannot deserialize the current JSON array (e.g. [1,2,3]) into type 'HETSAPI.Models.DumpTruck' because the type requires a JSON object (e.g. {"name":"value"}) to deserialize correctly.
    To fix this error either change the JSON to a JSON object (e.g. {"name":"value"}) or change the deserialized type to an array or a type that implements a collection interface (e.g. ICollection, IList) like List<T> that can be deserialized from a JSON array. JsonArrayAttribute can also be added to the type to force it to deserialize from a JSON array.
    Path 'dumpTruck', line 8, position 22.

If we look at line 8, position 22, it has the following text:

	"dumpTruck": [],

Equipment has the following definition for dumpTruck:

	public DumpTruck DumpTruck { get; set; }

So the problem here is that DumpTruck is an object, however is being classed as an array in the data file.

To correct this issue, the line in the json file must be changed to:

	"dumpTruck": null,

(Or a full DumpTruck object)

Note that the validator will stop at the first error.  You will need to run the validator multiple times if there is more than one error in the data. 
