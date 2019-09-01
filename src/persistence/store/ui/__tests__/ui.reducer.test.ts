import UiReducer from '../ui.reducer'
import { createHideDialogAction, createShowDialogAction } from '../ui.actions'
import { DialogType } from '../ui.state'

describe('UiReducer', () => {
    it('should return the UI state with the dialog set to database download progress when hide the showDialogAction with the database download progress dialog is given', () => {
        const result = UiReducer(
            { dialog: DialogType.Hide },
            createShowDialogAction(DialogType.DatabaseDownloadProgress),
        )

        expect(result).toHaveProperty('dialog')
        expect(result.dialog).toEqual(DialogType.DatabaseDownloadProgress)
    })

    it('should return the UI state with the dialog set to hidden when hide the hideDialogAction is given', () => {
        const result = UiReducer({ dialog: DialogType.Hide }, createHideDialogAction())

        expect(result).toHaveProperty('dialog')
        expect(result.dialog).toEqual(DialogType.Hide)
    })
})
