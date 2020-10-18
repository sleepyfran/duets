#[macro_use]
extern crate derive_builder;

pub mod entities;
pub mod extensions;
mod game_info;
pub mod results;
pub mod serializables;
pub mod shared;

pub use game_info::{get_game_info, GameInfo};
