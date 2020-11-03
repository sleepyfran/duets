use common::entities::{Genre, Song, VocalStyle};
use common::extensions::option::OptionCloneExtensions;
use std::str::FromStr;

use crate::data;
use crate::world::interactions::{InputType, SequenceOutput};

#[derive(Clone)]
pub enum SongOutcome {
    NewSong {
        name: String,
        genre: Genre,
        vocal_style: VocalStyle,
    },
    ImproveSong(Song),
}

pub fn to_song_outcome(output: &SequenceOutput) -> SongOutcome {
    let first_item = &output.values[0];

    match first_item {
        InputType::Confirmation(value) => {
            if *value {
                to_improved_song_outcome(&output.clone().modify_values(|input| input[1..].to_vec()))
            } else {
                to_new_song_outcome(&output.clone().modify_values(|values| values[1..].to_vec()))
            }
        }
        _ => to_new_song_outcome(&output.clone().modify_values(|values| values[0..].to_vec())),
    }
}

fn to_new_song_outcome(output: &SequenceOutput) -> SongOutcome {
    let mut sequence = output.values.to_vec();
    let song_name = sequence.remove(0).as_text();
    let genre_id = sequence.remove(0).as_option().id;
    let genre = data::find_by_id(&output.context.database.genres, genre_id).unwrap_cloned();
    let vocal_style_name = sequence.remove(0).as_option().id;
    let vocal_style = VocalStyle::from_str(&vocal_style_name).unwrap();

    SongOutcome::NewSong {
        name: song_name,
        genre,
        vocal_style,
    }
}

fn to_improved_song_outcome(output: &SequenceOutput) -> SongOutcome {
    let mut sequence = output.values.to_vec();
    let song_id = sequence.remove(0).as_option().id;
    let song = output
        .context
        .game_state
        .character
        .songs_in_progress
        .get(&song_id)
        .unwrap_cloned();

    SongOutcome::ImproveSong(song)
}
