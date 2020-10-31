use common::entities::{Genre, Song, VocalStyle};
use common::extensions::option::OptionCloneExtensions;
use simulation::commands::songs::SongCommands;
use std::str::FromStr;

use crate::context::Context;
use crate::data;
use crate::world::interactions::{EffectType, InputType, SequenceInput};

enum SongInput {
    NewSong {
        name: String,
        genre: Genre,
        vocal_style: VocalStyle,
    },
    ImproveSong(Song),
}

/// Applies the given skill effect.
pub fn apply(effect_type: EffectType, input: &SequenceInput) -> Context {
    let song_input = to_song_input(input);

    match song_input {
        SongInput::NewSong {
            name,
            genre,
            vocal_style,
        } => {
            let song = Song::create(name, genre, vocal_style);
            let updated_song = apply_effect_to(effect_type, song);

            context_with_song(input.clone().context, updated_song)
        }
        SongInput::ImproveSong(song) => {
            context_with_song(input.clone().context, apply_effect_to(effect_type, song))
        }
    }
}

pub fn apply_effect_to(effect_type: EffectType, song: Song) -> Song {
    match effect_type {
        EffectType::Negative(amount) => song.decrease_quality_by(amount),
        EffectType::Positive(amount) => song.increase_quality_by(amount),
    }
}

pub fn context_with_song(context: Context, song: Song) -> Context {
    context.modify_game_state(|game_state| {
        game_state.modify_character(|character| character.add_or_modify_song_in_progress(song))
    })
}

fn to_song_input(input: &SequenceInput) -> SongInput {
    let first_item = &input.values[0];

    match first_item {
        InputType::Confirmation(value) => {
            if *value {
                to_previous_song_input(&input.clone().modify_input(|input| input[1..].to_vec()))
            } else {
                to_new_song_input(&input.clone().modify_input(|values| values[1..].to_vec()))
            }
        }
        _ => to_new_song_input(&input.clone().modify_input(|values| values[0..].to_vec())),
    }
}

fn to_new_song_input(input: &SequenceInput) -> SongInput {
    let mut sequence = input.values.to_vec();
    let song_name = sequence.remove(0).as_text();
    let genre_id = sequence.remove(0).as_option().id;
    let genre = data::find_by_id(&input.context.database.genres, genre_id).unwrap_cloned();
    let vocal_style_name = sequence.remove(0).as_option().id;
    let vocal_style = VocalStyle::from_str(&vocal_style_name).unwrap();

    SongInput::NewSong {
        name: song_name,
        genre,
        vocal_style,
    }
}

fn to_previous_song_input(input: &SequenceInput) -> SongInput {
    let mut sequence = input.values.to_vec();
    let song_id = sequence.remove(0).as_option().id;
    let song = input
        .context
        .game_state
        .character
        .songs_in_progress
        .get(&song_id)
        .unwrap_cloned();

    SongInput::ImproveSong(song)
}
