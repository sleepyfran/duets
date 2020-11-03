use simulation::commands::calendar::CalendarCommands;

use crate::world::interactions::{SequenceOutput, TimeConsumption};

pub fn apply(consumption: TimeConsumption, output: &SequenceOutput) -> SequenceOutput {
    output.modify_context(|context| {
        context.modify_game_state(|game_state| {
            game_state.modify_calendar(|calendar| match consumption {
                TimeConsumption::None => calendar,
                TimeConsumption::TimeUnit(times) => calendar.increase_time(times),
                TimeConsumption::Days(days) => calendar.increase_days(days),
            })
        })
    })
}
