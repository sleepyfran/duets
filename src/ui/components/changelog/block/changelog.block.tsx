import React, { FunctionComponent } from 'react'
import './changelog.block.scss'
import markdownToHtml from '@ui/utils/markdown-to-html'

export type ChangelogBlockProps = {
    version: string
    releaseDate: Date
    changesMarkdown: string
}

const ChangelogBlock: FunctionComponent<ChangelogBlockProps> = props => {
    const htmlContent = markdownToHtml(props.changesMarkdown)

    return (
        <div className="changelog-block">
            <div className="header">
                <h2>v{props.version}</h2>
                <span>{props.releaseDate.toDateString()}</span>
            </div>
            <div className="changes" dangerouslySetInnerHTML={{ __html: htmlContent }}></div>
        </div>
    )
}

export default ChangelogBlock
