namespace Cli.Components.Commands

open Cli.Actions
open Cli.Text

[<RequireQualifiedAccess>]
module PhoneCommand =
    /// Command which opens the phone of the user.
    let get =
        { Name = "phone"
          Description = I18n.translate (CommandText CommandPhoneDescription)
          Handler = fun _ -> Some Phone }
