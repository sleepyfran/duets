module Duets.Cli.Text.World.Common

open Duets.Entities

/// Function to use when a city does not have a specific descriptor. It returns
/// an empty string whatever the input is.
let nonExistent (_: DayMoment) = [ "" ]
