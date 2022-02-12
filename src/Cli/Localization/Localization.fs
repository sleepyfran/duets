[<AutoOpen>]
module Cli.Localization.Root

/// Translates the given text into the current language. For now just invokes
/// the English translation, but in the future (if we ever support other
/// languages) this should call the correct resolver for the language).
let toString = English.toString
