use common::entities::{Instrument, Skill, SkillCategory, SkillWithLevel};
use game::context::Context;
use game::world::interactions::*;

fn create_interaction() -> instruments::compose::ComposeInteraction {
    instruments::compose::ComposeInteraction {
        instrument: Instrument {
            name: "test".into(),
            allows_another_instrument: false,
            associated_skill: Skill {
                name: "test".into(),
                category: SkillCategory::Music,
            },
        },
    }
}

fn create_context_with_skill_level(level: u8) -> Context {
    Context::default().modify_game_state(|state| {
        state.modify_character(|character| {
            character.with_skill(SkillWithLevel {
                name: "test".into(),
                category: SkillCategory::Music,
                level: 10,
            })
        })
    })
}

fn assert_song_improved_by(effect: &InteractionEffect, amount: u8) {
    match effect {
        InteractionEffect::Song(EffectType::Positive(2)) => {}
        _ => panic!("Wrong value"),
    }
}

#[test]
fn compose_effects_should_improve_song() {
    let effects = create_interaction().effects(&create_context_with_skill_level(10));
    let song_effect = effects.applied_after_interaction.first().unwrap();
    assert_song_improved_by(song_effect, 2);

    let effects = create_interaction().effects(&create_context_with_skill_level(52));
    let song_effect = effects.applied_after_interaction.first().unwrap();
    assert_song_improved_by(song_effect, 10);

    let effects = create_interaction().effects(&create_context_with_skill_level(70));
    let song_effect = effects.applied_after_interaction.first().unwrap();
    assert_song_improved_by(song_effect, 14);

    let effects = create_interaction().effects(&create_context_with_skill_level(100));
    let song_effect = effects.applied_after_interaction.first().unwrap();
    assert_song_improved_by(song_effect, 20);
}
