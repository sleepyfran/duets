namespace Duets.CityExplorer

open Avalonia
open Avalonia.Controls
open Avalonia.Controls.Shapes
open Avalonia.Layout
open Avalonia.Markup.Xaml
open Avalonia.Media
open Duets.Entities

type MainWindow() =
    inherit Window()

    // Helper to parse string to CityId DU
    let tryParseCityId (name: string) =
        match name with
        | "London" -> Some CityId.London
        | "LosAngeles" -> Some CityId.LosAngeles
        | "Madrid" -> Some CityId.Madrid
        | "MexicoCity" -> Some CityId.MexicoCity
        | "NewYork" -> Some CityId.NewYork
        | "Prague" -> Some CityId.Prague
        | "Sydney" -> Some CityId.Sydney
        | "Tokyo" -> Some CityId.Tokyo
        | _ -> None

    override this.OnInitialized() =
        base.OnInitialized()
        AvaloniaXamlLoader.Load(this)
        let world = Duets.Data.World.World.get
        let cityMap = world.Cities

        let cities =
            cityMap |> Map.toList |> List.map (fun (id, _) -> id.ToString())

        let listBox = this.FindControl<ListBox>("CitiesListBox")
        let zonesPanel = this.FindControl<StackPanel>("ZonesPanel")
        let zoneDetailsPanel = this.FindControl<StackPanel>("ZoneDetailsPanel")

        let placesSummaryPanel =
            this.FindControl<StackPanel>("PlacesSummaryPanel")

        let mutable detailsOpen = false

        listBox.ItemsSource <- cities

        listBox.SelectionChanged.Add(fun _ ->
            zonesPanel.Children.Clear()
            zoneDetailsPanel.Children.Clear()
            placesSummaryPanel.Children.Clear()

            // Show city overview panels, hide zone details
            zonesPanel.IsVisible <- true
            placesSummaryPanel.IsVisible <- true
            zoneDetailsPanel.IsVisible <- false
            detailsOpen <- false

            match listBox.SelectedItem with
            | :? string as cityName ->
                match tryParseCityId cityName with
                | Some cityId ->
                    let cityOpt = cityMap |> Map.tryFind cityId

                    match cityOpt with
                    | Some city ->
                        // Calculate and display places summary
                        let placesSummary =
                            city.Zones
                            |> Map.values
                            |> Seq.collect (fun zone ->
                                zone.Streets.Nodes |> Map.values)
                            |> Seq.collect _.Places
                            |> Seq.countBy (fun place ->
                                place.PlaceType |> World.Place.Type.toIndex)
                            |> Map.ofSeq

                        let summaryTitle =
                            TextBlock(
                                Text = "Places In City",
                                FontWeight = FontWeight.Bold,
                                Margin = Thickness(0.0, 0.0, 0.0, 4.0)
                            )

                        placesSummaryPanel.Children.Add(summaryTitle)

                        placesSummary
                        |> Map.iter (fun placeTypeIndex count ->
                            let placeTypeText = placeTypeIndex |> string

                            let summaryText =
                                TextBlock(Text = $"{placeTypeText}: {count}")

                            placesSummaryPanel.Children.Add(summaryText))


                        // For each metro line, draw a horizontal row of zones
                        city.MetroLines
                        |> Map.toList
                        |> List.iter (fun (lineId, line) ->
                            let lineStack =
                                StackPanel(
                                    Orientation = Orientation.Horizontal,
                                    Margin = Thickness(0.0, 0.0, 0.0, 16.0),
                                    Spacing = 8.0
                                )
                            // Choose color based on lineId
                            let lineColor =
                                match lineId with
                                | MetroLineId.Red -> Colors.IndianRed
                                | MetroLineId.Blue -> Colors.SteelBlue
                            // Get ordered list of zones for this line
                            let rec walkLine acc currentZoneId =
                                match
                                    line.Stations |> Map.tryFind currentZoneId
                                with
                                | Some(MetroStationConnection.OnlyNext next) ->
                                    walkLine (acc @ [ currentZoneId ]) next
                                | Some(MetroStationConnection.PreviousAndNext(_,
                                                                              next)) ->
                                    walkLine (acc @ [ currentZoneId ]) next
                                | _ -> acc @ [ currentZoneId ]
                            // Find the starting zone (no previous)
                            let startZoneId =
                                line.Stations
                                |> Map.tryFindKey (fun _ conn ->
                                    match conn with
                                    | MetroStationConnection.OnlyNext _ ->
                                        true
                                    | MetroStationConnection.PreviousAndNext(prev,
                                                                             _) ->
                                        not (line.Stations.ContainsKey prev)
                                    | _ -> false)
                                |> Option.defaultValue (
                                    line.Stations
                                    |> Map.toList
                                    |> List.head
                                    |> fst
                                )

                            let orderedZones = walkLine [] startZoneId
                            // Draw the line
                            orderedZones
                            |> List.iteri (fun i zoneId ->
                                // Check if this zone is a transfer (connected to 2+ lines)
                                let isTransfer =
                                    match city.Zones.TryGetValue(zoneId) with
                                    | true, zone ->
                                        zone.MetroStations.Count >= 2
                                    | _ -> false

                                let border = Border()

                                border.Background <-
                                    SolidColorBrush(Colors.White)

                                border.BorderBrush <-
                                    if isTransfer then
                                        SolidColorBrush(
                                            Colors.MediumSeaGreen
                                        )
                                    else
                                        SolidColorBrush(lineColor)

                                border.BorderThickness <- Thickness(2.0)
                                border.CornerRadius <- CornerRadius(8.0)
                                border.Padding <- Thickness(12.0)

                                let text =
                                    TextBlock(
                                        Text = city.Zones[zoneId].Name,
                                        FontSize = 16.0,
                                        Foreground =
                                            SolidColorBrush(Colors.Black)
                                    )

                                border.Child <- text

                                border.PointerPressed.Add(fun _ ->
                                    // Hide city overview panels and show zone details
                                    zonesPanel.IsVisible <- false
                                    placesSummaryPanel.IsVisible <- false
                                    zoneDetailsPanel.IsVisible <- true
                                    zoneDetailsPanel.Children.Clear()
                                    detailsOpen <- true

                                    let detailsStack = StackPanel()

                                    let arrowBtn =
                                        Button(
                                            Content = "← Back",
                                            Margin =
                                                Thickness(
                                                    0.0,
                                                    0.0,
                                                    0.0,
                                                    12.0
                                                )
                                        )

                                    arrowBtn.Click.Add(fun _ ->
                                        // Show city overview panels and hide zone details
                                        zoneDetailsPanel.IsVisible <- false
                                        zonesPanel.IsVisible <- true

                                        placesSummaryPanel.IsVisible <-
                                            true

                                        detailsOpen <- false)

                                    detailsStack.Children.Add(arrowBtn)

                                    let detailsText =
                                        TextBlock(
                                            Text =
                                                $"Zone: {city.Zones[zoneId].Name}",
                                            FontSize = 20.0,
                                            FontWeight = FontWeight.Bold,
                                            Margin =
                                                Thickness(
                                                    0.0,
                                                    0.0,
                                                    0.0,
                                                    8.0
                                                )
                                        )

                                    detailsStack.Children.Add(detailsText)
                                    let zone = city.Zones[zoneId]

                                    // --- STREET GRAPH ---
                                    let streetGraphCanvas =
                                        Canvas(
                                            Width = 400.0,
                                            Height = 200.0,
                                            Background =
                                                SolidColorBrush(
                                                    Colors.LightGray
                                                ),
                                            Margin =
                                                Thickness(
                                                    0.0,
                                                    0.0,
                                                    0.0,
                                                    16.0
                                                )
                                        )

                                    let streets = zone.Streets.Nodes

                                    let connections =
                                        zone.Streets.Connections

                                    if not streets.IsEmpty then
                                        let streetPositions =
                                            let angleStep =
                                                2.0 * System.Math.PI
                                                / float streets.Count

                                            let radius = 80.0
                                            let centerX = 200.0
                                            let centerY = 100.0

                                            streets
                                            |> Map.toList
                                            |> List.mapi
                                                (fun i (streetId, _) ->
                                                    let angle =
                                                        float i * angleStep

                                                    let x =
                                                        centerX
                                                        + radius
                                                          * System.Math.Cos(
                                                              angle
                                                          )

                                                    let y =
                                                        centerY
                                                        + radius
                                                          * System.Math.Sin(
                                                              angle
                                                          )

                                                    (streetId, Point(x, y)))
                                            |> Map.ofList

                                        // Draw edges first, so they appear behind nodes
                                        for startStreetId, endStreetMap in
                                            connections |> Map.toList do
                                            let startPos =
                                                streetPositions[startStreetId]

                                            for endStreetId in
                                                (endStreetMap |> Map.values) do
                                                if
                                                    startStreetId < endStreetId
                                                then
                                                    let endPos =
                                                        streetPositions[endStreetId]

                                                    let line =
                                                        Line(
                                                            StartPoint =
                                                                startPos,
                                                            EndPoint =
                                                                endPos,
                                                            Stroke =
                                                                SolidColorBrush(
                                                                    Colors.Black
                                                                ),
                                                            StrokeThickness =
                                                                1.0
                                                        )

                                                    streetGraphCanvas
                                                        .Children
                                                        .Add(line)

                                        // Draw nodes
                                        for streetId, street in
                                            streets |> Map.toList do
                                            let pos =
                                                streetPositions[streetId]

                                            let hasMetroStation =
                                                street.Places
                                                |> List.exists (fun p ->
                                                    p.PlaceType = PlaceType.MetroStation)

                                            let node =
                                                Ellipse(
                                                    Width = 20.0,
                                                    Height = 20.0,
                                                    Fill =
                                                        if
                                                            hasMetroStation
                                                        then
                                                            SolidColorBrush(
                                                                Colors.MediumSeaGreen
                                                            )
                                                        else
                                                            SolidColorBrush(
                                                                Colors.DarkSlateBlue
                                                            )
                                                )

                                            Canvas.SetLeft(
                                                node,
                                                pos.X - 10.0
                                            )

                                            Canvas.SetTop(
                                                node,
                                                pos.Y - 10.0
                                            )

                                            streetGraphCanvas.Children.Add(
                                                node
                                            )

                                            let label =
                                                TextBlock(
                                                    Text = street.Name,
                                                    FontSize = 12.0
                                                )

                                            Canvas.SetLeft(
                                                label,
                                                pos.X + 15.0
                                            )

                                            Canvas.SetTop(
                                                label,
                                                pos.Y - 8.0
                                            )

                                            streetGraphCanvas.Children.Add(
                                                label
                                            )

                                    detailsStack.Children.Add(
                                        streetGraphCanvas
                                    )

                                    // --- PLACES BY STREET, THEN TYPE ---
                                    for _, street in
                                        zone.Streets.Nodes |> Map.toList do
                                        let streetHeader =
                                            TextBlock(
                                                Text = street.Name,
                                                FontSize = 18.0,
                                                FontWeight = FontWeight.Bold,
                                                Margin =
                                                    Thickness(
                                                        0.0,
                                                        12.0,
                                                        0.0,
                                                        4.0
                                                    )
                                            )

                                        detailsStack.Children.Add(
                                            streetHeader
                                        )

                                        let placesByType =
                                            street.Places
                                            |> List.groupBy _.PlaceType

                                        for placeType, places in
                                            placesByType do
                                            let typeHeaderBorder =
                                                Border(
                                                    Background =
                                                        SolidColorBrush(
                                                            Colors.DarkSlateGray
                                                        ),
                                                    CornerRadius =
                                                        CornerRadius(6.0),
                                                    Padding =
                                                        Thickness(
                                                            8.0,
                                                            2.0,
                                                            8.0,
                                                            2.0
                                                        ),
                                                    Margin =
                                                        Thickness(
                                                            8.0,
                                                            8.0,
                                                            0.0,
                                                            2.0
                                                        )
                                                )

                                            let placeTypeText =
                                                placeType
                                                |> World.Place.Type.toIndex
                                                |> string

                                            let typeHeaderText =
                                                TextBlock(
                                                    Text = placeTypeText,
                                                    FontWeight =
                                                        FontWeight.Bold
                                                )

                                            typeHeaderBorder.Child <-
                                                typeHeaderText

                                            detailsStack.Children.Add(
                                                typeHeaderBorder
                                            )

                                            for place in places do
                                                let info =
                                                    match
                                                        place.PlaceType
                                                    with
                                                    | PlaceType.ConcertSpace c ->
                                                        $" ({c})"
                                                    | PlaceType.Hotel h ->
                                                        $" ({h})"
                                                    | PlaceType.RadioStudio r ->
                                                        $" ({r})"
                                                    | PlaceType.RehearsalSpace r ->
                                                        $" ({r})"
                                                    | PlaceType.Studio s ->
                                                        $" ({s})"
                                                    | _ -> ""

                                                let placeText =
                                                    TextBlock(
                                                        Text =
                                                            $"- {place.Name}{info}",
                                                        Margin =
                                                            Thickness(
                                                                16.0,
                                                                0.0,
                                                                0.0,
                                                                0.0
                                                            )
                                                    )

                                                detailsStack.Children.Add(
                                                    placeText
                                                )

                                    zoneDetailsPanel.Children.Add(
                                        detailsStack
                                    ))

                                lineStack.Children.Add(border)
                                // Draw a line between nodes except after the last
                                if i < orderedZones.Length - 1 then
                                    let connector =
                                        Border(
                                            Width = 32.0,
                                            Height = 4.0,
                                            Background =
                                                SolidColorBrush(lineColor),
                                            VerticalAlignment =
                                                VerticalAlignment.Center
                                        )

                                    lineStack.Children.Add(connector))
                            // Add a label for the line
                            let label =
                                TextBlock(
                                    Text = lineId.ToString(),
                                    Foreground = SolidColorBrush(lineColor),
                                    FontWeight = FontWeight.Bold,
                                    Margin = Thickness(0.0, 0.0, 0.0, 4.0)
                                )

                            zonesPanel.Children.Add(label)
                            zonesPanel.Children.Add(lineStack))
                    | None -> ()
                | None -> ()
            | _ -> ())
