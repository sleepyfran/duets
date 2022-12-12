module Cli.Text.Career

open Entities

let name t =
    match t with
    | Bartender -> "Bartender"
    
let scheduleDescription schedule =
    match schedule with
    | JobSchedule.Free -> "No schedule"

let careerChange (job: Job) placeName =
    Styles.success $"You now work as {name job.Id} at {Styles.place placeName}"

let careerLeft (job: Job) placeName =
    Styles.danger $"You left your job as {name job.Id} at {Styles.place placeName}"
