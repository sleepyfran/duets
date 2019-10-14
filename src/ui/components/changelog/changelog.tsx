import React, { FunctionComponent } from 'react'
import ChangelogBlock from './block/changelog.block'
import { ChangelogList } from '@core/entities/changelog'

type ChangelogProps = {
    changelogList: ChangelogList
}

const Changelog: FunctionComponent<ChangelogProps> = props => {
    return (
        <div className="changelog">
            {props.changelogList.length ? (
                props.changelogList.map((changelog, index) => <ChangelogBlock key={index} changelog={changelog} />)
            ) : (
                <p>No versions released yet.</p>
            )}
        </div>
    )
}

export default Changelog
