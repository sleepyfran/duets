import React, { FunctionComponent } from 'react'
import Button, { BaseButtonProps, ButtonType, ButtonStyle } from '@ui/components/buttons/button'
import { ReactComponent as BandIcon } from '@ui/assets/icons/band.svg'
import { ReactComponent as CharacterIcon } from '@ui/assets/icons/artist.svg'
import { ReactComponent as CityIcon } from '@ui/assets/icons/city.svg'
import { ReactComponent as PhoneIcon } from '@ui/assets/icons/phone.svg'

export enum HomeButtonType {
    city,
    character,
    band,
    phone,
}

export type HomeButtonProps = {
    type: HomeButtonType
} & BaseButtonProps

const homeButtonIconMap = new Map([
    [HomeButtonType.band, BandIcon],
    [HomeButtonType.character, CharacterIcon],
    [HomeButtonType.city, CityIcon],
    [HomeButtonType.phone, PhoneIcon],
])

const HomeButton: FunctionComponent<HomeButtonProps> = props => {
    const Icon = homeButtonIconMap.get(props.type)

    return (
        <Button {...props} type={ButtonType.transparent} style={ButtonStyle.square}>
            {Icon ? <Icon /> : <></>}
        </Button>
    )
}

export default HomeButton
