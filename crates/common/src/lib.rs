#[macro_use]
extern crate derive_builder;

mod game_info;
pub mod serializables;

pub use game_info::{get_game_info, GameInfo};
