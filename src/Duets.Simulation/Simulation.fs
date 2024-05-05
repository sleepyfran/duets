[<RequireQualifiedAccess>]
module Duets.Simulation.Simulation

open Duets.Entities
open Duets.Simulation.Events
open Duets.Simulation.Flights
open Duets.Simulation.Songs
open Duets.Simulation.Songs.Composition
open Duets.Simulation.Studio

type private TickState =
    { AppliedEffects: Effect list
      State: State }

let rec private tick' tickState (nextEffectFns: EffectFn list) : TickState =
    match nextEffectFns with
    | effectFn :: rest -> effectFn tickState.State |> tickEffect tickState rest
    | [] -> tickState

and private tickEffect tickState nextEffectFns effects =
    match effects with
    | [] -> tick' tickState nextEffectFns
    | effect :: restOfEffects ->
        (*
        Before applying the effect and gathering its associated effects, check if
        there's any current modifier that needs to be applied to the effect. For
        example, if the character is not inspired, song related effects have
        less effect.
        *)
        let effect =
            EffectModifiers.EffectModifiers.modify tickState.State effect

        let updatedState = State.Root.applyEffect tickState.State effect
        let associatedEffectFns = Events.associatedEffects effect

        (* Tick all the associated effects first, and pass the rest of the
           effects that come after the current one that was applied plus all
           the other effect functions that are left to be applied.
           tickAssociatedEffects will then decide what to apply and what to
           discard. *)
        tickAssociatedEffects
            { AppliedEffects = tickState.AppliedEffects @ [ effect ]
              State = updatedState }
            associatedEffectFns
            (* Prepend the rest of the effects so that they'll be processed
               before the next effects on the chain. *)
            ((fun _ -> restOfEffects) :: nextEffectFns)

and private tickAssociatedEffects tickState associatedEffects nextEffectFns =
    match associatedEffects with
    | BreakChain effectFns :: _ ->
        (* Breaking the chain means discarding the tail of associated effects
           and also the rest of the effect fns that were left to be applied. *)
        tick' tickState effectFns
    | ContinueChain effectFns :: restOfAssociatedEffects ->
        (* When continuing a chain, we pre-pend all the effect functions that
           were generated in this associated effect to the actual tail of effect
           functions that are left to be applied. *)
        effectFns @ nextEffectFns
        |> tickAssociatedEffects tickState restOfAssociatedEffects
    | [] -> tick' tickState nextEffectFns

//// Ticks the simulation by applying multiple effects, gathering its associated
/// effects and applying them as well.
/// Returns a tuple with the list of all the effects that were applied in the
/// order in which they were applied and the updated state.
let rec tickMultiple currentState effects =
    let effectFns = fun _ -> effects

    let tickResult =
        tick'
            { AppliedEffects = []
              State = currentState }
            (effectFns :: Events.endOfChainEffects)

    tickResult.AppliedEffects, tickResult.State

/// Same as `tickMultiple` but with one effect.
let tickOne currentState effect = tickMultiple currentState [ effect ]

/// Attempts to run the given action against the state and returns whether
/// the action is possible or not. If possible, determines the effects associated
/// with the action, ticks them through the simulation and returns the updated
/// state along with the effects that were applied.
let runAction currentState action : ActionResult =
    match action with
    | AirportBoardPlane flight -> Airport.boardPlane flight |> Ok
    | AirportPassSecurity -> Airport.passSecurityCheck currentState |> Ok
    | AirportWaitForLanding flight ->
        Airport.leavePlane currentState flight |> Ok
    | RehearsalRoomComposeSong opts ->
        ComposeSong.composeSong currentState opts.Band opts.Song |> Ok
    | RehearsalRoomDiscardSong opts ->
        DiscardSong.discardSong opts.Band opts.Song |> Ok
    | RehearsalRoomFireMember opts ->
        Bands.Members.fireMember currentState opts.Band opts.CurrentMember
    | RehearsalRoomFinishSong opts ->
        FinishSong.finishSong currentState opts.Band opts.Song |> Ok
    | RehearsalRoomHireMember opts ->
        Bands.Members.hireMember currentState opts.Band opts.MemberToHire |> Ok
    | RehearsalRoomImproveSong opts ->
        ImproveSong.improveSong currentState opts.Band opts.Song
    | RehearsalRoomPracticeSong opts ->
        Practice.practiceSong currentState opts.Band opts.Song
    | RehearsalRoomSwitchToGenre opts ->
        Bands.SwitchGenre.switchGenre currentState opts.Band opts.Genre
    | StudioStartAlbum opts ->
        RecordAlbum.startAlbum
            currentState
            opts.Studio
            opts.SelectedProducer
            opts.Band
            opts.AlbumName
            opts.FirstSong
    | StudioRecordSongForAlbum opts ->
        RecordAlbum.recordSongForAlbum
            currentState
            opts.Studio
            opts.Band
            opts.Album
            opts.Song
    | StudioReleaseAlbum opts ->
        ReleaseAlbum.releaseAlbum currentState opts.Band opts.Album |> Ok
    | StudioRenameAlbum opts ->
        RenameAlbum.renameAlbum opts.Band opts.Album opts.Name |> Ok
    |> Result.map (tickMultiple currentState)
