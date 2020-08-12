use serde::{Deserialize, Serialize};

use engine::entities::Room;

#[derive(Deserialize, Serialize)]
#[serde(remote = "Room")]
#[serde(rename_all = "camelCase")]
pub struct RoomDef {
    pub id: String,
    pub description: String,
}
