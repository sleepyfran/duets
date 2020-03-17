import { Dialog } from '@core/entities/dialog'
import { useStorage } from './storage.hooks'

/**
 * Defines a hook that exports utility methods to show and hide dialogs in the app.
 */
export const useDialog = () => {
    const [getStorage, setStorage] = useStorage()
    const storage = getStorage()

    const showDialog = (dialog: Dialog) => {
        storage.ui.dialog = dialog
        setStorage(storage)
    }
    const hideDialog = () => {
        storage.ui.dialog = Dialog.Hide
        setStorage(storage)
    }

    return { showDialog, hideDialog }
}
