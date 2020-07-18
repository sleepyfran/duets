mod common;
mod effects;
mod screens;

use common::action::ActionResult;
use common::orchestrator;
use screens::main_menu;

fn main() {
    let main_menu_screen = main_menu::create_main_screen();
    orchestrator::start(ActionResult::Screen(main_menu_screen));
}
