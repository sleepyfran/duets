import React, { FunctionComponent } from 'react'
import '@ui/styles/changelog.block.scss'
import markdownToHtml from '@ui/utils/markdown-to-html'
import { Changelog } from '@core/entities/changelog'

export type ChangelogBlockProps = {
    changelog: Changelog
}

const ChangelogBlock: FunctionComponent<ChangelogBlockProps> = props => {
    const changelog = props.changelog
    const htmlContent = markdownToHtml(changelog.body)

    return (
        <div className="changelog-block">
            <div className="header">
                <h2>v{changelog.version}</h2>
                <span>{changelog.releaseDate.toDateString()}</span>
            </div>
            <div className="changes" dangerouslySetInnerHTML={{ __html: htmlContent }}></div>
        </div>
    )
}

export default ChangelogBlock
