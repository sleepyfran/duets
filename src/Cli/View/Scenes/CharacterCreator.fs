module Cli.View.Scenes.CharacterCreator

open Cli.View.Actions
open Cli.View.TextConstants
open Entities

let genderOptions =
  [ { Id = "Male"
      Text = TextConstant CharacterCreatorGenderMale }
    { Id = "Female"
      Text = TextConstant CharacterCreatorGenderFemale }
    { Id = "Other"
      Text = TextConstant CharacterCreatorGenderOther } ]

let rec characterCreator () =
  seq {
    yield
      Prompt
        { Title = TextConstant CharacterCreatorInitialPrompt
          Content = TextPrompt genderPrompt }
  }

and genderPrompt name =
  seq {
    yield
      Prompt
        { Title = TextConstant CharacterCreatorGenderPrompt
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
        { Title = TextConstant CharacterCreatorAgePrompt
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
            Message
            <| TextConstant CreatorErrorCharacterNameTooShort

            Scene CharacterCreator
          }
    | Error Character.NameTooLong ->
        yield!
          seq {
            Message
            <| TextConstant CreatorErrorCharacterNameTooLong

            Scene CharacterCreator
          }
    | Error Character.AgeTooYoung ->
        yield!
          seq {
            Message
            <| TextConstant CreatorErrorCharacterAgeTooYoung

            yield! genderPrompt name
          }
    | Error Character.AgeTooOld ->
        yield!
          seq {
            Message
            <| TextConstant CreatorErrorCharacterAgeTooOld

            yield! genderPrompt name
          }
  }
