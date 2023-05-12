module Duets.Cli.Scenes.Phone.Apps.Statistics.Root

open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text

type private StatisticsOption =
    | Band
    | Albums
    | Reviews

let private textFromOption opt =
    match opt with
    | Band -> "Band statistics"
    | Albums -> "Album statistics"
    | Reviews -> "Album reviews"

let rec statisticsApp () =
    let selectedChoice =
        showOptionalChoicePrompt
            Phone.statisticsAppSectionPrompt
            Generic.backToPhone
            textFromOption
            [ Band; Albums; Reviews ]

    match selectedChoice with
    | Some Band -> Band.bandStatisticsSubScene statisticsApp
    | Some Albums -> Albums.albumsStatisticsSubScene statisticsApp
    | Some Reviews -> AlbumReviews.reviewsStatisticsSubScene statisticsApp
    | None -> Scene.Phone
