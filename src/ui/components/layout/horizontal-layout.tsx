import React, { FunctionComponent, ReactNode } from 'react'
import '@ui/styles/layout.scss'

export enum LayoutMode {
    Half,
    Default,
}

type LayoutProps = {
    className?: string
    mode?: LayoutMode
    left: ReactNode
    right: ReactNode
}

const HorizontalLayout: FunctionComponent<LayoutProps> = props => {
    const halfMode = props.mode === LayoutMode.Half

    return (
        <div className={`layout ${halfMode ? 'half' : 'default'} ${props.className}`}>
            {props.left}
            {props.right}
        </div>
    )
}

export default HorizontalLayout
