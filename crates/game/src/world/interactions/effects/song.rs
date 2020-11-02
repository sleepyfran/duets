use common::entities::Song;

use simulation::commands::songs::SongCommands;

use crate::world::interactions::{
    to_song_outcome, EffectType, Outcome, SequenceOutput, SongOutcome,
};

/// Creates a new song or improves a previously selected one and puts it into the context.
pub fn apply(effect_type: EffectType, output: &SequenceOutput) -> SequenceOutput {
    let song_outcome = to_song_outcome(output);

    match song_outcome.clone() {
        SongOutcome::NewSong {
            name,
            genre,
            vocal_style,
        } => {
            let song = Song::create(name, genre, vocal_style);
            let updated_song = apply_effect_to(effect_type, song);

            output_with_song(output, updated_song, song_outcome)
        }
        SongOutcome::ImproveSong(song) => {
            output_with_song(output, apply_effect_to(effect_type, song), song_outcome)
        }
    }
}

pub fn apply_effect_to(effect_type: EffectType, song: Song) -> Song {
    match effect_type {
        EffectType::Negative(amount) => song.decrease_quality_by(amount),
        EffectType::Positive(amount) => song.increase_quality_by(amount),
    }
}

pub fn output_with_song(
    output: &SequenceOutput,
    song: Song,
    outcome: SongOutcome,
) -> SequenceOutput {
    output
        .modify_context(|context| {
            context.modify_game_state(|game_state| {
                game_state
                    .modify_character(|character| character.add_or_modify_song_in_progress(song))
            })
        })
        .add_outcome(Outcome::Song(outcome))
}
