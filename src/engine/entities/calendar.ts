export enum TimeOfDay {
    Dawn,
    Morning,
    Midday,
    Sunset,
    Dusk,
    Night,
    Midnight,
}

export type Calendar = {
    date: Date
    time: TimeOfDay
}
