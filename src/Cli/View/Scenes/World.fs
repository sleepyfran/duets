module Cli.View.Scenes.World

open Cli.View.Actions
open Cli.View.TextConstants

let commands: Command list =
    [
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
        yield
            Prompt
                { Title = TextConstant CommonCommandPrompt
                  Content = CommandPrompt commands }
    }
