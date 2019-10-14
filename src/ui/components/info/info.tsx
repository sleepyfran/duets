import React, { FunctionComponent } from 'react'
import '@ui/styles/info.scss'

type InfoProps = {
    text: string
}

const info: FunctionComponent<InfoProps> = props => {
    return <div className="info">{props.text}</div>
}

export default info
