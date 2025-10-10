module Duets.Data.World.Ids

module Common =
    let bar = "bar"
    let cafe = "cafe"
    let lobby = "lobby"
    let restaurant = "restaurant"

module Airport =
    let securityControl = "security_control"
    let boardingGate = "boarding_gate"

module Bookstore =
    let readingRoom = "reading_room"

module CarDealer =
    let showRoom = "show_room"

module Casino =
    let casinoFloor = "casino_floor"

module ConcertSpace =
    let backstage = "backstage"
    let stage = "stage"

module Gym =
    let changingRoom = "changing_room"
    let gym = "gym"

module Home =
    let kitchen = "kitchen"
    let livingRoom = "living_room"
    let bedroom = "bedroom"

module Metro =
    let platform = "platform"

module Studio =
    let masteringRoom = "mastering_room"
    let recordingRoom = "recording_room"

module RehearsalRoom =
    let room (n: int<_>) = $"rehearsal_room_{n}"

module Workshop =
    let workshop = "workshop"

module Restaurant =
    let kitchen = "kitchen"
