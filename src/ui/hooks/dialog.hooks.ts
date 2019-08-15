import { useDispatch } from 'react-redux'
import { DialogType } from '@persistence/store/ui/ui.state'
import { createHideDialogAction, createShowDialogAction } from '@persistence/store/ui/ui.actions'

/**
 * Defines a hook that exports utility methods to show and hide dialogs in the app.
 */
export const useDialog = () => {
    const dispatch = useDispatch()

    const showDialog = (dialog: DialogType) => {
        dispatch(createShowDialogAction(dialog))
    }
    const hideDialog = () => {
        dispatch(createHideDialogAction())
    }

    return { showDialog, hideDialog }
}
