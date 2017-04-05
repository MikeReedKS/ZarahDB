# ZarahDB
Open Source NoSQL Database for Unique Key Tables

Quick Start Tutorial

I'll get better instructions up before the full release, but hopefully this will help get you started. You might want to look at the Overview before diving into the tutorial.
The basic concept of a scattered database is that all data is stored as JSON files scattered across the file system in a logical hierarchy. Here are some concepts that will help you visualize how the system works:

It's a database, but it is based on scattering data in files, like this: (If this is confusing at all, skip straight to the Quick Start section)
On the disk there is a top level folder called an instance. Under the instance are sub-folders called tables. 
Tables have a hierarchy of folders based on the key and the selected max depth (default 5) using the first letters of the key.
In the lowest folder of the hierarchy there will be a JSON file, named the same as the key (or close).
In the JSON is a node named after the key, and that node has an array of column/value pairs.

Instance = Root Folder for the scattered database (Zarah is Hebrew for Scattered).
Table = Sub-folder. In standard DB terms the table can be said to contain any number of rows, each row is accessed by its key.
Key = A single JSON file that contains all the data associated with the key, which is stored as columns, each column having a value.
Column = The name for the name/value pair to be stored or retrieved.
Value = A string value, which can be anything from null, a number, a set of typed characters or even a full JSON blob.

To get started, all you need to do is create a new instance and then start storing and retrieving values.
Quick Start

Using the WebAPI from the Swagger UI (Or from any language)

The Swagger UI makes using the web interface very straight forward.

Create: Use "GET /Instance/Exists" to test if we already have an instance named "Test".
If it exists, go to step 2.
If it doesn't exist, then use the "POST /Instance" to create a "Test" instance.
Note that you don't really need to create anything, when you write to the DB, that will create everything automatically.
Write: Use "POST /Value" with the instance "Test", the table "Test Table", the Key "Test Key", the column of "Test Column" and a value of "Test Value".
Read: Use "GET /Value" using "Test" as the instance, "Test Table" as the table, "Test Key" as the key and "Test Column" as the column and check that you receive the value of "Test Value" in return.
Using the DLL in your C# project

The ZarahDB_Library.dll file is all you need to add a scattered database to your C# project. Add this DLL and add a reference to it in your project. You will also need the Newtonsoft.Json NuGet package. You can get the ZarahDB_Library from https://www.nuget.org/packages/ZarahDB.Library or using the packet manager in Visual Studio.

In Visual Studio, create a new project called "ZarahDB_HelloWorld".
Right click the "ZarahDB_HelloWorld" project in the Solution Explorer and click "Open Folder in File Explorer". This will open a new file explorer window showing the files in your new project.
Copy the ZarahDB_Library.dll file into the project folder. (You can right click it above to download the latest version)
Add a reference to the ZarahDB_Library.dll library to your project.
Click "Program.cs" in the Solution Explorer and replace the contents of that file with this:

```C#
using System;
using ZarahDB_Library;

namespace ZarahDB_HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            var instance = new Uri(@"C:\ZDB");
            var table = "Test Table";
            var key = "Test Key";
            var column = "Test Column";
            var value = "Test Value";

            //Store a value in the database
            var putResult = ZarahDB.Put(instance, table, key, column, value);

            //Read a value from the database
            var getResult = ZarahDB.Get(instance, table, key, column);
            Console.WriteLine(getResult.Value);

            //Pause, awaiting a key press
            Console.ReadKey();
        }
    }
}
```

The Results

Either using the API or the the code, the result is one row written to a table called "Test_Table" for the Key "Test Key". The row contains a single column called "Test Column" and has a value of "Test Value". The result will be a single JSON file in the `C:/zdb/Test_Table/t/e/s/t/_` folder called `test key.json`. It's contents will look like the following JSON. (It won't be formatted, but it will contain the same structure and data)

```JSON
{
    "Keys": [
        {
            "Key": "Test Key",
            "ColumnValues": [
                {
                    "Column": "Test Column",
                    "Value": "Test Value",
                    "PreviousValue": null,
                    "Updated": "636267579942863287"
                }
            ]
        }
    ]
}
```

You can now read and write data to a ZarahDB database. It really is that easy. Now you need to decide if you will store values as Name/Value pairs or as documents. The examples above used a Name/Value pair. For each column, we assume there will be a single value.

Going the Document DB route

We can also approch the value as a document, then it can contain any type of data. A common thing to store is a JSON blob. Using the Json.Net library from Newtonsoft we can simply serialize and deserialize any object type into a JSON Blob using the following code:

```C#
using System;
using Newtonsoft.Json;
using ZarahDB_Library;

namespace ConsoleApplication1
{
    class Program
    {
        public class ComplexObject //This could be any variable or structure in our program, from simple to very complex.
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime DateOfBirth { get; set; }
        }

        static void Main(string[] args)
        {
            var instance = new Uri(@"C:\ZDB");
            var table = "Test Table";
            var key = "Test Key";
            var column = "Test Column";

            //Create a value that is a document (JSON in this case)
            var complexObject = new ComplexObject
            {
                FirstName = "Mike",
                LastName = "Reed",
                DateOfBirth = DateTime.Parse("06/24/1962")
            };
            var value = JsonConvert.SerializeObject(complexObject);

            //Store a value in the database
            var putResult = ZarahDB.Put(instance, table, key, column, value);
            Console.WriteLine(putResult.Status);

            //Read a value from the database
            var getResult = ZarahDB.Get(instance, table, key, column);
            Console.WriteLine(getResult.Value);

            //Pause, awaiting a key press
            Console.ReadKey();
        }
    }
}
```

Additionally we can use `complexObject = JsonConvert.DeserializeObject< ComplexObject>(value);` to get the value converted back from a document (JSON Blob in this case) to a complex object used in our program. Imagine how easy this makes storing your settings or other program data, no longer do you need to map to and from tables, think about what data goes where or many of the complexities of storing data in a typical SQL database.

Where would I use this?

Anywhere that you have a unique ID that maps to a set of data. That's not every case, but it happens quite frequetly. If you have objects in your program that you reference by ID (number, word, anything unique) then you can save the entire object into a ZarahDB and extract it again, each in a single line of code. How cool is that?

There are many uses for a ZarahDB, so how will you use it?
