module Duets.Cli.Text.Career

open Duets.Common
open Duets.Entities

module Barista =
    let careerStageName (CareerStageId stage) =
        match stage with
        | 0uy -> "Dishwasher"
        | 1uy -> "Junior Barista"
        | 2uy -> "Barista"
        | 3uy -> "Senior Barista"
        | _ -> "Manager"

module Bartender =
    let careerStageName (CareerStageId stage) =
        match stage with
        | 0uy -> "Dishwasher"
        | 1uy -> "Table cleaner"
        | 2uy -> "Bartender"
        | 3uy -> "Mixologist"
        | _ -> "Manager"

let name (job: Job) =
    match job.Id with
    | Barista -> Barista.careerStageName job.CurrentStage.Id
    | Bartender -> Bartender.careerStageName job.CurrentStage.Id

let typeName id =
    match id with
    | Barista -> "Barista"
    | Bartender -> "Bartender"

let shiftDurationDescription schedule =
    match schedule with
    | JobSchedule.Free shiftDuration ->
        $"""{shiftDuration} {Generic.simplePluralOf "day moment" shiftDuration} per shift"""

let scheduleDescription schedule =
    match schedule with
    | JobSchedule.Free _ ->
        $"""No schedule, {shiftDurationDescription schedule}"""

let careerChange (job: Job) placeName =
    Styles.success $"You now work as {name job} at {Styles.place placeName}"

let careerLeft (job: Job) placeName =
    Styles.danger $"You left your job as {name job} at {Styles.place placeName}"

let careerPromoted (job: Job) placeName salary =
    Styles.success
        $"You got promoted to {name job} at {Styles.place placeName}. You will now earn {Styles.money salary} per day moment"

let workShiftEvent (job: Job) =
    match job.Id with
    | Barista ->
        [ "You start your barista shift, there's not much people and the ones that are in the cafe are just sitting with their coffee already"
          "There's a lot of people coming in and ordering stuff, get ready for some lattes!" ]
    | Bartender ->
        [ "You start your bartender shift, the day is quite calm"
          "Among all the noise in the bar, you start your crazy day as bartender..." ]
    |> List.sample
    |> Styles.progress

let workShiftFinished salary =
    Styles.success $"You finished your shift and earned {Styles.money salary}"
