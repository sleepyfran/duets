module Duets.Cli.Text.Travel

open Duets.Entities
open Duets.Entities.SituationTypes

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

let vehicleEmoji situation =
    match situation with
    | TravellingByCar _ -> Emoji.car
    | TravellingByMetro -> Emoji.metro

let vehicleName situation =
    match situation with
    | TravellingByCar(_, car) -> $"on {Generic.itemName car |> Styles.object}"
    | TravellingByMetro -> "by metro"

let actionPrompt date dayMoment attributes situation (currentPlace: Place) =
    $"""{Generic.infoBar date dayMoment attributes}
{vehicleEmoji situation} Travelling {vehicleName situation} from {currentPlace.Name |> Styles.place}
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

let driveCancelled =
    "You decide not to drive anywhere right now." |> Styles.error

let driveCalculatingRoute = "Calculating route..." |> Styles.progress

let driverTooDrunk =
    "You're way too drunk to drive. Sober up first!" |> Styles.error

let driveAlreadyAtDestination =
    "You're already at this location. No need to drive!" |> Styles.success

let driveCannotReachDestination =
    "You cannot reach this destination by car from here." |> Styles.error

let driveRouteEstimate travelTime destinationName =
    $"Driving to {destinationName |> Styles.place} takes approximately {Styles.time travelTime} minutes."
    |> Styles.hint

let driveIntercityEstimate destinationCity distance (time: int<hour>) =
    $"Driving to {destinationCity |> Styles.place} is {distance} km away and will take approximately {Styles.time time} hours."
    |> Styles.hint

let driveIntercityWarning =
    "This process cannot be cancelled once started." |> Styles.warning

let driveConfirmRoute = "Do you want to drive there?" |> Styles.prompt

let driveStarting destinationName =
    $"You get in your car and start driving to {destinationName |> Styles.place}..."
    |> Styles.information

let driveArrivedAtDestination destinationName =
    $"You've arrived at {destinationName |> Styles.place}!" |> Styles.success

let drivingMomentPrefix = "As you drive, you notice: "
