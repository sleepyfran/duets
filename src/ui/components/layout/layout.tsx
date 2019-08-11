import React, { FunctionComponent, ReactNode } from 'react'
import './layout.scss'

export enum LayoutMode {
    half,
    default,
}

type LayoutProps = {
    mode?: LayoutMode
    left: ReactNode
    right: ReactNode
}

const Layout: FunctionComponent<LayoutProps> = props => {
    const halfMode = props.mode === LayoutMode.half

    return (
        <div className={`layout ${halfMode ? 'half' : 'default'}`}>
            {props.left}
            {props.right}
        </div>
    )
}

export default Layout
