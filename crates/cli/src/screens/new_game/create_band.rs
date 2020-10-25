use common::entities::{Genre, Instrument};
use game::builders::start::BandStartBuilder;

use super::super::GameScreen;
use crate::effects;
use crate::shared::action::{Choice, CliAction, Prompt, PromptText};
use crate::shared::context::{Context, ScreenContext};
use crate::shared::display;

pub type NewBandContext = ScreenContext<BandStartBuilder>;

pub fn create_starting_context(global_context: &Context) -> NewBandContext {
    NewBandContext {
        global_context: global_context.clone(),
        state: BandStartBuilder::default(),
        next_action: Some(Box::new(continue_to_genre_input)),
    }
}

/// Creates an input chain that asks for all the details necessary to create a band.
pub fn start_with_name_input(context: NewBandContext) -> CliAction {
    CliAction::Prompt(Prompt::TextInput {
        text: PromptText::WithEmoji(String::from(
            "Let's create your first band. What's the band's name?",
        )),
        on_action: Box::new(|input, global_context| {
            context.next_action.unwrap()(NewBandContext {
                global_context: global_context.clone(),
                state: BandStartBuilder::default().name(input),
                next_action: Some(Box::new(continue_to_instrument_input)),
            })
        }),
    })
}

pub fn continue_to_genre_input(context: NewBandContext) -> CliAction {
    let genres: Vec<(usize, Genre)> = context
        .global_context
        .database
        .genres
        .clone()
        .into_iter()
        .enumerate()
        .collect();

    CliAction::Prompt(Prompt::ChoiceInput {
        text: PromptText::WithEmoji(String::from(
            "What genre are they going to play? You'll be able to change this later",
        )),
        choices: genres
            .clone()
            .into_iter()
            .map(|(index, genre)| Choice {
                id: index,
                text: genre.name,
            })
            .collect(),
        on_action: Box::new(move |choice, global_context| {
            let selected_genre = genres[choice.id].1.clone();
            context.next_action.unwrap()(NewBandContext {
                global_context: global_context.clone(),
                state: context.state.genre(selected_genre),
                next_action: Some(Box::new(continue_to_confirmation)),
            })
        }),
    })
}

pub fn continue_to_instrument_input(context: NewBandContext) -> CliAction {
    let instruments: Vec<(usize, Instrument)> = context
        .global_context
        .database
        .instruments
        .clone()
        .into_iter()
        .enumerate()
        .collect();

    CliAction::Prompt(Prompt::ChoiceInput {
        text: PromptText::WithEmoji(String::from(
            "What instrument are you going to play? You'll be able to learn more later and use them when giving concerts or composing",
        )),
        choices: instruments
            .clone()
            .into_iter()
            .map(|(index, instrument)| Choice {
                id: index,
                text: instrument.name,
            })
            .collect(),
        on_action: Box::new(move |choice, global_context| {
            let selected_instrument = instruments[choice.id].1.clone();
            context.next_action.unwrap()(NewBandContext {
                global_context: global_context.clone(),
                state: context
                    .state
                    .starting_instrument(selected_instrument),
                next_action: None,
            })
        }),
    })
}

fn continue_to_confirmation(context: NewBandContext) -> CliAction {
    let band = context
        .state
        .starting_character(context.global_context.game_state.character)
        .build()
        .unwrap()
        .to_band(&context.global_context.game_state.calendar);

    display::show_line_break();
    display::show_text_with_new_line(&String::from("We have everything!"));
    display::show_line_break();
    display::show_warning(&format!(
        "This will create your first band, called {}, playing {} with your character playing the {}",
        band.name,
        band.genre.name,
        band.members[0].instruments[0].name,
    ));

    CliAction::Prompt(Prompt::ChoiceInput {
        text: PromptText::WithEmoji(String::from(
            "Do you want to create it? You will be able to edit any of these details later",
        )),
        choices: vec![
            Choice {
                id: 0,
                text: String::from("Yes, let me play!"),
            },
            Choice {
                id: 1,
                text: String::from("No, actually..."),
            },
        ],
        on_action: Box::new(|choice, global_context| match choice.id {
            0 => {
                display::show_line_break();
                display::show_text(&String::from("Awesome!"));

                CliAction::Chain(
                    Box::new(effects::set_state(
                        global_context.clone().game_state.with_band(band),
                    )),
                    Box::new(CliAction::Screen(GameScreen::Home)),
                )
            }
            1 => {
                display::show_text(&String::from("Let's get to it"));
                start_with_name_input(NewBandContext {
                    global_context: global_context.clone(),
                    state: BandStartBuilder::default(),
                    next_action: Some(Box::new(continue_to_genre_input)),
                })
            }
            _ => CliAction::NoOp,
        }),
    })
}
