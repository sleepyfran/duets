namespace Duets.Entities

[<AutoOpen>]
module ActionTypes =
    /// Defines all the actions that are available in the game, with their
    /// associated payload types. All actions should be prefixed by the context
    /// in which they happen, for example: boarding a plane can only happen
    /// in the airport, so the action should be called AirportBoardPlane.
    type Action =
        | AirportBoardPlane of Flight
        | AirportPassSecurity
        | AirportWaitForLanding of Flight