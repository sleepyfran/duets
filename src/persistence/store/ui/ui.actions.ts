import { DialogType } from './ui.state'

export type HideDialogAction = {
    type: 'hideDialog'
}

export type ShowDialogAction = {
    type: 'showDialog'
    dialog: DialogType
}

export type UiActions = HideDialogAction | ShowDialogAction

export const createHideDialogAction = (): HideDialogAction => ({
    type: 'hideDialog',
})

export const createShowDialogAction = (dialog: DialogType): ShowDialogAction => ({
    type: 'showDialog',
    dialog,
})
