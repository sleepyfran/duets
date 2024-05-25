[<RequireQualifiedAccess>]
module Duets.Cli.Text.World.ConcertSpace

open Duets.Agents
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation

let private attendanceAwareDescription
    attendancePercentage
    onNoOne
    onFew
    onMany
    onFull
    =
    match attendancePercentage with
    | att when att <= 5<percent> -> List.sample onNoOne
    | att when att <= 45<percent> -> List.sample onFew
    | att when att <= 75<percent> -> List.sample onMany
    | _ -> List.sample onFull

let rec description (place: Place) (roomType: RoomType) =
    let currentBand = Queries.Bands.currentBand (State.get ())

    let scheduledConcert =
        Queries.Concerts.scheduledForRightNow
            (State.get ())
            currentBand.Id
            place.Id

    match scheduledConcert with
    | Some scheduledConcert ->
        let concert = Concert.fromScheduled scheduledConcert
        let attendancePercentage = Queries.Concerts.attendancePercentage concert
        roomDescription place roomType attendancePercentage
    | None -> roomDescription place roomType 0<percent>

and private roomDescription
    (place: Place)
    (roomType: RoomType)
    attendancePercentage
    =
    match roomType with
    | RoomType.Backstage -> backstageDescription place attendancePercentage
    | RoomType.Bar -> barDescription attendancePercentage
    | RoomType.Lobby -> lobbyDescription attendancePercentage
    | RoomType.Stage -> stageDescription attendancePercentage
    | _ -> failwith "Room type not supported in concert space"

and private backstageDescription (place: Place) attendancePercentage =
    let attendanceDescription =
        attendanceAwareDescription
            attendancePercentage
            [ "There's absolutely no sound coming from outside. It doesn't matter whether you close the door or not because there's no one on the outside to look through it." ]
            [ "You feel rather alone with only a mumbled idle chat coming from outside. Peaking through the door, you can see the place is quite empty." ]
            [ "You start hearing the chatting that is forming on the outside and getting louder. Peaking through the door, you can see the place is quite packed." ]
            [ "You can hear the crow talking and laughing on the outside as you get closer to the door. Peaking through the door, you can see the place is full." ]

    match place.Quality with
    | q when q < 30<quality> ->
        $"The backstage is more like the extra space of the cleaning room. Finding some place to put your instruments is hard given how packed it is with cleaning products. {attendanceDescription}"
    | q when q < 60<quality> ->
        $"The backstage looks okay, there's some drinks on the table. {attendanceDescription}"
    | _ ->
        $"The backstage looks incredible. There's a table full of food and drinks, plenty of space for your instruments and clothes. {attendanceDescription}"

and private barDescription attendancePercentage =
    attendanceAwareDescription
        attendancePercentage
        [ "As you step into the dimly lit bar, the air is heavy with the faint scent of stale beer and echoes of forgotten laughter. The once vibrant venue now stands deserted, its empty booths and stools waiting for a lively crowd that will never arrive. Behind the counter stands a lone bartender, his weary eyes reflecting the emptiness that surrounds him."
          "You enter the barren bar, the absence of chatter and clinking glasses creating an eerie silence. The dusty shelves behind the counter display rows of liquor bottles, their labels faded and forgotten. The bartender, dressed in a faded black shirt, leans against the counter, polishing a solitary glass with deliberate care, his movements reflecting a sense of longing for days gone by."
          "A solemn ambiance engulfs the desolate bar, the absence of music amplifying the emptiness. The dim lights hanging above cast elongated shadows, playing tricks on your eyes. The bartender, clad in a worn leather apron, surveys the deserted room with a mix of resignation and hope. His aged hands rest on the counter, a silent invitation for conversation that will never come."
          "The abandoned bar exudes a nostalgic charm, with peeling wallpaper and faded concert posters adorning the walls. The worn wooden floor creaks under your feet as you approach the lonely bartender, who leans wearily against the counter. Behind him, a tarnished mirror reflects an empty dance floor, once alive with the rhythm of music and dancing bodies."
          "You enter the hushed bar, the absence of human presence palpable in the stagnant air. The jukebox sits silently in the corner, a relic of forgotten melodies. The bartender, his face etched with stories untold, offers a melancholic smile as you take a seat on one of the empty stools. Behind him, a stack of untouched cocktail napkins stands as a reminder of the countless conversations that will never be had." ]
        [ "You enter the barren bar, the absence of chatter and clinking glasses creating an eerie silence. A small group of people huddled together at a table, their subdued conversations punctuating the stillness. The dusty shelves behind the counter display rows of liquor bottles, their labels faded and forgotten. The bartender, dressed in a faded black shirt, leans against the counter, polishing a solitary glass with deliberate care, his movements reflecting a sense of longing for days gone by."
          "A solemn ambiance engulfs the desolate bar, the absence of music amplifying the emptiness. Two friends sit at the bar, their eyes fixed on their drinks, lost in their own thoughts. The dim lights hanging above cast elongated shadows, playing tricks on your eyes. The bartender, clad in a worn leather apron, surveys the deserted room with a mix of resignation and hope. His aged hands rest on the counter, a silent invitation for conversation that will never come."
          "The abandoned bar exudes a nostalgic charm, with peeling wallpaper and faded concert posters adorning the walls. A small crowd gathers near the stage, their subdued excitement a flicker of life in the empty space. The worn wooden floor creaks under your feet as you approach the lonely bartender, who leans wearily against the counter. Behind him, a tarnished mirror reflects an empty dance floor, once alive with the rhythm of music and dancing bodies."
          "You enter the hushed bar, the absence of human presence palpable in the stagnant air. A couple sits at a corner booth, their whispered conversation barely audible. The jukebox sits silently in the corner, a relic of forgotten melodies. The bartender, his face etched with stories untold, offers a melancholic smile as you take a seat on one of the empty stools. Behind him, a stack of untouched cocktail napkins stands as a reminder of the countless conversations that will never be had." ]
        [ "You enter the bustling bar, the lively chatter of a crowded venue filling the air. People from all walks of life fill the booths and stools, engaged in animated conversations. The bartender, dressed in a crisp white shirt, skillfully moves behind the counter, serving drinks with a practiced efficiency, a beacon of normalcy amidst the chaos."
          "The energetic ambiance of the lively bar engulfs you as you step inside. The dance floor is filled with swirling bodies, moving to the rhythm of the music. Groups of friends occupy the booths, clinking glasses in celebration. The bartender, adorned in a stylish vest, effortlessly mixes cocktails, a master of his craft in this sea of revelry."
          "The vibrant bar hums with excitement as people mingle and laugh. The air is filled with the aroma of freshly poured drinks and the melodies of live music. Couples sway on the dance floor, lost in each other's arms. The bartender, exuding confidence, expertly tends to the crowded counter, skillfully juggling orders amidst the joyful chaos."
          "You enter the lively bar, its pulsating energy electrifying the air. The venue is packed with a diverse crowd, their laughter and conversations blending into a symphony of sound. The bartender, wearing a stylish bowtie, effortlessly navigates through the throng of patrons, serving drinks with a charismatic flair, the heart and soul of this vibrant gathering." ]
        [ "You squeeze your way through the crowded bar, the lively chatter and laughter of a packed venue filling the air. Every booth and stool is occupied, with people shoulder-to-shoulder, enjoying the vibrant atmosphere. The bartender, clad in a black vest, moves swiftly behind the counter, skillfully mixing drinks amidst the bustling chaos."
          "The energetic bar pulses with life as people revel in the crowded space. Every seat is taken, and the dance floor is a colorful mosaic of moving bodies. Laughter and cheers resonate through the air, creating an electric atmosphere. The bartender, donning a stylish apron, works tirelessly, pouring drinks with expert precision amid the joyful commotion."
          "The packed bar buzzes with excitement, the sound of lively conversations and clinking glasses echoing throughout the space. Every nook and cranny is filled with people enjoying themselves. The bartender, dressed in a crisp white shirt, skillfully navigates the crowded counter, effortlessly serving drinks with a smile, a beacon of efficiency amidst the overwhelming crowd."
          "You enter the bustling bar, the energetic atmosphere almost tangible. The venue is packed to the brim, with people laughing, dancing, and engaging in animated conversations. The bartender, sporting a bowtie, deftly moves behind the counter, expertly juggling orders and serving drinks with impeccable timing, a maestro orchestrating the symphony of the packed bar." ]

and private lobbyDescription attendancePercentage =
    attendanceAwareDescription
        attendancePercentage
        [ "You enter the spacious lobby of the concert space. The polished marble floors shine under the soft glow of elegant chandeliers. Empty ticket booths line the walls, waiting for customers who will never arrive. The silence in the air amplifies the emptiness of the space, creating a somber atmosphere."
          "The lobby stretches out before you, devoid of any human presence. The walls are adorned with grand artwork, and the sound of your footsteps echoes against the marble floor. The ticket booths remain unattended, a reminder of the absence of bustling activity. The stillness envelops the space, creating a serene ambiance."
          "As you step into the lobby, a sense of solitude washes over you. The high ceilings tower above, accentuating the vastness of the empty space. The ticket booths stand unoccupied, their windows reflecting the dim lights that line the corridor. The tranquility in the air allows for a moment of quiet contemplation before the anticipated event."
          "You find yourself in the quiet expanse of the lobby. The grandeur of the architecture is evident, with intricate patterns adorning the walls and a majestic staircase leading to higher levels. The ticket booths remain vacant, untouched by the hands of eager attendees. The absence of people allows you to appreciate the intricate details and immerse yourself in the tranquility of the surroundings."
          "As you enter the lobby, a sense of stillness greets you. The vast space is devoid of any human presence, offering a moment of solace before the impending excitement. The ticket booths stand empty, their counters pristine and awaiting the arrival of ticket holders. The hushed ambiance creates a serene atmosphere, inviting reflection amidst the anticipation." ]
        [ "You step into the lobby of the concert space, where a small group of people mill about, engaged in quiet conversations. The sound of their hushed voices echoes against the high ceilings. The ticket booths are manned by a couple of attendants, patiently waiting to assist anyone who might arrive. The lobby exudes a sense of anticipation, albeit on a smaller scale."
          "The lobby comes alive with a modest crowd. Groups of friends gather, exchanging excited whispers and discussing their expectations for the upcoming performance. The ticket booths are occupied by friendly attendants, ready to assist concertgoers with their inquiries. The low hum of conversation adds a touch of warmth to the atmosphere."
          "Amidst the grandeur of the lobby, a small number of attendees explore the space. Conversations fill the air as friends reunite and share their enthusiasm. The ticket booths are attended by a couple of helpful staff members, ensuring a smooth entry for those in need. The presence of a few people adds a gentle buzz of excitement to the surroundings."
          "The lobby hosts a small gathering of concertgoers. Laughter and chatter resonate through the open space, creating an intimate ambiance. The ticket booths are occupied by friendly attendants who provide guidance and assistance with genuine smiles. The mingling of a few people lends an air of anticipation, as everyone eagerly awaits the upcoming performance."
          "As you enter the lobby, you notice a small group of people engaged in lively conversation. Their enthusiasm is contagious, and you find yourself drawn into the pre-show energy. The ticket booths are manned by a few attentive staff members, ready to assist attendees. The modest crowd creates a sense of camaraderie, with anticipation lingering in every interaction." ]
        [ "As you enter the bustling lobby of the concert space, you're greeted by a sea of people. Excited chatter fills the air, blending with the energetic music playing in the background. The ticket booths are abuzz with activity, as concertgoers line up to collect their tickets. The lobby is a vibrant hub of anticipation, with friends meeting, merchandise stands bustling, and the air charged with pre-show excitement."
          "The lobby teems with an enthusiastic crowd. Conversations and laughter intermingle, creating a lively buzz that permeates the space. The ticket booths are surrounded by a throng of eager attendees, their anticipation palpable. The energy is infectious, as people gather in groups, sharing their excitement and building a sense of anticipation together."
          "The bustling lobby is a testament to the popularity of the concert. Waves of people fill the space, their animated conversations blending into a symphony of excitement. The ticket booths are surrounded by a crowd of eager fans, eagerly awaiting their turn. The lively atmosphere creates an electrifying energy that sets the stage for an unforgettable experience."
          "You find yourself in a crowded lobby, surrounded by a multitude of excited concertgoers. Laughter, cheers, and snippets of conversation fill the air, creating a vibrant tapestry of sound. The ticket booths are bustling with activity, attended by a team of dedicated staff members working diligently to accommodate the influx of attendees. The lobby is alive with the anticipation and shared enthusiasm of the crowd."
          "As you enter the lobby, you are swept up in a sea of people. The air vibrates with animated conversations, laughter, and the hum of excitement. The ticket booths are in constant motion, as a steady stream of attendees collects their passes. The bustling atmosphere in the lobby creates a sense of unity among the crowd, fueling the collective anticipation for the upcoming performance." ]
        [ "You enter the lobby of the concert space, and it's packed to the brim with enthusiastic concertgoers. The air is filled with a palpable energy, a mixture of anticipation and excitement. The ticket booths are surrounded by long queues of people eagerly awaiting their turn. The lobby is alive with animated conversations, laughter, and the occasional cheer. It's a testament to the undeniable popularity of the upcoming performance."
          "The lobby is a bustling hive of activity, brimming with excited concert attendees. Every corner is occupied, and the air reverberates with a buzz of anticipation. The ticket booths are enveloped in long queues, as the crowd eagerly awaits their chance to secure entry. The vibrant atmosphere in the lobby sets the stage for an unforgettable event, with the energy reaching a crescendo as the concert draws near."
          "As you step into the packed lobby, a wave of excitement washes over you. The space is alive with a sea of concertgoers, their anticipation tangible in the air. The ticket booths are surrounded by a mass of people, patiently waiting for their turn. Conversations blend into a constant hum, and the lobby exudes an electric atmosphere that promises an unforgettable experience."
          "The lobby is teeming with an exuberant crowd, their energy contagious. Excited chatter and laughter fill the air, creating a lively backdrop for the event. The ticket booths are overwhelmed by a sea of eager attendees, all vying to secure their passes. The fully packed lobby amplifies the sense of anticipation, with the collective enthusiasm reaching a fever pitch."
          "You find yourself amidst a bustling crowd in the lobby, which is filled to capacity with enthusiastic concertgoers. The air crackles with excitement, as conversations merge into an indistinguishable hum. The ticket booths are surrounded by a throng of people, each eager to claim their entry. The lobby pulsates with an infectious energy, creating an immersive experience even before the main event begins." ]

and private stageDescription attendancePercentage =
    attendanceAwareDescription
        attendancePercentage
        [ "The stage stands empty, awaiting the arrival of an enthusiastic crowd. Its vibrant lights and intricate set design are on full display, but the absence of an audience leaves the space in a state of anticipation. The silence amplifies the potential energy that will soon fill the air."
          "As you gaze upon the stage, you can envision the crowd that will soon gather. The lights shine brightly, illuminating the grandeur of the set. The stage stands empty, yet it exudes a sense of expectancy, as if yearning for the arrival of an eager audience."
          "The stage awaits the vibrant energy of a crowd, its empty expanse holding the promise of captivating performances. The lights twinkle in anticipation, eagerly waiting to cast their glow upon a sea of faces. The stage stands ready, knowing that soon it will be embraced by the dynamic presence of an enthusiastic audience."
          "The stage stands in solitude, its potential magnified by the absence of a crowd. The lights shimmer, revealing an intricate backdrop that yearns to be seen. The stillness in the air only heightens the anticipation, as the stage patiently awaits the arrival of an eager audience."
          "Awaiting the arrival of a lively crowd, the stage stands empty but not devoid of energy. The lights are poised to illuminate the space, casting a spotlight on the performances yet to unfold. The absence of people is but a temporary state, as the stage patiently anticipates the arrival of a captivated audience." ]
        [ "In the midst of the crowd, a small group of people gathers near the stage. Their excited whispers and anticipatory glances reveal their eagerness for the performance to begin. The stage looms before them, ready to be adorned with the captivating presence of both performers and a growing audience."
          "A few people gather near the stage, their eyes fixed on the empty space before them. They chat in hushed tones, exchanging whispers of anticipation. The stage stands as a focal point, drawing their attention and fueling their excitement for the forthcoming performances."
          "A handful of individuals have positioned themselves close to the stage, eager to be at the forefront of the action. Their anticipation is palpable as they converse with excitement, their eyes frequently glancing toward the empty platform. The stage beckons them forward, promising an unforgettable experience."
          "Amidst the crowd, a small group of individuals finds their way to the front, close to the stage. They engage in animated conversations, sharing their enthusiasm for the upcoming performances. Their eyes are fixated on the empty stage, eagerly anticipating the moment it comes alive with artists and a growing sea of spectators."
          "In the crowd, a few dedicated fans position themselves near the stage, vying for the best view. They exchange excited whispers and share their anticipation for the performances yet to come. Their proximity to the stage creates a sense of intimacy, as they eagerly await the moment the empty space transforms into a spectacle of music and entertainment." ]
        [ "The stage is surrounded by a sea of people, their energy infectious. Excited conversations and laughter fill the air, blending with the pulsating music. The crowd's anticipation is palpable as they eagerly await the performers who will soon grace the stage. The atmosphere is electric, with the stage serving as the epicenter of collective excitement."
          "A multitude of people fills the crowd, their voices blending into a symphony of anticipation. Laughter, cheers, and snippets of conversations create a vibrant backdrop for the stage. The energy is palpable as the crowd eagerly awaits the performances that will soon captivate their senses."
          "The stage is embraced by a bustling crowd, their animated conversations and enthusiastic cheers resonating throughout the space. The air is charged with anticipation as the audience eagerly awaits the performers who will command the stage. The crowd's collective excitement creates a buzzing atmosphere that sets the stage for an unforgettable experience."
          "A vibrant crowd gathers around the stage, their voices rising and falling in a harmonious chorus. Excitement permeates the air as they eagerly anticipate the artists who will soon take center stage. The stage becomes a focal point, drawing the attention and energy of the crowd, creating a dynamic atmosphere of shared enthusiasm."
          "The crowd envelops the stage, their presence a testament to the allure of the performances to come. Eager conversations, cheers, and applause create a constant hum of excitement. The stage basks in the collective energy of the crowd, eagerly awaiting the transformation from an empty platform to a captivating showcase of talent." ]
        [ "The stage is engulfed by a massive crowd, stretching as far as the eye can see. The audience's collective anticipation reverberates through the air, intensifying the atmosphere. The stage becomes a beacon, drawing the focus and energy of the crowd, as they eagerly await the unfolding of a mesmerizing spectacle."
          "A dense throng of people fills the stage, their presence overwhelming. Excitement permeates the air, as the crowd buzzes with anticipation. The stage, almost engulfed by the sea of faces, stands as the epicenter of a collective experience, ready to unleash an unforgettable performance upon the electrified audience."
          "The stage is packed with an exuberant crowd, their sheer numbers creating an electric atmosphere. The air reverberates with excited conversations, cheers, and applause. The stage becomes a focal point of energy and anticipation, as the densely packed crowd eagerly awaits the artists who will command the space."
          "A massive crowd surrounds the stage, their energy palpable. The air is alive with the hum of conversations, cheers, and applause. The stage stands at the center of a vibrant tapestry of people, their collective anticipation building to a crescendo. The atmosphere is charged, as the crowd eagerly awaits the performances that will ignite the stage."
          "The stage is swarmed by a massive throng of people, their sheer presence creating an immersive experience. The air is filled with an intoxicating blend of excited chatter, cheers, and applause. The stage becomes a pulsating epicenter of energy, as the crowd's anticipation reaches a fever pitch, eager to witness the grand spectacle that will unfold before them." ]
