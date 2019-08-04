import React, { FunctionComponent } from 'react'
import './layout.scss'

export enum LayoutMode {
    half,
    default,
}

type LayoutProps = {
    mode?: LayoutMode
}

const Layout: FunctionComponent<LayoutProps> = props => {
    const halfMode = props.mode === LayoutMode.half

    return <div className={`layout ${halfMode ? 'half' : 'default'}`}>{props.children}</div>
}

export default Layout
