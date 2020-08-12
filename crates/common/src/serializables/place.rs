use serde::{Deserialize, Serialize};

use engine::entities::{Place, Room};

#[derive(Deserialize, Serialize)]
#[serde(remote = "Place")]
pub struct PlaceDef {
    pub id: String,
    pub name: String,
    #[serde(with = "room_vec")]
    pub rooms: Vec<Room>,
}

/// We need to define a custom deserializer because Serde does not support containers right now.
///
/// Tracking issue: https://github.com/serde-rs/serde/issues/723
/// Taken from: https://github.com/serde-rs/serde/issues/723#issuecomment-382501277
mod room_vec {
    use engine::entities::Room;
    use serde::ser::SerializeSeq;
    use serde::{Deserialize, Deserializer, Serialize, Serializer};

    pub fn deserialize<'de, D>(deserializer: D) -> Result<Vec<Room>, D::Error>
    where
        D: Deserializer<'de>,
    {
        #[derive(Deserialize)]
        struct Wrapper(#[serde(with = "super::super::RoomDef")] Room);

        let v = Vec::deserialize(deserializer)?;
        Ok(v.into_iter().map(|Wrapper(a)| a).collect())
    }

    pub fn serialize<S>(value: &Vec<Room>, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: Serializer,
    {
        #[derive(Serialize)]
        struct Wrapper<'s>(#[serde(with = "super::super::RoomDef")] &'s Room);

        let mut seq = serializer.serialize_seq(Some(value.len()))?;
        for e in value {
            seq.serialize_element(&Wrapper(e))?;
        }
        seq.end()
    }
}
