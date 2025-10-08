module Duets.Cli.Text.Travel

open Duets.Entities

let planeActionPrompt date dayMoment attributes flight =
    $"""{Generic.infoBar date dayMoment attributes}
{Emoji.flying} Flying to {Generic.cityName flight.Destination |> Styles.place}. What do you want to do?"""

let passingSecurityCheck = Styles.progress "Passing security check..."

let itemsTakenBySecurity =
    Styles.danger
        $"Some of your items were taken away by security because they were not allowed in the plane"

let waitingToBoard = Styles.progress "Waiting to board plane..."

let planeBoarded flight time =
    Styles.success
        $"You boarded the plane to {Generic.cityName flight.Destination}. You should be there in around {Generic.duration time}"

let waitForLanding = Styles.progress "Waiting for the plane to land..."

let gettingOffPlane = Styles.progress "Getting off plane..."

let passingPassportControl = Styles.progress "Passing passport control..."

let blindlyStaringAtPhone = Styles.progress "Blindly staring at your phone..."

let gettingAnnoyed = Styles.progress "Getting annoyed..."

let waitingSomeMore = Styles.progress "Waiting some more..."

let actionPrompt date dayMoment attributes vehicle =
    $"""{Generic.infoBar date dayMoment attributes}
{Emoji.vehicle vehicle} Travelling
What do you want to do?"""
    |> Styles.prompt

let arrivedAtStation dayMoment =
    match dayMoment with
    | EarlyMorning
    | Morning ->
        "The train slows abruptly, a slight jostle as it comes to a stop. The doors slide open, revealing a platform surging with early morning commuters. A cacophony of sounds – hurried footsteps, snippets of phone calls, and the automated announcements – washes over you as you step into the throng. You've arrived, right in the thick of the morning rush."
    | Midday
    | Afternoon
    | Evening ->
        "The train decelerates smoothly, arriving at your destination. The doors chime open, offering a view of a platform with a more relaxed pace. A mix of voices and the gentle hum of the station fills the air as you step onto the platform. You've reached your stop, the afternoon atmosphere much more leisurely."
    | Night
    | Midnight ->
        "The train glides to a quiet stop, the air brakes sighing softly. The doors slide open to reveal a dimly lit platform, where only a handful of other travelers are waiting. The silence, broken only by occasional announcements and the low hum of the station, is striking. You disembark, arriving at your stop in the quiet of the night."
    |> Styles.event
