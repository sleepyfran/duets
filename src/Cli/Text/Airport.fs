module Cli.Text.Airport

open Entities

let passingSecurityCheck =
    Styles.progress "Passing security check..."

let itemsTakenBySecurity =
    Styles.danger
        $"Some of your items were taken away by security because they were not allowed in the plane"

let waitingToBoard =
    Styles.progress "Waiting to board plane..."

let planeBoarded flight time =
    Styles.success
        $"You boarded the plane to {Generic.cityName flight.Destination}. You should be there in around {Generic.duration time}"
