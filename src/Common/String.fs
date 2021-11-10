module Common.String

/// Calls Split on the given string with the specified separator.
let split separator (str: string) = str.Split([| separator |])
