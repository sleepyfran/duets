import UiReducer from '../ui.reducer'
import { createHideDialogAction, createShowDialogAction } from '../ui.actions'
import { DialogType } from '../ui.state'

describe('UiReducer', () => {
    it('should return the UI state with the dialog set to database download when hide the showDialogAction with the database download dialog is given', () => {
        const result = UiReducer({ dialog: DialogType.hide }, createShowDialogAction(DialogType.databaseDownload))

        expect(result).toHaveProperty('dialog')
        expect(result.dialog).toEqual(DialogType.databaseDownload)
    })

    it('should return the UI state with the dialog set to hidden when hide the hideDialogAction is given', () => {
        const result = UiReducer({ dialog: DialogType.databaseDownload }, createHideDialogAction())

        expect(result).toHaveProperty('dialog')
        expect(result.dialog).toEqual(DialogType.hide)
    })
})
