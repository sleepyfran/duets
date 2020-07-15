mod common;
mod effects;
mod screens;

use common::display;
use screens::main_menu;

fn main() {
    let main_menu_screen = main_menu::create_main_screen();
    display::show(&main_menu_screen);
}
