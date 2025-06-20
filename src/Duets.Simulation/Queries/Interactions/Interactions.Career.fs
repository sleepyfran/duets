namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Common.Tuple
open Duets.Entities
open Duets.Simulation

module Career =
    let internal interactions state (currentPlace: Place) =
        let currentJob = Queries.Career.current state

        match currentJob with
        | Some job when job.Location |> snd3 = currentPlace.Id ->
            [ CareerInteraction.Work job |> Interaction.Career ]
        | _ -> []
