module Duets.Cli.Text.Career

open Duets.Common
open Duets.Entities

module Barista =
    let careerStageName (CareerStageId stage) =
        match stage with
        | 0uy -> "Dishwasher"
        | 1uy -> "Junior Barista"
        | 2uy -> "Barista"
        | 3uy -> "Senior Barista"
        | _ -> "Manager"

module Bartender =
    let careerStageName (CareerStageId stage) =
        match stage with
        | 0uy -> "Dishwasher"
        | 1uy -> "Table cleaner"
        | 2uy -> "Bartender"
        | 3uy -> "Mixologist"
        | _ -> "Manager"

module MusicProducer =
    let careerStageName (CareerStageId stage) =
        match stage with
        | 0uy -> "Assistant Producer"
        | 1uy -> "Junior Producer"
        | 2uy -> "Producer"
        | 3uy -> "Senior Producer"
        | _ -> "Distinguished Producer"

let name (job: Job) =
    match job.Id with
    | Barista -> Barista.careerStageName job.CurrentStage.Id
    | Bartender -> Bartender.careerStageName job.CurrentStage.Id
    | MusicProducer -> MusicProducer.careerStageName job.CurrentStage.Id

let typeName id =
    match id with
    | Barista -> "Barista"
    | Bartender -> "Bartender"
    | MusicProducer -> "Music Producer"

let shiftDurationDescription schedule =
    match schedule with
    | JobSchedule.Free shiftDuration ->
        $"""{shiftDuration} {Generic.simplePluralOf "day moment" shiftDuration} per shift"""

let scheduleDescription schedule =
    match schedule with
    | JobSchedule.Free _ ->
        $"""No schedule, {shiftDurationDescription schedule}"""

let careerChange (job: Job) placeName =
    Styles.success $"You now work as {name job} at {Styles.place placeName}"

let careerLeft (job: Job) placeName =
    Styles.danger $"You left your job as {name job} at {Styles.place placeName}"

let careerPromoted (job: Job) placeName salary =
    Styles.success
        $"You got promoted to {name job} at {Styles.place placeName}. You will now earn {Styles.money salary} per day moment"

let workShiftEvent (job: Job) =
    match job.Id with
    | Barista ->
        [ "Prepare to face the morning rush with customers whose coffee orders are longer than a Shakespearean soliloquy..."
          "Your job today involves deciphering the secret coffee codes of a regular named 'Espresso Macchiato with a twist of caramel and a dash of unicorn tears...'"
          "Get ready to craft lattes so intricate, they belong in an art gallery. Latte art skills are your secret weapon against coffee chaos..."
          "Welcome to the world of caffeinated chaos, where the espresso machine is your trusty steed, and the milk frother is your loyal sidekick..."
          "In this coffee battleground, you'll find customers who insist on ordering 'iced coffee, but not too cold' and 'hot chocolate, extra cold...'" ]
    | Bartender ->
        [ "Prepare to mix drinks for customers who think 'just a splash' means half the bottle..."
          "Your job today involves decoding cocktail orders like 'I want something strong but not too strong, sweet but not too sweet, and it should glow in the dark...'"
          "Get ready to shake, stir, and garnish concoctions so elaborate, they could star in their own reality show..."
          "Welcome to the world of mixology mayhem, where your cocktail shaker is your trusty weapon, and the bar is your battleground..."
          "In this mixological madness, you'll encounter patrons who order 'a drink that tastes like the color blue' and 'something that transports me to a tropical beach, minus the sand...'" ]
    | MusicProducer ->
        [ "Get ready to produce the musical equivalent of a herd of tone-deaf cats in a room full of broken instruments. It's your chance to turn this disaster into a symphony of chaos!"
          "Brace yourself for a band with more egos than members. It's your job to make them sound harmonious, or at least get them to agree on a pizza topping."
          "Welcome to the gig where you're in charge of a band that thinks 'tuning' is a myth. Can you turn this cacophony into a chart-topping hit?"
          "Your new band's lead singer is convinced they're the next pop sensation, while the guitarist believes they're a reincarnation of a rock legend. Good luck finding common ground!"
          "You're about to work with a talented indie rock band known for their unique blend of folk and alternative sounds."
          "You'll take the reins of a promising young pop group. They're eager to break into the industry with their catchy tunes and energetic performances."
          "Get ready to produce the next big hip-hop sensation. This rap collective has a strong message to convey through their lyrics, and it's up to you to make their beats shine."
          "Your task today is to work with a seasoned jazz ensemble. These musicians have a deep appreciation for improvisation, and it's your job to capture their magic in the studio."
          "Today you have a well-established country band. Their storytelling through music has earned them a loyal fanbase, and you're here to help them craft their next hit album." ]
    |> List.sample
    |> Styles.progress

let workShiftFinished salary =
    Styles.success $"You finished your shift and earned {Styles.money salary}"
