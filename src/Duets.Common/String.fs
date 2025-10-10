module Duets.Common.String

open System
open System.Globalization
open System.Text.RegularExpressions

/// Calls Split on the given string with the specified separator.
let split separator (str: string) = str.Split([| separator |])

/// Calls Split on the given string with the specified separator.
let splitS (separator: string) (str: string) = str.Split(separator)

/// Calls StartsWith on `str` with `subStr` as the parameter.
let startsWith (subStr: string) (str: string) = str.StartsWith(subStr)

/// Calls Contains on `str` with `subStr` as the parameter.
let contains (subStr: string) (str: string) = str.Contains(subStr)

/// Calls contains only if subStr is not empty.
let nonEmptyContains (subStr: string) (str: string) =
    (String.IsNullOrEmpty(subStr) |> not) && contains subStr str

/// Checks whether the given `str` contains the `subStr` without taking any
/// diacritics or casing into account.
let diacriticInsensitiveContains (subStr: string) (str: string) =
    let compareInfo = CultureInfo.InvariantCulture.CompareInfo

    compareInfo.IndexOf(
        subStr,
        str,
        CompareOptions.IgnoreNonSpace ||| CompareOptions.IgnoreCase
    ) > -1

/// Calls `IsNullOrWhiteSpace` on the given string.
let isEmpty (str: string) = System.String.IsNullOrWhiteSpace(str)

/// Removes any space in the beginning of the string.
let trimStart (str: string) = str.TrimStart()

/// Removes any space from the beginning and end of the string.
let trim (str: string) = str.Trim()

/// Makes the given string lowercase.
let lowercase (str: string) = str.ToLower()

/// Transforms the given string into "Title Case".
let titleCase (str: string) =
    CultureInfo.InvariantCulture.TextInfo.ToTitleCase str

/// Replaces all occurrences of `pattern` in `str` with `replacement`.
let replace (pattern: string) (replacement: string) (str: string) =
    Regex.Replace(str, pattern, replacement)

/// Calls String join with the given separator and items.
let join (separator: string) (items: string list) =
    String.Join(separator, items)
