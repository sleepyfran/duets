module Cli.Scenes.InteractiveSpaces.RehearsalRoom.ComposeSong

open Agents
open Cli.Actions
open Cli.Common
open Cli.Text
open Entities
open FSharp.Data.UnitSystems.SI.UnitNames
open Simulation.Songs.Composition.ComposeSong

let rec composeSongSubScene () =
    seq {
        yield
            Prompt
                { Title =
                      I18n.translate (RehearsalSpaceText ComposeSongTitlePrompt)
                  Content = TextPrompt(lengthPrompt) }
    }

and lengthPrompt name =
    seq {
        yield
            Prompt
                { Title =
                      I18n.translate (
                          RehearsalSpaceText ComposeSongLengthPrompt
                      )
                  Content = LengthPrompt(genrePrompt name) }
    }

and genrePrompt name length =
    seq {
        yield
            Prompt
                { Title =
                      I18n.translate (RehearsalSpaceText ComposeSongGenrePrompt)
                  Content =
                      ChoicePrompt
                      <| MandatoryChoiceHandler
                          { Choices = genreOptions
                            Handler = vocalStylePrompt name length } }
    }

and vocalStylePrompt name length selectedGenre =
    let vocalStyleOptions =
        Database.vocalStyleNames ()
        |> List.map
            (fun vocalStyle ->
                { Id = vocalStyle.ToString()
                  Text = Literal(vocalStyle.ToString()) })

    seq {
        yield
            Prompt
                { Title =
                      I18n.translate (
                          RehearsalSpaceText ComposeSongVocalStylePrompt
                      )
                  Content =
                      ChoicePrompt
                      <| MandatoryChoiceHandler
                          { Choices = vocalStyleOptions
                            Handler = handleSong name length selectedGenre.Id } }
    }

and handleSong name length genre selectedVocalStyle =
    let vocalStyle =
        Song.VocalStyle.from selectedVocalStyle.Id

    seq {
        match Song.from name length vocalStyle genre with
        | Ok song -> yield! composeWithProgressbar song
        | Error Song.NameTooShort ->
            yield!
                handleError
                <| I18n.translate (
                    RehearsalSpaceText ComposeSongErrorNameTooShort
                )
        | Error Song.NameTooLong ->
            yield!
                handleError
                <| I18n.translate (
                    RehearsalSpaceText ComposeSongErrorNameTooLong
                )
        | Error Song.LengthTooShort ->
            yield!
                handleError
                <| I18n.translate (
                    RehearsalSpaceText ComposeSongErrorLengthTooShort
                )
        | Error Song.LengthTooLong ->
            yield!
                handleError
                <| I18n.translate (
                    RehearsalSpaceText ComposeSongErrorLengthTooLong
                )
    }

and composeWithProgressbar song =
    let state = State.get ()

    seq {

        yield
            ProgressBar
                { StepNames =
                      [ I18n.translate (
                          RehearsalSpaceText ComposeSongProgressBrainstorming
                        )
                        I18n.translate (
                            RehearsalSpaceText
                                ComposeSongProgressConfiguringReverb
                        )
                        I18n.translate (
                            RehearsalSpaceText ComposeSongProgressTryingChords
                        ) ]
                  StepDuration = 2<second>
                  Async = true }

        yield
            ComposeSongConfirmation song.Name
            |> RehearsalSpaceText
            |> I18n.translate
            |> Message

        yield Effect <| composeSong state song

        yield Scene Scene.World
    }

and handleError message =
    seq {
        yield Message <| message
        yield! composeSongSubScene ()
    }
