module Duets.Cli.Scenes.Phone.Apps.FindMyStuff.Root

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation.Queries

let findMyStuffApp () =
    let state = State.get ()
    let cars = FindMyStuff.findCarsInWorld state

    Phone.findMyStuffAppTitle |> Styles.header |> showMessage

    if cars.IsEmpty then
        Phone.findMyStuffNoCars |> showMessage
    else
        let rows =
            cars
            |> List.map (fun located ->
                let carName = $"{located.Item.Brand} {located.Item.Name}"
                let cityName = Generic.cityName located.CityName
                let location = $"{cityName}, {located.StreetName}"
                [ carName; location ])


        showTable
            [ Phone.findMyStuffCarHeader; Phone.findMyStuffLocationHeader ]
            rows

    showContinuationPrompt ()

    Scene.Phone
