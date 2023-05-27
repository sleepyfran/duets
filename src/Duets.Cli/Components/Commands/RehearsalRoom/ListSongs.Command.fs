namespace Duets.Cli.Components.Commands

open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities

[<RequireQualifiedAccess>]
module ListSongsCommand =
    let private showUnfinishedTable
        columns
        (unfinishedSongs: Unfinished<Song> list)
        =
        let rows =
            unfinishedSongs
            |> List.map (fun (Unfinished(song, _, quality)) ->
                [ song.Name
                  Songs.length song.Length
                  $"{Styles.Level.from quality}%%"
                  "-" ])

        showTableWithTitle "Unfinished songs" columns rows

    let private showFinishedTable
        columns
        (finishedSongs: Finished<Song> list)
        =
        let rows =
            finishedSongs
            |> List.map (fun (Finished(song, quality)) ->
                [ song.Name
                  Songs.length song.Length
                  $"{Styles.Level.from quality}%%"
                  $"{Styles.Level.from song.Practice}%%" ])

        showTableWithTitle "Finished songs" columns rows

    /// Command to list all songs, finished and unfinished.
    let create
        (unfinishedSongs: Unfinished<Song> list)
        (finishedSongs: Finished<Song> list)
        =
        { Name = "list songs"
          Description =
            "Lists all the songs, finished and unfinished, that your band has created"
          Handler =
            (fun _ ->
                let columns =
                    [ "Name"; "Length"; "Quality"; "Practice" ]
                    |> List.map Styles.header

                showUnfinishedTable columns unfinishedSongs
                showFinishedTable columns finishedSongs

                Scene.World) }
