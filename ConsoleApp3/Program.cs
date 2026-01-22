using System;
using System.IO;
using System.Reflection;

// NOTE: This file contains very detailed examples for Attributes and File Handling.
// NOTE: Every comment is written as a "note" to explain what the code is doing.

/// <summary>
/// NOTE: This custom attribute will be used to tag classes and methods with metadata.
/// NOTE: Attributes are like "labels" you can attach to code so you can read them later.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class ExampleNoteAttribute : Attribute
{
    // NOTE: This is a property that stores the note text.
    public string Text { get; }

    // NOTE: This constructor lets us set the note text when we apply the attribute.
    public ExampleNoteAttribute(string text)
    {
        Text = text;
    }
}

/// <summary>
/// NOTE: This class is the startup class for the project (contains Main).
/// NOTE: We will run two detailed examples: Attributes and File Handling.
/// </summary>
[ExampleNote("MainClass demonstrates Attribute usage and File handling step-by-step.")]
public class MainClass
{
    /// <summary>
    /// NOTE: Main is the entry point of this console app.
    /// NOTE: The project should be configured so this class is the startup class.
    /// </summary>
    public static void Main(string[] args)
    {
        // NOTE: Provide a clear header so users know what the program is doing.
        Console.WriteLine("=== Detailed Examples: Attributes and File Handling ===");
        Console.WriteLine();

        // NOTE: Run the Attribute example first so we learn metadata basics.
        RunAttributeExample();

        // NOTE: Add a blank line to visually separate the examples.
        Console.WriteLine();

        // NOTE: Run the File Handling example next so we learn how to read/write files.
        RunFileHandlingExample();

        // NOTE: Pause at the end so the console window stays open.
        Console.WriteLine();
        Console.WriteLine("Press ENTER to exit...");
        Console.ReadLine();
    }

    /// <summary>
    /// NOTE: This method demonstrates how to define, apply, and read Attributes.
    /// </summary>
    [ExampleNote("Attribute example method.")]
    private static void RunAttributeExample()
    {
        // NOTE: Start by describing the example in the console output.
        Console.WriteLine("[Attribute Example]");

        // NOTE: Get the Type object for MainClass so we can inspect its attributes.
        Type targetType = typeof(MainClass);

        // NOTE: Read all ExampleNoteAttribute instances from the class itself.
        ExampleNoteAttribute[] classNotes = targetType.GetCustomAttributes<ExampleNoteAttribute>(inherit: false);

        // NOTE: Print how many attributes were found on the class.
        Console.WriteLine($"Class '{targetType.Name}' has {classNotes.Length} ExampleNoteAttribute(s).");

        // NOTE: Loop over each attribute instance and print its Text property.
        foreach (ExampleNoteAttribute note in classNotes)
        {
            Console.WriteLine($" - Class Note: {note.Text}");
        }

        // NOTE: Get the MethodInfo for RunAttributeExample so we can read its attributes.
        MethodInfo? methodInfo = targetType.GetMethod(nameof(RunAttributeExample), BindingFlags.NonPublic | BindingFlags.Static);

        // NOTE: Ensure we found the method before reading attributes.
        if (methodInfo is not null)
        {
            // NOTE: Get the attributes applied to this method.
            ExampleNoteAttribute[] methodNotes = methodInfo.GetCustomAttributes<ExampleNoteAttribute>(inherit: false);

            // NOTE: Print how many attributes were found on the method.
            Console.WriteLine($"Method '{methodInfo.Name}' has {methodNotes.Length} ExampleNoteAttribute(s).");

            // NOTE: Display each note from the method attribute list.
            foreach (ExampleNoteAttribute note in methodNotes)
            {
                Console.WriteLine($" - Method Note: {note.Text}");
            }
        }
        else
        {
            // NOTE: If the method wasn't found, tell the user (this should not happen).
            Console.WriteLine("RunAttributeExample method could not be found via reflection.");
        }
    }

    /// <summary>
    /// NOTE: This method demonstrates detailed File Handling.
    /// NOTE: It shows creating a directory, writing a file, appending, and reading.
    /// </summary>
    [ExampleNote("File handling example method.")]
    private static void RunFileHandlingExample()
    {
        // NOTE: Start by describing the example in the console output.
        Console.WriteLine("[File Handling Example]");

        // NOTE: Decide where to create example files.
        // NOTE: Environment.CurrentDirectory is usually the app's working folder.
        string baseFolder = Path.Combine(Environment.CurrentDirectory, "ExampleFiles");

        // NOTE: Create the directory if it doesn't already exist.
        Directory.CreateDirectory(baseFolder);
        Console.WriteLine($"Created/confirmed folder: {baseFolder}");

        // NOTE: Build a file path inside the ExampleFiles directory.
        string filePath = Path.Combine(baseFolder, "notes.txt");

        // NOTE: Prepare the initial content for the file.
        // NOTE: We use Environment.NewLine to ensure correct line breaks on any OS.
        string initialContent = "NOTE: This file was created by the File Handling example."
            + Environment.NewLine
            + "NOTE: The next step will append more lines.";

        // NOTE: Write the initial content to the file (overwrites if it exists).
        File.WriteAllText(filePath, initialContent);
        Console.WriteLine($"Wrote initial content to: {filePath}");

        // NOTE: Prepare additional content that will be appended to the file.
        string appendedContent = Environment.NewLine
            + "NOTE: This line was appended later."
            + Environment.NewLine
            + $"NOTE: Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";

        // NOTE: Append the content instead of overwriting the file.
        File.AppendAllText(filePath, appendedContent);
        Console.WriteLine("Appended additional content.");

        // NOTE: Read the file contents back into a string.
        string finalContent = File.ReadAllText(filePath);

        // NOTE: Display the final file contents so the user can verify the result.
        Console.WriteLine("Final file contents:");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine(finalContent);
        Console.WriteLine("----------------------------------------");

        // NOTE: Demonstrate basic file info (size and last write time).
        FileInfo info = new FileInfo(filePath);
        Console.WriteLine($"File size (bytes): {info.Length}");
        Console.WriteLine($"Last modified: {info.LastWriteTime}");
    }
}
