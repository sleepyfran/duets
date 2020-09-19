use app::context::Context;
use app::operations::start::SavegameState;

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
