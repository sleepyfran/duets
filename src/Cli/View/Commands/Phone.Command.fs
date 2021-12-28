namespace Cli.View.Commands

open Cli.View.Actions
open Cli.View.TextConstants

[<RequireQualifiedAccess>]
module PhoneCommand =
    /// Command which opens the phone of the user.
    let get =
        { Name = "phone"
          Description = TextConstant CommandPhoneDescription
          Handler = HandlerWithNavigation(fun _ -> seq { Scene Phone }) }
