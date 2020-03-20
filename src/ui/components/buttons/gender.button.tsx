import React, { FunctionComponent } from 'react'
import Button, { ButtonSize, ButtonType, ButtonStyle, BaseButtonProps } from '@ui/components/buttons/button'
import { ReactComponent as MaleIcon } from '@ui/assets/icons/male.svg'
import { ReactComponent as FemaleIcon } from '@ui/assets/icons/female.svg'
import { Gender } from '@engine/entities/gender'

type GenderButtonProps = {
    error?: boolean
    gender: Gender
} & BaseButtonProps

const GenderButton: FunctionComponent<GenderButtonProps> = props => {
    const icon = props.gender === Gender.Male ? <MaleIcon /> : <FemaleIcon />
    const type = props.gender === Gender.Male ? ButtonType.male : ButtonType.female

    return (
        <Button {...props} type={type} size={ButtonSize.regular} style={ButtonStyle.circular}>
            {icon}
        </Button>
    )
}

export default GenderButton
