import React, { FunctionComponent } from 'react'
import './changelog.scss'
import ChangelogBlock, { ChangelogBlockProps } from './block/changelog.block'

type ChangelogProps = {
    changelogBlocks: ChangelogBlockProps[]
}

const Changelog: FunctionComponent<ChangelogProps> = props => {
    return (
        <div className="changelog">
            {props.changelogBlocks.length ? (
                props.changelogBlocks.map((change, index) => (
                    <ChangelogBlock
                        key={index}
                        version={change.version}
                        releaseDate={change.releaseDate}
                        changesMarkdown={change.changesMarkdown}
                    />
                ))
            ) : (
                <p>No versions released yet.</p>
            )}
        </div>
    )
}

export default Changelog
