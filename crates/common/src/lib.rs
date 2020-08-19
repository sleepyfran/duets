#[macro_use]
extern crate derive_builder;

pub mod entities;
mod game_info;
pub mod serializables;
pub mod shared;

pub use game_info::{get_game_info, GameInfo};
