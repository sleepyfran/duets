module View.Scenes.CharacterCreator

open Entities.Character
open View.Actions
open View.Scenes.Index
open View.TextConstants

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
  let character = { getDefault () with Name = name }

  seq {
    yield
      Prompt
        { Title = TextConstant CharacterCreatorGenderPrompt
          Content = ChoicePrompt(genderOptions, handleGender character) }
  }

and handleGender character choice =
  let character =
    { character with
        Gender =
          match choice.Id with
          | "male" -> Male
          | "female" -> Female
          | _ -> Other }

  seq {
    yield
      Prompt
        { Title = TextConstant CharacterCreatorAgePrompt
          Content = NumberPrompt <| handleAge character }
  }

and handleAge character age =
  let character = { character with Age = age }

  seq { yield Scene <| BandCreator character }
