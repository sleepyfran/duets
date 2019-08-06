import ChangelogsReducer from '../changelogs.reducer'
import { createSaveErrorAction, createSaveChangelogsAction } from '../changelogs.actions'
import { Changelog } from '@core/entities/changelog'

describe('ChangelogsReducer', () => {
    it('should return an empty list when the saveChangelogsAction with an empty list is given', () => {
        const result = ChangelogsReducer('loading', createSaveChangelogsAction([]))

        expect(result).toHaveLength(0)
    })

    it('should return a given list when the saveChangelogsAction with such list is given', () => {
        const list: Changelog[] = [
            {
                version: '0',
                releaseDate: new Date(),
                body: '',
            },
        ]
        const result = ChangelogsReducer('loading', createSaveChangelogsAction(list))

        expect(result).toEqual(list)
    })

    it('should return the error state when the saveErrorAction is given', () => {
        const error = new Error()
        const result = ChangelogsReducer('loading', createSaveErrorAction(error))

        expect(result).toEqual(error)
    })
})
