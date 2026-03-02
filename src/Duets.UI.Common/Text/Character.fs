module Duets.UI.Common.Text.Character

open Duets.Entities

let gender =
    function
    | Male -> "♂ Male"
    | Female -> "♀ Female"
    | Other -> "⚥ Other"
