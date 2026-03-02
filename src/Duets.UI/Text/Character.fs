module Duets.UI.Text.Character

open Duets.Entities

let gender =
    function
    | Male -> "♂ Male"
    | Female -> "♀ Female"
    | Other -> "⚥ Other"
