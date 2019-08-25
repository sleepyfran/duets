import React, { FunctionComponent } from 'react'
import './info.scss'

type InfoProps = {
    text: string
}

const info: FunctionComponent<InfoProps> = props => {
    return <div className="info">{props.text}</div>
}

export default info
