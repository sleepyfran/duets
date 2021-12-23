module Cli.View.Scenes.Studio.Root

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Entities
open Simulation.Queries

let studioOptions hasAvailableRecords =
    seq {
        yield
            { Id = "start_record"
              Text = TextConstant StudioStartRecord }

        if hasAvailableRecords then
            yield
                { Id = "continue_record"
                  Text = TextConstant StudioContinueRecord }

            yield
                { Id = "discard_record"
                  Text = TextConstant StudioDiscardRecord }
    }
    |> List.ofSeq

/// Creates the studio scene which allows user to record albums. This scenes
/// receives a studio as a parameter that will be used to apply prices and
/// calculate qualities.
let rec studioScene (studio: Studio) =
    let state = State.Root.get ()
    let currentBand = Bands.currentBand state

    let unreleasedAlbums =
        Albums.unreleasedByBand state currentBand.Id

    let hasUnreleasedAlbums = not (Map.isEmpty unreleasedAlbums)

    seq {
        yield
            StudioWelcomePrice(studio.Name, studio.PricePerSong)
            |> TextConstant
            |> Message

        yield
            Prompt
                { Title = TextConstant StudioPrompt
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = studioOptions hasUnreleasedAlbums
                            Handler =
                                mapOptionalChoiceHandler
                                <| processSelection studio
                            BackText = TextConstant CommonBackToMap } }
    }

and processSelection studio choice =
    seq {
        match choice.Id with
        | "start_record" -> yield SubScene <| StudioCreateRecord studio
        | "continue_record" ->
            yield SubScene <| SubScene.StudioContinueRecord studio
        | "discard_record" -> yield! []
        | _ -> yield! []
    }
