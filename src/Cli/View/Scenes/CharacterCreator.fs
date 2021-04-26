module Cli.View.Scenes.CharacterCreator

open Cli.View.Actions
open Cli.View.TextConstants
open Entities

let genderOptions =
  [ { Id = "male"
      Text = TextConstant CharacterCreatorGenderMale }
    { Id = "female"
      Text = TextConstant CharacterCreatorGenderFemale }
    { Id = "other"
      Text = TextConstant CharacterCreatorGenderOther } ]

let rec characterCreator () =
  seq {
    yield
      Prompt
        { Title = TextConstant CharacterCreatorInitialPrompt
          Content = TextPrompt handleName }
  }

and handleName name =
  seq {
    yield
      Prompt
        { Title = TextConstant CharacterCreatorGenderPrompt
          Content = ChoicePrompt(genderOptions, handleGender name) }
  }

and handleGender name choice =
  seq {
    yield
      Prompt
        { Title = TextConstant CharacterCreatorAgePrompt
          Content = NumberPrompt <| handleAge name choice.Id }
  }

and handleAge name genderChoiceId age =
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

            yield! handleName name
          }
    | Error Character.AgeTooOld ->
        yield!
          seq {
            Message
            <| TextConstant CreatorErrorCharacterAgeTooOld

            yield! handleName name
          }
  }
