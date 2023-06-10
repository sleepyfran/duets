module Duets.Common.List

/// Creates a new list containing only the given item.
let ofItem item = [ item ]

/// Removes the first occurrence of the given item (if any) in the list.
let rec removeFirstOccurrenceOf item list =
    match list with
    | head :: tail when head = item -> tail
    | head :: tail -> head :: removeFirstOccurrenceOf item tail
    | _ -> []

/// Like fold but passing the list before the initial value.
let fold' folder list state = List.fold folder state list

/// Like `forall2` but returns true instead of throwing an exception when the
/// size of the collections differ.
let forall2' predicate source1 source2 =
    try
        List.forall2 predicate source1 source2
    with _ ->
        false

/// Returns a list created from a map that transforms the KeyValuePair given
/// by `ofSeq` into an actual tuple.
let ofMap (map: Map<_, _>) =
    List.ofSeq map |> List.map (fun kvp -> (kvp.Key, kvp.Value))
    
/// Returns a list created from the keys of a map ignoring its values.
let ofMapKeys (map: Map<_, _>) =
   List.ofSeq map |> List.map (fun kvp -> kvp.Key) 

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

/// Returns a random index from the list.
let sampleIndex list =
    let maxIndex = List.length list - 1
    let maxIndex = if maxIndex < 0 then 0 else maxIndex
    Random.between 0 maxIndex

/// Returns a random element from the list. If the passed list is empty, throws
/// since there are no elements to pick.
let sample list = List.item (sampleIndex list) list

/// Returns a random element from the list. If the passed list is empty, returns
/// None.
let trySample list = List.tryItem (sampleIndex list) list

/// Returns whether the given list has any element or not.
let isNotEmpty list = List.isEmpty list |> not
