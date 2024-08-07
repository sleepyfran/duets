module Duets.Cli.Text.Airport

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
