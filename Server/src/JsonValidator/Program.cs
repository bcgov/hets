using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using HETSAPI.Models;

namespace JsonValidator
{
    public static class Program
    {        
        public static void Main(string[] args)
        {
            AssemblyName name = new AssemblyName("HETSAPI");            
            Assembly assembly = Assembly.Load(name);
            
            // if no args, print help
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("JsonValidator.exe <Type> <File Name to validate>");
                Console.WriteLine("Where Type is one of:");
               
                List<Type> types = assembly.GetTypes().Where(t => t.GetTypeInfo().IsSubclassOf(typeof(AuditableEntity))).ToList();

                foreach (Type type in types)
                {
                    Console.WriteLine (type.Name);                    
                }
            }
            else
            {
                string classname = "HETSAPI.Models." + args[0];
                string filename = args[1];
                bool success = true;

                if (File.Exists(filename))
                {
                    Console.WriteLine("Validating file " + filename + " for type " + classname);

                    object sampleObject = assembly.CreateInstance(classname);

                    if (sampleObject != null)
                    {
                        string json = File.ReadAllText(filename);
                        try
                        {
                            JsonConvert.DeserializeObject (json, sampleObject.GetType());
                        }
                        catch (Exception e)
                        {
                            success = false;
                            Console.WriteLine("Filename " + filename + " does not contain a valid object of type " + classname);
                            Console.WriteLine("Errors:");
                            Console.Write(e.Message);
                        }

                        if (success)
                        {
                            Console.WriteLine("Filename " + filename + " contains a valid object of type " + classname);
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine("Unable to find type " + classname);
                    }
                }
                else
                {
                    Console.WriteLine("Filename specified does not exist.");
                }                                    
            }
        }
    }
}
