module Duets.Entities.Time

open FSharp.Data.UnitSystems.SI.UnitNames
open Duets.Common

let private secondsInMinute = 60<second / minute>

module Length =
    /// Creates an empty length that indicates 0.
    let empty =
        { Minutes = 0<minute>
          Seconds = 0<second> }

    type ValidationError =
        | LessThan0Minutes
        | LessThan0Seconds
        | MoreThan60Seconds
        | InvalidFormat

    let private validateMinutes minutes =
        if minutes >= 0<minute> then
            Ok minutes
        else
            Error LessThan0Minutes

    let private validateSeconds seconds =
        if seconds < 0<second> then Error LessThan0Seconds
        else if seconds > 60<second> then Error MoreThan60Seconds
        else Ok seconds

    /// Creates a length given the minutes and seconds, validating that a correct
    /// length was passed.
    let from minutes seconds =
        validateMinutes minutes
        |> Result.andThen (validateSeconds seconds)
        |> Result.transform { Minutes = minutes; Seconds = seconds }

    /// Creates a length from the given amount of seconds.
    let fromSeconds (s: int<second>) =
        let ts = System.TimeSpan.FromSeconds(float s)
        from (ts.Minutes * 1<minute>) (ts.Seconds * 1<second>)

    /// Creates a length given a string representing it with the format mm:ss,
    /// such as 6:55, 0:35 or 3:00.
    let parse (input: string) =
        try
            input.Split ':'
            |> fun inputs ->
                match inputs with
                | [| minutes; seconds |] ->
                    from (int minutes * 1<minute>) (int seconds * 1<second>)
                | _ -> Error InvalidFormat
        with _ ->
            Error InvalidFormat

    /// Returns the total amount of seconds in the given length.
    let inSeconds length =
        length.Minutes * secondsInMinute + length.Seconds
