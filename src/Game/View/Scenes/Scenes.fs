module View.Scenes.Index

open Entities.Character

/// Defines the index of all scenes available in the game that can be instantiated.
type Scene =
  | MainMenu
  | CharacterCreator
  // Band creator needs a character the character that was created in the
  // previous step.
  | BandCreator of Character
