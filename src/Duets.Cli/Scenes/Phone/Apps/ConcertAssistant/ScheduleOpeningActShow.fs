module rec Duets.Cli.Scenes.Phone.Apps.ConcertAssistant.OpeningAct

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Concerts
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

let scheduleShow app =
    let selectedCity = showCityPrompt "Where do you want to look for concerts?"

    match selectedCity with
    | Some city ->
        OpeningActOpportunities.generate (State.get ()) city.Id
        |> Seq.groupBy (fun (_, concert) -> concert.Date)
        |> Map.ofSeq
        |> promptForDate app
    | None -> app ()

let private promptForDate app potentialConcerts =
    let selectedDate =
        showOptionalChoicePrompt
            "Which date do you prefer?"
            Generic.cancel
            Generic.dateWithDay
            (potentialConcerts |> List.ofMapKeys)

    match selectedDate with
    | Some date ->
        potentialConcerts
        |> Map.find date
        |> promptForConcert app potentialConcerts date
    | None -> app ()

let private promptForConcert app allPotentialConcerts date concertsOnDate =
    let selectedConcert =
        showOptionalChoicePrompt
            $"Select a concert to apply for on {Generic.dateWithDay date}"
            (Styles.faded "None")
            (fun (band: Band, concert: Concert) ->
                let place =
                    Queries.World.placeInCityById
                        concert.CityId
                        concert.VenueId

                let capacity =
                    match place.PlaceType with
                    | PlaceType.ConcertSpace space -> space.Capacity
                    | _ -> 0

                let fansInCity = Queries.Bands.fansInCity' band concert.CityId

                $"Headliner {Styles.highlight band.Name} ({band.Genre}/{fansInCity |> Styles.number} fans) @ {Styles.place place.Name} ({capacity} capacity)")
            concertsOnDate

    match selectedConcert with
    | Some(headliner, concert) ->
        applyForOpportunity
            app
            allPotentialConcerts
            date
            concertsOnDate
            headliner
            concert
    | None -> promptForDate app allPotentialConcerts

let private applyForOpportunity
    app
    allPotentialConcerts
    date
    concertsOnDate
    headliner
    concert
    =
    showProgressBarSync
        [ $"{headliner.Name} is thinking about it..." ]
        2<second>

    let applicationResult =
        OpeningActOpportunities.applyToConcertOpportunity
            (State.get ())
            headliner
            concert

    match applicationResult with
    | Ok effect ->
        let earningPercentage =
            match concert.ParticipationType with
            | Headliner -> 100<percent>
            | OpeningAct(_, earningPercentage) -> earningPercentage

        let accepted =
            $"Awesome! {headliner.Name} wants to play with you! Do you want to schedule the concert? They are offering you {earningPercentage |> Styles.percentage} of the ticket sales"
            |> Styles.success
            |> showConfirmationPrompt

        if accepted then
            Effect.apply effect
            Scene.Phone
        else
            promptForConcert app allPotentialConcerts date concertsOnDate
    | Error OpeningActOpportunities.NotEnoughFame ->
        $"{headliner.Name} rejected you because you don't have enough fans"
        |> Styles.error
        |> showMessage

        promptForConcert app allPotentialConcerts date concertsOnDate
    | Error OpeningActOpportunities.NotEnoughReleases ->
        $"{headliner.Name} rejected you because you haven't released anything yet"
        |> Styles.error
        |> showMessage

        promptForConcert app allPotentialConcerts date concertsOnDate
    | Error OpeningActOpportunities.AnotherConcertAlreadyScheduled ->
        $"You cant' play with {headliner.Name} on {Generic.dateWithDay date} because you already have a concert scheduled"
        |> Styles.error
        |> showMessage

        promptForConcert app allPotentialConcerts date concertsOnDate
    | Error OpeningActOpportunities.GenreMismatch ->
        $"{headliner.Name} rejected you because your genres are too different"
        |> Styles.error
        |> showMessage

        promptForConcert app allPotentialConcerts date concertsOnDate
