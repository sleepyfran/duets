module UI.Text.Music

open Entities

let roleName =
    function
    | Bass -> "Bass"
    | Drums -> "Drums"
    | Guitar -> "Guitar"
    | Vocals -> "Microphone"

let vocalStyleName =
    function
    | VocalStyle.Normal -> "Normal"
    | VocalStyle.Growl -> "Growl"
    | VocalStyle.Screamo -> "Screamo"
    | VocalStyle.Instrumental -> "Instrumental"
