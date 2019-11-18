import React, { FunctionComponent } from 'react'
import Button, { ButtonType } from '@ui/components/buttons/button'
import { useCommands } from '@ui/hooks/injections.hooks'
import { useDialog } from '@ui/hooks/dialog.hooks'
import ConfirmationDialog from '@ui/dialogs/common/confirmation.dialog'
import DateInput from '@ui/components/inputs/date.input'
import { stringToDate } from '@core/utils/mappers'
import { CharacterCreationScreen } from '@ui/screens/screens'
import { useHistory } from 'react-router-dom'
import { useForm } from '@ui/hooks/form.hooks'

const StartDateSelectionDialog: FunctionComponent = () => {
    const history = useHistory()
    const { hideDialog } = useDialog()

    const { validateStartDate } = useCommands().forms.creation
    const form = useForm()
    const { content: startDate, bind: bindStartDate } = form.withInput({
        id: 'startDate',
        map: stringToDate,
    })

    const handleConfirm = () => {
        form.clear()

        const result = validateStartDate(startDate, 'startDate')
        result.fold(
            errors => form.markValidationErrors(errors),
            date => {
                hideDialog()
                history.push(CharacterCreationScreen.path, date)
            },
        )
    }

    return (
        <ConfirmationDialog
            title="Game start date"
            content={
                <>
                    <p>In which date should the game start? You'll be able to create a character after this.</p>
                    <DateInput label="Game start date" maxDate={new Date()} {...bindStartDate} />
                </>
            }
            choice={
                <>
                    <Button buttonType={ButtonType.Warn} onClick={hideDialog}>
                        Cancel
                    </Button>
                    <Button onClick={handleConfirm}>Set start date</Button>
                </>
            }
        />
    )
}

export default StartDateSelectionDialog
