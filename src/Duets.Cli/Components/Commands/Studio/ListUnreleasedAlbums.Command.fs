namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module ListUnreleasedAlbumsCommand =
    /// Command to list all unreleased albums.
    let create unreleasedAlbums =
        { Name = "list albums"
          Description = "Shows all unreleased albums"
          Handler =
            (fun _ ->
                let columns =
                    [ Styles.header "Name"
                      Styles.header "Type"
                      Styles.header "Total time"
                      Styles.header "Tracklist" ]

                let rows =
                    unreleasedAlbums
                    |> List.map (fun (UnreleasedAlbum album) ->
                        let albumTrackList =
                            Queries.Albums.trackList (State.get ()) album

                        let trackList =
                            albumTrackList
                            |> List.fold
                                (fun (idx, acc) (Recorded(song, quality)) ->
                                    let updatedIdx = idx + 1

                                    let separator =
                                        if
                                            updatedIdx
                                            <> album.TrackList.Length
                                        then
                                            "\n"
                                        else
                                            ""

                                    updatedIdx,
                                    $"{acc}{updatedIdx} - {Generic.songWithDetails song.Name quality song.Length}{separator}")
                                (0, "")
                            |> snd

                        let formattedLength =
                            Album.lengthInSeconds albumTrackList
                            |> Time.Length.fromSeconds
                            |> Result.unwrap
                            |> Generic.length

                        [ album.Name
                          Generic.albumType album.Type
                          formattedLength
                          trackList ])

                showTable columns rows

                Scene.World) }
