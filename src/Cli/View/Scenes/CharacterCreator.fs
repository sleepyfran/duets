module Cli.View.Scenes.CharacterCreator

open Cli.View.Actions
open Cli.View.Text
open Entities

let genderOptions =
    [ { Id = "Male"
        Text = I18n.translate (CreatorText CharacterCreatorGenderMale) }
      { Id = "Female"
        Text = I18n.translate (CreatorText CharacterCreatorGenderFemale) }
      { Id = "Other"
        Text = I18n.translate (CreatorText CharacterCreatorGenderOther) } ]

let rec characterCreator () =
    seq {
        yield
            Prompt
                { Title =
                      I18n.translate (CreatorText CharacterCreatorInitialPrompt)
                  Content = TextPrompt genderPrompt }
    }

and genderPrompt name =
    seq {
        yield
            Prompt
                { Title =
                      I18n.translate (CreatorText CharacterCreatorGenderPrompt)
                  Content =
                      ChoicePrompt
                      <| MandatoryChoiceHandler
                          { Choices = genderOptions
                            Handler = agePrompt name } }
    }

and agePrompt name choice =
    seq {
        yield
            Prompt
                { Title = I18n.translate (CreatorText CharacterCreatorAgePrompt)
                  Content = NumberPrompt <| handleCharacter name choice.Id }
    }

and handleCharacter name genderChoiceId age =
    let gender = Character.Gender.from genderChoiceId

    seq {
        match (Character.from name age gender) with
        | Ok character -> yield Scene(BandCreator character)
        | Error Character.NameTooShort ->
            yield!
                seq {
                    I18n.translate (
                        CreatorText CreatorErrorCharacterNameTooShort
                    )
                    |> Message

                    Scene CharacterCreator
                }
        | Error Character.NameTooLong ->
            yield!
                seq {
                    I18n.translate (
                        CreatorText CreatorErrorCharacterNameTooLong
                    )
                    |> Message

                    Scene CharacterCreator
                }
        | Error Character.AgeTooYoung ->
            yield!
                seq {
                    I18n.translate (
                        CreatorText CreatorErrorCharacterAgeTooYoung
                    )
                    |> Message

                    yield! genderPrompt name
                }
        | Error Character.AgeTooOld ->
            yield!
                seq {
                    I18n.translate (CreatorText CreatorErrorCharacterAgeTooOld)
                    |> Message

                    yield! genderPrompt name
                }
    }
