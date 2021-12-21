module Common.String

/// Calls Split on the given string with the specified separator.
let split separator (str: string) = str.Split([| separator |])

/// Makes the given string lowercase.
let lowercase (str: string) = str.ToLower()
