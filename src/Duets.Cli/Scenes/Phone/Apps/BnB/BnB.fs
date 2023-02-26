module Duets.Cli.Scenes.Phone.Apps.BnB.Root

open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text

type private BnBMenuOptions =
    | RentPlace
    | ListCurrentBookings

let private textFromOption opt =
    match opt with
    | RentPlace -> "Rent place"
    | ListCurrentBookings -> "List current bookings"

/// Creates the BnB app, which allows the user to rent places and manage their
/// bookings.
let rec bnbApp () =
    let selection =
        showOptionalChoicePrompt
            "What do you want to do?"
            Generic.back
            textFromOption
            [ RentPlace; ListCurrentBookings ]

    match selection with
    | Some RentPlace -> Rent.rent bnbApp
    | Some ListCurrentBookings -> List.listAll bnbApp
    | None -> Scene.Phone
