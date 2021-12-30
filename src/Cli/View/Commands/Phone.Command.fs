namespace Cli.View.Commands

open Cli.View.Actions
open Cli.View.Text

[<RequireQualifiedAccess>]
module PhoneCommand =
    /// Command which opens the phone of the user.
    let get =
        { Name = "phone"
          Description = I18n.translate (CommandText CommandPhoneDescription)
          Handler = HandlerWithNavigation(fun _ -> seq { Scene Phone }) }
