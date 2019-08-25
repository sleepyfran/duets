import { CitiesActions } from '@persistence/store/database/cities/cities.actions'
import { InstrumentsActions } from '@persistence/store/database/instruments/instruments.actions'
import { SkillsActions } from '@persistence/store/database/skills/skills.actions'

export type DatabaseActions = CitiesActions | InstrumentsActions | SkillsActions
