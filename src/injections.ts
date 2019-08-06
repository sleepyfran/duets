import Store from '@persistence/store/store'
import ChangelogsFetcher from '@infrastructure/changelogs'
import ChangelogsData from '@persistence/store/changelogs/changelogs.data'
import CreateChangelogsQuery from '@core/queries/changelogs'
import { Injections } from '@ui/contexts/injections.context'

const changelogsQuery = CreateChangelogsQuery(ChangelogsFetcher, ChangelogsData(Store.dispatch))

const injections: Injections = {
    changelogs: changelogsQuery,
}

export default injections
