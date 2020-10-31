use common::entities::{Instrument, Object, ObjectType};
use game::context::Context;
use game::operations::start::SavegameState;
use game::world::interactions::{get_for, InteractItem};

use crate::screens::create;
use crate::screens::GameScreen;

#[test]
fn create_should_return_main_menu_when_given_main_menu() {
    let screen = create(
        GameScreen::MainMenu(SavegameState::None(Context::default())),
        &Context::default(),
    );
    assert_eq!(screen.name, "Main Menu");
}

#[test]
fn create_should_return_home_when_given_home() {
    let screen = create(GameScreen::Home, &Context::default());
    assert_eq!(screen.name, "Home");
}

#[test]
fn create_should_return_new_game_when_given_new_game() {
    let screen = create(GameScreen::NewGame, &Context::default());
    assert_eq!(screen.name, "New game");
}

#[test]
fn create_should_return_interaction_when_given_interaction() {
    let dummy_instrument = Instrument::default();
    let dummy_object = Object {
        id: "".into(),
        name: "".into(),
        description: "".into(),
        r#type: ObjectType::Instrument(dummy_instrument),
    };
    let interaction = get_for(&dummy_object).into_iter().next().unwrap();
    let screen = create(
        GameScreen::Interaction {
            context: Context::default(),
            interaction,
            sequence: InteractItem::End,
        },
        &Context::default(),
    );
    assert_eq!(screen.name, "Interaction");
}
