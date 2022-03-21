module Common.String

/// Calls Split on the given string with the specified separator.
let split separator (str: string) = str.Split([| separator |])

/// Calls StartsWith on `str` with `subStr` as the parameter.
let startsWith (subStr: string) (str: string) = str.StartsWith(subStr)

/// Removes any space in the beginning of the string.
let trimStart (str: string) = str.TrimStart()

/// Makes the given string lowercase.
let lowercase (str: string) = str.ToLower()
