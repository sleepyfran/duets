module Duets.Cli.Scenes.Phone.Apps.Statistics.Root

open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text

type private StatisticsOption =
    | Band
    | Albums
    | Reviews
    | Relationships

let private textFromOption opt =
    match opt with
    | Band -> "Band statistics"
    | Albums -> "Album statistics"
    | Reviews -> "Album reviews"
    | Relationships -> "Relationships"

let rec statisticsApp () =
    let selectedChoice =
        showOptionalChoicePrompt
            Phone.statisticsAppSectionPrompt
            Generic.backToPhone
            textFromOption
            [ Band; Albums; Reviews; Relationships ]

    match selectedChoice with
    | Some Band -> Band.bandStatisticsSubScene statisticsApp
    | Some Albums -> Albums.albumsStatisticsSubScene statisticsApp
    | Some Reviews -> AlbumReviews.reviewsStatisticsSubScene statisticsApp
    | Some Relationships -> Relationships.relationshipsStatisticsSubScene statisticsApp
    | None -> Scene.Phone
