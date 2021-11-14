module Cli.View.Scenes.World

open Cli.View.Actions
open Cli.View.TextConstants

let commands: Command list =
    [ yield
        { Name = "phone"
          Description = TextConstant CommandPhoneDescription
          Handler = HandlerWithNavigation(fun _ -> seq { Scene Phone }) }

#if DEBUG
      yield
          { Name = "dev"
            Description = TextConstant CommandDevDescription
            Handler =
                HandlerWithNavigation(fun _ -> seq { Scene DeveloperRoom }) }
#endif
     ]

let worldScene () =
    seq {
        yield Figlet <| TextConstant WorldTitle

        yield
            Prompt
                { Title = TextConstant CommonCommandPrompt
                  Content = CommandPrompt commands }
    }
