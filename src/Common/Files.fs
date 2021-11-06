module Common.Files

open System
open System.IO

/// Returns the path to the Duets folder where we are storing the savegame
/// and other config files.
let duetsFolder () =
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
    |> fun appDataPath -> Path.Combine(appDataPath, "Duets")
    |> fun duetsPath ->
        Directory.CreateDirectory duetsPath |> ignore
        duetsPath

/// Returns the path to the savegame file.
let savegamePath () =
    duetsFolder ()
    |> fun duetsPath -> Path.Combine(duetsPath, "savegame.json")
    
/// Returns the path to the log file.
let logPath () =
    duetsFolder ()
    |> fun duetsPath -> Path.Combine(duetsPath, "activity.log")

/// Defines the key of the JSON data to fetch.
type DataKey =
    | Genres
    | Npcs
    | Studios

/// Returns the full path to a data file located in the Data folder.
let dataFile key =
    let dataDirectory =
        Directory.GetParent(__SOURCE_DIRECTORY__)
        |> fun baseDir -> Path.Combine(baseDir.FullName, "Database/Data")

    match key with
    | Genres -> Path.Combine(dataDirectory, "genres.json")
    | Npcs -> Path.Combine(dataDirectory, "npcs.json")
    | Studios -> Path.Combine(dataDirectory, "studios.json")

/// Attempts to read all text from a given file and returns an option with the
/// text.
let readAll path =
    try
        File.ReadAllText path |> Some
    with _ -> None

/// Writes the content in the specified path. Creates the file if it's not
/// created already. Notice that while the file is automatically created the
/// parent folders are not, so if the path is `/a/b/c.txt` and the b folder
/// is not present, the file will not be written.
let write path content = File.WriteAllText(path, content)

/// Appends the content to the specified path. Creates the file if it's not
/// created already. Notice that while the file is automatically created the
/// parent folders are not, so if the path is `/a/b/c.txt` and the b folder
/// is not present, the file will not be written.
let append path content = File.AppendAllText(path, content)
