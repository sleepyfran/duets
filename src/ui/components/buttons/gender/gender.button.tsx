import React, { FunctionComponent } from 'react'
import CircularButton from '@ui/components/buttons/circular/circular.button'
import { ReactComponent as MaleIcon } from '@ui/assets/icons/male.svg'
import { ReactComponent as FemaleIcon } from '@ui/assets/icons/female.svg'
import { Gender } from '@engine/entities/gender'
import './gender.button.scss'

type PlayButtonProps = {
    onClick: () => void
    selected: boolean
    gender: Gender
}

const PlayButton: FunctionComponent<PlayButtonProps> = props => {
    const icon = props.gender === Gender.Male ? <MaleIcon /> : <FemaleIcon />
    const colorClass = props.gender === Gender.Male ? 'male' : 'female'
    const selectedClass = props.selected ? 'selected' : ''

    return (
        <CircularButton
            circleClassName={`gender-button-circle ${selectedClass} ${colorClass}`}
            size="35"
            onClick={props.onClick}
        >
            {icon}
        </CircularButton>
    )
}

export default PlayButton
