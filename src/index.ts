/**
 * This file will serve as the entry point of the app, here will be the configuration of our dependency injection
 * container and the initialization core parts of the game like the UI.
 */
import InitializeUi from '@ui/main'
import { version, homepage } from '../package.json'

InitializeUi({
    homepageUrl: homepage,
    sourceCodeUrl: homepage,
    version,
})
