import React, { FunctionComponent } from 'react'
import './full-size.sidebar.scss'

type FullSizeSidebarProps = {
    className: string
}

const FullSizeSidebar: FunctionComponent<FullSizeSidebarProps> = props => {
    return <div className={`full-size-sidebar ${props.className}`}>{props.children}</div>
}

export default FullSizeSidebar
