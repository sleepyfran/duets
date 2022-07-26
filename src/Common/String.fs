module Common.String

open System.Globalization

/// Calls Split on the given string with the specified separator.
let split separator (str: string) = str.Split([| separator |])

/// Calls StartsWith on `str` with `subStr` as the parameter.
let startsWith (subStr: string) (str: string) = str.StartsWith(subStr)

/// Calls Contains on `str` with `subStr` as the parameter.
let contains (subStr: string) (str: string) = str.Contains(subStr)

/// Checks whether the given `str` contains the `subStr` without taking any
/// diacritics or casing into account.
let diacriticInsensitiveContains (subStr: string) (str: string) =
    let compareInfo =
        CultureInfo.InvariantCulture.CompareInfo

    compareInfo.IndexOf(
        subStr.ToUpper(),
        str.ToUpper(),
        CompareOptions.IgnoreNonSpace
    ) > -1

/// Calls `IsNullOrWhiteSpace` on the given string.
let isEmpty (str: string) = System.String.IsNullOrWhiteSpace(str)

/// Removes any space in the beginning of the string.
let trimStart (str: string) = str.TrimStart()

/// Makes the given string lowercase.
let lowercase (str: string) = str.ToLower()
