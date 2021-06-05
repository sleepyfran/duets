module Common.List

/// Returns a list created from the values of a map ignoring its keys.
let ofMapValues (map: Map<_, _>) =
    List.ofSeq map |> List.map (fun kvp -> kvp.Value)

/// Applies the averageBy operation from the List module but returning the
/// given defaultValue if the list is empty.
let averageByOrDefault projection defaultValue list =
    if List.length list = 0 then
        defaultValue |> float
    else
        list |> List.averageBy projection

/// Returns a random element from the list. If the passed list is empty, throws
/// since there are no elements to pick.
let sample list =
    List.item (Random.between 0 (List.length list - 1)) list
