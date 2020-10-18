pub mod instruments;

use common::entities::GameState;
use common::results::InteractResult;

pub type Result = (InteractResult, GameState);
