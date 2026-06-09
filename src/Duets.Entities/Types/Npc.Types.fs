namespace Duets.Entities

[<AutoOpen>]
module NpcTypes =
    type NpcWork =
        | Croupier
        | Nurse
        | PlayableWork of CareerId
        | Recepcionist
        | Security
        | Seller
        | Waiter

    /// Determines which role the character takes in the current room it was spawned on.
    type NpcRoomGoal =
        | Audience
        | BandMember
        | Customer
        | Commuter
        | Fan
        | Musician
        | Tourist
        | Working of NpcWork

    /// Wraps an NPC with extra information about which role they have on the
    /// current room.
    type PresentNpc = { Npc: Character; Goal: NpcRoomGoal }
