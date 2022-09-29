module UI.Text.Date

open Entities

(* TODO: Figure out why localization does not work when using ToString("d", CurrentCulture). *)
let format (date: Date) = $"{date.Day}/{date.Month}/{date.Year}"

let dayMomentName =
    function
    | Dawn -> "Dawn"
    | Morning -> "Morning"
    | Midday -> "Midday"
    | Sunset -> "Sunset"
    | Dusk -> "Dusk"
    | Night -> "Night"
    | Midnight -> "Midnight"
