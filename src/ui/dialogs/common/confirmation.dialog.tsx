import React, { FunctionComponent, ReactNode } from 'react'
import './confirmation.dialog.scss'

export type ConfirmationDialogProps = {
    title: string
    content: ReactNode
    choice: ReactNode
}

const ConfirmationDialog: FunctionComponent<ConfirmationDialogProps> = props => {
    return (
        <div className="confirmation-dialog">
            <h1>{props.title}</h1>
            {props.content}
            <div className="choice">{props.choice}</div>
        </div>
    )
}

export default ConfirmationDialog
