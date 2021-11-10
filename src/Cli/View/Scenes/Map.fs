module Cli.View.Scenes.Map

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants

let mapOptions =
    [ yield
        { Id = "rehearsal_room"
          Text = TextConstant MapOptionRehearsalRoom }

      yield
          { Id = "studios"
            Text = TextConstant MapOptionStudios } ]

let rec mapScene () =
    seq {
        yield Figlet <| TextConstant MapTitle

        yield
            Prompt
                { Title = TextConstant MapPrompt
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = mapOptions
                            Handler =
                                worldOptionalChoiceHandler <| processSelection
                            BackText = TextConstant CommonBackToWorld } }
    }

and processSelection choice =
    seq {
        match choice.Id with
#if DEBUG
        | "dev_room" -> yield Scene DeveloperRoom
#endif
        | "rehearsal_room" -> yield Scene RehearsalRoom
        | "bank" -> yield Scene Bank
        | "studios" -> yield! studioSelection ()
        | _ -> yield NoOp
    }

and studioSelection () =
    let studios = Database.studios ()

    let studioChoices =
        studios
        |> List.map
            (fun studio ->
                { Id = studio.Id.ToString()
                  Text = Literal studio.Name })

    seq {
        yield
            Prompt
                { Title = TextConstant MapPrompt
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = studioChoices
                            Handler =
                                mapOptionalChoiceHandler
                                <| processStudio studios
                            BackText = TextConstant CommonBackToMap } }
    }

and processStudio studios choice =
    seq {
        yield
            studios
            |> List.find (fun studio -> studio.Id.ToString() = choice.Id)
            |> Studio
            |> Scene
    }
