namespace Duets.Cli.Components.Commands.Cheats

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
open Duets.Data
open Duets.Entities
open Duets.Simulation
open FSharp.Data.UnitSystems.SI.UnitNames

[<RequireQualifiedAccess>]
module BandCommands =
    /// Allows the player to change the number of fans the band has.
    let pactWithTheDevil =
        { Name = "pact with the devil"
          Description =
            "Allows you to change how many fans you have... for a price. Nah, not really, go nuts."
          Handler =
            (fun _ ->
                let band = Queries.Bands.currentBand (State.get ())

                let allCities = Queries.World.allCities

                let chosenCity =
                    showChoicePrompt
                        "Where do you want to change your fans?"
                        (fun (city: City) -> Generic.cityName city.Id)
                        allCities

                let fansInCity = Queries.Bands.fansInCity' band chosenCity.Id

                let fans =
                    $"You currently have {fansInCity}, how many fans do you want there?"
                    |> Styles.prompt
                    |> showNumberPrompt
                    |> (*) 1<fans>

                let updatedFans = band.Fans |> Map.add chosenCity.Id fans

                BandFansChanged(band, Diff(band.Fans, updatedFans))
                |> Effect.apply

                Scene.Cheats) }

    /// Creates 10 high-quality songs and releases them as an album.
    let makeMeABand =
        { Name = "make me a band"
          Description =
            "Creates 10 high-quality songs and releases them as an album following your band's genre."
          Handler =
            (fun _ ->
                let state = State.get ()
                let band = Queries.Bands.currentBand state
                let today = Queries.Calendar.today state

                let songs =
                    [ 1..10 ]
                    |> List.map (fun i ->
                        let adjective = List.sample Words.adjectives
                        let noun = List.sample Words.nouns
                        let songName = $"{adjective} {noun}"

                        let length =
                            { Minutes = Random.between 3 5 * 1<minute>
                              Seconds = Random.between 0 59 * 1<second> }

                        let vocalStyle = List.sample VocalStyles.all

                        let song = Song.from songName length vocalStyle

                        let quality =
                            85<quality> + (Random.between 0 15) * 1<quality>

                        Finished(song, quality))

                songs
                |> List.iter (fun finishedSong ->
                    SongFinished(band, finishedSong, today) |> Effect.apply)

                let recordedSongs =
                    songs
                    |> List.map (fun (Finished(song, quality)) ->
                        let recordedQuality = quality + 5<quality>
                        Recorded(song, recordedQuality))

                let albumName =
                    $"{List.sample Words.adjectives} {List.sample Words.nouns}"

                let firstRecordedSong = List.head recordedSongs
                let (Recorded(firstSong, firstQuality)) = firstRecordedSong

                let album =
                    Album.from
                        band
                        albumName
                        (Recorded(firstSong.Id, firstQuality))

                let albumWithAllSongs =
                    recordedSongs |> Album.updateTrackList album

                let unreleasedAlbum =
                    { Album = albumWithAllSongs
                      SelectedProducer = SelectedProducer.PlayableCharacter }

                AlbumStarted(band, unreleasedAlbum) |> Effect.apply

                let releasedAlbum =
                    Album.Released.fromUnreleased unreleasedAlbum today 1.0

                AlbumReleased(band, releasedAlbum) |> Effect.apply

                "There you go, no need to make any effort!"
                |> Styles.success
                |> showMessage

                Scene.Cheats) }
