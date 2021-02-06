module View.Scenes.CharacterCreator

open Entities.Character
open View.Actions
open View.Scenes.Index
open View.TextConstants

let genderOptions =
  [ { Id = "male"
      Text = CharacterCreatorGenderMale }
    { Id = "female"
      Text = CharacterCreatorGenderFemale }
    { Id = "other"
      Text = CharacterCreatorGenderOther } ]

let rec characterCreator () =
  seq {
    yield
      Prompt
        { Title = CharacterCreatorInitialPrompt
          Content = TextPrompt handleName }
  }

and handleName name =
  let character = { getDefault () with Name = name }

  seq {
    yield
      Prompt
        { Title = CharacterCreatorGenderPrompt
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
        { Title = CharacterCreatorAgePrompt
          Content = NumberPrompt <| handleAge character }
  }

and handleAge character age =
  let character = { character with Age = age }

  seq { yield Scene <| BandCreator character }
