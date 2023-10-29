namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module MeCommand =
    let private showCharacterInfo character =
        let age = Queries.Characters.ageOf (State.get ()) character

        Command.meName character.Name |> showMessage

        Command.meBirthdayAge character.Birthday age |> showMessage

    let private showCharacterAttributes _ =
        Queries.Characters.allPlayableCharacterAttributes (State.get ())
        |> List.map (fun (attr, amount) ->
            (amount, Character.attributeName attr))
        |> showBarChart

    let private showCharacterMoodlets character =
        let moodlets = Queries.Characters.moodlets character

        if Set.isEmpty moodlets then
            Styles.faded "No moodlets affecting you right now" |> showMessage
        else
            moodlets
            |> Set.iter (fun moodlet ->
                let moodletName =
                    match moodlet.MoodletType with
                    | MoodletType.JetLagged -> "Jet lagged" |> Styles.warning
                    | MoodletType.NotInspired ->
                        "Not inspired" |> Styles.warning
                    | MoodletType.TiredOfTouring ->
                        "Tired of touring" |> Styles.warning

                let moodletExplanation =
                    match moodlet.MoodletType with
                    | MoodletType.JetLagged ->
                        "You've traveled to a city with a very different timezone, you'll need some time to adjust. You might feel more tired than usual"
                    | MoodletType.NotInspired ->
                        "You've been composing too much lately, better take a break! Composing and improving songs while not inspired won't be as effective"
                    | MoodletType.TiredOfTouring ->
                        "You've been having too many concerts in the past few days, you need some rest! You won't be able to perform as well as usual"

                let moodletExpirationText =
                    match moodlet.Expiration with
                    | MoodletExpirationTime.Never -> "Does not expire"
                    | MoodletExpirationTime.AfterDays days ->
                        $"Expires after {days} days"
                    | MoodletExpirationTime.AfterDayMoments dayMoments ->
                        $"Expires after {dayMoments} day moments"
                    |> Styles.faded

                $"""{Emoji.moodlet moodlet.MoodletType} {moodletName} - {moodletExplanation}
   {moodletExpirationText}"""
                |> showMessage)

    let private showCharacterSkills (character: Character) =
        let skills =
            Queries.Skills.characterSkillsWithLevel (State.get ()) character.Id
            |> List.ofMapValues

        if List.isEmpty skills then
            Styles.faded "No skills learned yet" |> showMessage
        else
            skills
            |> List.map (fun (skill, level) ->
                (level, Skill.skillName skill.Id))
            |> showBarChart

    /// Command which displays the information of the playable character.
    let get =
        { Name = "@me"
          Description = Command.meDescription
          Handler =
            fun _ ->
                let playableCharacter =
                    Queries.Characters.playableCharacter (State.get ())

                Some "Character info" |> showSeparator

                showCharacterInfo playableCharacter

                Some "Attributes" |> showSeparator

                showCharacterAttributes playableCharacter

                Some "Moodlets" |> showSeparator

                showCharacterMoodlets playableCharacter

                Some "Skills" |> showSeparator

                showCharacterSkills playableCharacter

                showSeparator None

                Scene.World }
