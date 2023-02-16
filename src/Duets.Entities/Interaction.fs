module Duets.Entities.Interaction

/// <summary>
/// Invokes the given <c>chooser</c> when the interaction is of type FreeRoam,
/// otherwise it returns None.
/// </summary>
let chooseFreeRoam chooser interactions =
    interactions
    |> List.choose (fun interaction ->
        match interaction with
        | Interaction.FreeRoam freeRoamInteraction ->
            chooser freeRoamInteraction
        | _ -> None)
