module Duets.Cli.Scenes.Phone.Apps.Statistics.Root

open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text

type private StatisticsOption =
    | Band
    | Albums

let private textFromOption opt =
    match opt with
    | Band -> Phone.statisticsAppSectionBand
    | Albums -> Phone.statisticsAppSectionAlbums

let rec statisticsApp () =
    let selectedChoice =
        showOptionalChoicePrompt
            Phone.statisticsAppSectionPrompt
            Generic.backToPhone
            textFromOption
            [ Band; Albums ]

    match selectedChoice with
    | Some Band -> Band.bandStatisticsSubScene statisticsApp
    | Some Albums -> Albums.albumsStatisticsSubScene statisticsApp
    | None -> Scene.Phone
