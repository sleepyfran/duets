module Cli.View.Scenes.World

open Cli.View.Actions
open Cli.View.TextConstants

let commands: Command list = []

let worldScene () =
    seq {
        yield
            Prompt
                { Title = TextConstant CommonCommandPrompt
                  Content = CommandPrompt commands }
    }
