use chrono::NaiveDate;
use serde::{Deserialize, Serialize};

use engine::entities::{Band, BandMember, Character, Genre, Instrument};

#[derive(Deserialize, Serialize)]
#[serde(remote = "BandMember")]
pub struct BandMemberDef {
    #[serde(with = "super::CharacterDef")]
    pub character: Character,
    #[serde(with = "super::naivedate::date")]
    pub since: NaiveDate,
    #[serde(with = "super::naivedate::option")]
    pub until: Option<NaiveDate>,
    #[serde(with = "super::instrument")]
    pub instruments: Vec<Instrument>,
}

/// Represents a band in the game.
#[derive(Deserialize, Serialize)]
#[serde(remote = "Band")]
pub struct BandDef {
    pub name: String,
    #[serde(with = "super::GenreDef")]
    pub genre: Genre,
    #[serde(with = "band_member_vec")]
    pub members: Vec<BandMember>,
}

/// We need to define a custom deserializer because Serde does not support containers right now.
///
/// Tracking issue: https://github.com/serde-rs/serde/issues/723
/// Taken from: https://github.com/serde-rs/serde/issues/723#issuecomment-382501277
mod band_member_vec {
    use engine::entities::BandMember;
    use serde::ser::SerializeSeq;
    use serde::{Deserialize, Deserializer, Serialize, Serializer};

    pub fn deserialize<'de, D>(deserializer: D) -> Result<Vec<BandMember>, D::Error>
    where
        D: Deserializer<'de>,
    {
        #[derive(Deserialize)]
        struct Wrapper(#[serde(with = "super::BandMemberDef")] BandMember);

        let v = Vec::deserialize(deserializer)?;
        Ok(v.into_iter().map(|Wrapper(a)| a).collect())
    }

    pub fn serialize<S>(value: &Vec<BandMember>, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: Serializer,
    {
        #[derive(Serialize)]
        struct Wrapper<'s>(#[serde(with = "super::BandMemberDef")] &'s BandMember);

        let mut seq = serializer.serialize_seq(Some(value.len()))?;
        for e in value {
            seq.serialize_element(&Wrapper(e))?;
        }
        seq.end()
    }
}
