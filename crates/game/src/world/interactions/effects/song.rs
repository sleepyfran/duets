use common::entities::Song;

use simulation::commands::songs::SongCommands;

use crate::world::interactions::{
    to_song_outcome, EffectType, Outcome, SequenceOutput, SongOutcome,
};

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

            output_with_song(output, updated_song).add_outcome(Outcome::Song(song_outcome))
        }
        SongOutcome::ImproveSong(song) => {
            let updated_song = apply_effect_to(effect_type, song);
            output_with_song(output, updated_song.clone())
                .add_outcome(Outcome::Song(SongOutcome::ImproveSong(updated_song)))
        }
    }
}

pub fn apply_effect_to(effect_type: EffectType, song: Song) -> Song {
    match effect_type {
        EffectType::Negative(amount) => song.decrease_quality_by(amount),
        EffectType::Positive(amount) => song.increase_quality_by(amount),
    }
}

pub fn output_with_song(output: &SequenceOutput, song: Song) -> SequenceOutput {
    output.modify_context(|context| {
        context.modify_game_state(|game_state| {
            game_state.modify_character(|character| character.add_or_modify_song_in_progress(song))
        })
    })
}
