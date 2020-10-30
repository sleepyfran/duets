use common::entities::Song;

pub trait SongCommands {
    /// Increases quality by the specified amount if possible (<= 100)
    fn increase_quality_by(self, amount: u8) -> Song;
    /// Decreases quality by the specified amount if possible (>0)
    fn decrease_quality_by(self, amount: u8) -> Song;
}

impl SongCommands for Song {
    fn increase_quality_by(self, amount: u8) -> Song {
        let quality = &self.quality.saturating_add(amount);
        self.with_quality(*quality)
    }

    fn decrease_quality_by(self, amount: u8) -> Song {
        let quality = &self.quality.saturating_sub(amount);
        self.with_quality(*quality)
    }
}
