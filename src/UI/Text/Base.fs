module UI.Text.Base

/// Returns a formatted list as empty if it contains nothing, "a" if it contains
/// only one element, "a and b" with two elements and "a, b and c" for all other
/// lengths.
let rec listOf (stuff: 'a list) toStr =
    match stuff with
    | [] -> ""
    | [ head ] -> toStr head
    | [ head; tail ] -> $"{toStr head} and {toStr tail}"
    | head :: tail -> $"{toStr head}, {listOf tail toStr}"

/// Returns the given singular form if the quantity is 1, or the plural form
/// in all other cases.
let pluralOf singular plural quantity =
    match quantity with
    | quantity when quantity = LanguagePrimitives.Int32WithMeasure 1 -> singular
    | _ -> plural

/// Returns the given singular form if the quantity is 1, or the plural form
/// by putting an "s" in the end in all other cases. Only works for words that
/// have a simple plural where slapping an "s" on the end works, for more
/// complex cases use `pluralOf` directly.
let simplePluralOf singular quantity =
    pluralOf singular $"{singular}s" quantity
