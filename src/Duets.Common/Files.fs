module Duets.Common.Files

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

/// Returns the path to the settings file.
let settingsPath () =
    duetsFolder () |> fun duetsPath -> Path.Combine(duetsPath, "settings.json")

/// Returns the path to the savegame file.
let savegamePath () =
    duetsFolder () |> fun duetsPath -> Path.Combine(duetsPath, "savegame.json")

/// Returns the path to the savegame file given a root path.
let savegameFile path = $"{path}/savegame.json"

/// Returns the path to the log file.
let logPath () =
    duetsFolder () |> fun duetsPath -> Path.Combine(duetsPath, "activity.log")

/// Returns the path to the stats file.
let statsPath () =
    duetsFolder () |> fun duetsPath -> Path.Combine(duetsPath, "stats.json")

/// Defines the key of the JSON data to fetch.
type DataKey =
    | Adjectives
    | Adverbs
    | Books
    | Genres
    | Movies
    | Nouns
    | Npcs
    | Studios

/// Returns the full path to a data file located in the Data folder.
let dataFile key =
    let dataDirectory =
        Directory.GetParent(__SOURCE_DIRECTORY__)
        |> fun baseDir -> Path.Combine(baseDir.FullName, "Duets.Data/Resources")

    match key with
    | Adjectives -> Path.Combine(dataDirectory, "adjectives.json")
    | Adverbs -> Path.Combine(dataDirectory, "adverbs.json")
    | Books -> Path.Combine(dataDirectory, "books.json")
    | Genres -> Path.Combine(dataDirectory, "genres.json")
    | Movies -> Path.Combine(dataDirectory, "movies.json")
    | Nouns -> Path.Combine(dataDirectory, "nouns.json")
    | Npcs -> Path.Combine(dataDirectory, "npcs.json")
    | Studios -> Path.Combine(dataDirectory, "studios.json")

/// Attempts to read all text from a given file and returns an option with the
/// text.
let readAll path =
    try
        File.ReadAllText path |> Some
    with _ ->
        None

/// Writes the content in the specified path. Creates the file if it's not
/// created already. Notice that while the file is automatically created the
/// parent folders are not, so if the path is `/a/b/c.txt` and the b folder
/// is not present, the file will not be written.
let write (path: string) (content: string) = File.WriteAllText(path, content)

/// Appends the content to the specified path. Creates the file if it's not
/// created already. Notice that while the file is automatically created the
/// parent folders are not, so if the path is `/a/b/c.txt` and the b folder
/// is not present, the file will not be written.
let append (path: string) (content: string) = File.AppendAllText(path, content)

/// Deletes the file at the specified path.
let delete path = File.Delete path
