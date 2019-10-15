import React, { FunctionComponent, useContext } from 'react'
import Button, { ButtonType } from '@ui/components/buttons/button'
import { useActions } from '@ui/hooks/injections.hooks'
import { useDialog } from '@ui/hooks/dialog.hooks'
import ConfirmationDialog from '@ui/dialogs/common/confirmation.dialog'
import DateInput from '@ui/components/inputs/date.input'
import { useInput } from '@ui/hooks/input.hooks'
import { stringToMaybeDate } from '@core/utils/mappers'
import { pipe } from 'fp-ts/es6/pipeable'
import { fold } from 'fp-ts/lib/Either'
import { FormContext } from '@ui/contexts/form.context'
import { CharacterCreationScreen } from '@ui/screens/screens'
import { useHistory } from 'react-router-dom'

const StartDateSelectionDialog: FunctionComponent = () => {
    const history = useHistory()
    const { hideDialog } = useDialog()

    const { content: startDate, bind: bindStartDate } = useInput('startDate', stringToMaybeDate)

    const { setStartDate } = useActions().creation
    const form = useContext(FormContext)
    const handleConfirm = () => {
        form.clearErrors()

        pipe(
            startDate,
            setStartDate,
            fold(
                validationErrors => form.markAllValidationErrors(validationErrors),
                date => {
                    hideDialog()
                    history.push(CharacterCreationScreen.path, date)
                },
            ),
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
