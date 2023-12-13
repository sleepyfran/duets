module Duets.Cli.Scenes.Phone.Apps.Statistics.Relationships

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation

let rec relationshipsStatisticsSubScene statisticsApp =
    let state = State.get ()
    let relationships = Queries.Relationship.all state |> List.ofMapValues

    let tableColumns =
        [ Styles.header "Name"
          Styles.header "Relationship type"
          Styles.header "Level" ]

    let tableRows =
        relationships
        |> List.map (fun relationship ->
            let npc = Queries.Characters.find state relationship.Character

            [ Styles.person npc.Name
              Social.relationshipType relationship.RelationshipType
              |> Styles.highlight
              $"{relationship.Level |> Styles.Level.from}%%" ])

    showTable tableColumns tableRows

    statisticsApp ()
