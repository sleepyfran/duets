module Cli.Scenes.Phone.Apps.Statistics.Root

open Cli.Components
open Cli.SceneIndex
open Cli.Text

type private StatisticsOption =
    | Band
    | Albums

let private textFromOption opt =
    match opt with
    | Band -> PhoneText StatisticsAppSectionBand
    | Albums -> PhoneText StatisticsAppSectionAlbums
    |> I18n.translate

let rec statisticsApp () =
    let selectedChoice =
        showOptionalChoicePrompt
            (PhoneText StatisticsAppSectionPrompt
             |> I18n.translate)
            (CommonText CommonBackToPhone |> I18n.translate)
            textFromOption
            [ Band; Albums ]

    match selectedChoice with
    | Some Band -> Band.bandStatisticsSubScene statisticsApp
    | Some Albums -> Albums.albumsStatisticsSubScene statisticsApp
    | None -> Scene.Phone
