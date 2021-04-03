module View.Scenes.CharacterCreator

open Mediator.Mutations.Setup
open View.Actions
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

and handleAge name gender age =
  let character = {
    Name = name
    Age = age
    Gender = gender
  }
  
  seq { yield Scene <| BandCreator character }
