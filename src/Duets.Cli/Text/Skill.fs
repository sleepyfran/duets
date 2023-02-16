module Duets.Cli.Text.Skill

open Duets.Common
open Duets.Entities

let skillName id =
    match id with
    | SkillId.Composition -> "Composition"
    | SkillId.Genre genre -> $"{genre} (Genre)"
    | SkillId.Instrument instrument ->
        $"{Generic.instrumentName instrument} (Instrument)"
    | SkillId.MusicProduction -> "Music production"
    | SkillId.Speech -> "Speech"

let categoryName category =
    match category with
    | SkillCategory.Character -> "Character"
    | SkillCategory.Music -> "Music"
    | SkillCategory.Production -> "Production"

let skillDescription id =
    match id with
    | SkillId.Composition ->
        "The composition skill determines how good you are at making new music. It directly impacts how good your songs will be"
    | SkillId.Genre genre ->
        $"The genre skill determines how good you are at making {genre} music. It directly impacts how good your {genre} songs will be"
    | SkillId.Instrument instrument ->
        $"The instrument skills determine how good you are at playing {Generic.instrumentName instrument |> String.lowercase}. It directly impacts how good your songs will be when your main instrument is {Generic.instrumentName instrument |> String.lowercase}"
    | SkillId.MusicProduction ->
        "The music production skill determines how good you are at recording, producing and mastering songs. This skill is not that important if you don't produce your own songs and instead you use a studio with a producer"
    | SkillId.Speech ->
        "The speech skill determines how good you are at talking, which can be useful when giving speeches in concerts and during interviews"
    |> Styles.faded

let skillImproved
    characterName
    characterGender
    (skill: Skill)
    previousLevel
    currentLevel
    =
    Styles.success
        $"""{characterName} improved {(Generic.possessiveAdjectiveForGender characterGender) |> String.lowercase} {skillName skill.Id |> String.lowercase} skill from {previousLevel} to {currentLevel}"""
