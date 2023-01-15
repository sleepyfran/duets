namespace Cli.Components.Commands

open Agents
open Cli
open Cli.Components
open Cli.Components.Commands
open Cli.SceneIndex
open Cli.Text
open Entities
open Simulation
open Simulation.Concerts

[<RequireQualifiedAccess>]
module StartConcertCommand =
    /// Returns a command that starts a scheduled concert.
    let create scheduledConcert =
        { Name = "start concert"
          Description = Command.startConcertDescription
          Handler =
            fun _ ->
                let concert = Concert.fromScheduled scheduledConcert

                let attendancePercentage =
                    Queries.Concerts.attendancePercentage concert

                Scheduler.startScheduledConcerts (State.get ()) concert.VenueId
                |> Effect.applyMultiple

                match attendancePercentage with
                | att when att <= 10 ->
                    Styles.Level.bad
                        "As you step out onto the stage, you can't help but feel a twinge of disappointment as you take in the sparse crowd. The rows of empty seats stretch out before you, with only a handful of people scattered throughout. The stage lights are bright, casting a warm glow over the venue, but it only serves to highlight the emptiness."
                | att when att <= 35 ->
                    Styles.Level.bad
                        "As you step out onto the stage, you can't help but feel a twinge of disappointment as you take in the crowd. The rows of seats are not completely empty but not as packed as you were hoping. The stage lights are bright, casting a warm glow over the venue, but it only serves to highlight the empty spaces."
                | att when att <= 60 ->
                    Styles.Level.normal
                        "As you step out onto the stage, you can't help but feel a mix of disappointment and appreciation as you take in the crowd. The rows of seats are partially filled with a good number of people scattered throughout the venue. The stage lights are bright, casting a warm glow over the venue, but it only serves to highlight the spaces that are still empty."
                | att when att <= 85 ->
                    Styles.Level.good
                        "As you step out onto the stage, you can't help but feel a rush of excitement as you take in the crowd. The rows of seats are almost completely filled, with only a few empty spaces scattered throughout the venue. The stage lights are bright, casting a warm glow over the venue, and the energy in the room is electric."
                | _ ->
                    Styles.Level.great
                        "As you step out onto the stage, you can't help but feel a rush of excitement as you take in the crowd. The rows of seats are packed, with people standing in the back and in the aisles, eagerly awaiting the start of the show. The stage lights are bright, casting a warm glow over the venue, and the energy in the room is electric."
                |> showMessage

                Scene.World }
