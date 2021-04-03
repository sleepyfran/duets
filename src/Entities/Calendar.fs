module Entities.Calendar

type Month =
  | January
  | February
  | March
  | April
  | May
  | June
  | July
  | August
  | September
  | October
  | November
  | December

/// Defines a custom in-game date. All dates in the game start in a year 0 and
/// go on from there instead of following real life years.
type Date = { Day: int; Month: Month; Year: int }

type PeriodEnd =
  | Date of Date
  | Ongoing

/// Defines a period of time with a start and an optional end.
type Period = Date * PeriodEnd

/// Transforms an int into a Month. If the given number is outside the 1-12
/// range it returns January as a default value.
let monthFromInt monthIdx =
  match monthIdx with
  | 1 -> Month.January
  | 2 -> Month.February
  | 3 -> Month.March
  | 4 -> Month.April
  | 5 -> Month.May
  | 6 -> Month.June
  | 7 -> Month.July
  | 8 -> Month.August
  | 9 -> Month.September
  | 10 -> Month.October
  | 11 -> Month.November
  | 12 -> Month.December
  | _ -> Month.January

/// Creates a new date from a given day an month with the year set to the
/// beginning of the game (Year 1).
let fromDayMonth day month =
  { Day = day
    Month = monthFromInt month
    Year = 1 }
