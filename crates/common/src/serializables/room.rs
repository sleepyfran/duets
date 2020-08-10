use serde::{Deserialize, Serialize};

use engine::entities::Room;

#[derive(Deserialize, Serialize)]
#[serde(remote = "Room")]
#[serde(rename_all = "camelCase")]
pub struct RoomDef {
    pub description: String,
}
