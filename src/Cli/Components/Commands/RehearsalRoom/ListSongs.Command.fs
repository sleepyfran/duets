namespace Cli.Components.Commands

open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities

[<RequireQualifiedAccess>]
module ListSongsCommand =
    let private showUnfinishedTable
        columns
        (unfinishedSongs: UnfinishedSongWithQualities list)
        =
        let rows =
            unfinishedSongs
            |> List.map (fun (UnfinishedSong song, _, quality) ->
                [ song.Name
                  Songs.length song.Length
                  $"{Styles.Level.from quality}%%"
                  "-" ])

        showTableWithTitle "Unfinished songs" columns rows

    let private showFinishedTable
        columns
        (finishedSongs: FinishedSongWithQuality list)
        =
        let rows =
            finishedSongs
            |> List.map (fun (FinishedSong song, quality) ->
                [ song.Name
                  Songs.length song.Length
                  $"{Styles.Level.from quality}%%"
                  $"{Styles.Level.from song.Practice}%%" ])

        showTableWithTitle "Finished songs" columns rows

    /// Command to list all songs, finished and unfinished.
    let create
        (unfinishedSongs: UnfinishedSongWithQualities list)
        (finishedSongs: FinishedSongWithQuality list)
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
