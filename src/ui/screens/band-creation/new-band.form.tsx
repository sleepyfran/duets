import React, { FunctionComponent } from 'react'
import { useSelector } from 'react-redux'
import TextInput from '@ui/components/inputs/text.input'
import { useForm } from '@ui/hooks/form.hooks'
import { stringToGenre, stringToRole, stringToString } from '@core/utils/mappers'
import { State } from '@persistence/store/store'
import SelectInput from '@ui/components/inputs/select.input'
import Button from '@ui/components/buttons/button'
import { useCommands } from '@ui/hooks/injections.hooks'

const NewBandForm: FunctionComponent = () => {
    const database = useSelector((state: State) => state.database)
    const genres = database.genres
    const roles = database.roles

    const genresSelect = genres.map(genre => ({
        label: genre.name,
        value: genre.name,
    }))

    const rolesSelect = roles.map(role => ({
        label: role.name,
        value: role.name,
    }))

    const form = useForm()

    const { content: name, bind: bindName } = form.withInput({ id: 'name', map: stringToString })
    const { content: genre, bind: bindGenre } = form.withInput({
        id: 'genre',
        map: value => stringToGenre(value, genres),
    })
    const { content: role, bind: bindRole } = form.withInput({
        id: 'role',
        map: value => stringToRole(value, roles),
    })

    const { createBand } = useCommands().forms.creation
    const handleCreate = () => {
        createBand({
            name,
            genre,
            role,
        }).then(result =>
            result.fold(form.markValidationErrors, () => alert('Created successfully, the rest will come soon!')),
        )
    }

    return (
        <div className="band-form">
            <h1>Create my own band</h1>
            <TextInput label="Name" {...bindName} />
            <SelectInput label="Genre" {...bindGenre} options={genresSelect} />
            <SelectInput label="Role" {...bindRole} options={rolesSelect} />
            <Button className="create-button" onClick={handleCreate}>
                Create my band
            </Button>
        </div>
    )
}

export default NewBandForm
