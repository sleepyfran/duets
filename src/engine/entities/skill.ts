export enum SkillType {
    music = 'Music',
    production = 'Production',
    social = 'Social',
}

export type Skill = {
    name: string
    level: number
    type: SkillType
}
