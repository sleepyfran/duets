module UI.Text.Character

open Entities

let gender =
    function
    | Male -> "♂ Male"
    | Female -> "♀ Female"
    | Other -> "⚥ Other"
