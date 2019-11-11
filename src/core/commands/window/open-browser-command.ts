import WindowInteractor from '@core/interfaces/window'

export type OpenBrowserCommand = (url: string) => void

/**
 * Creates a command that opens an URL in the default browser.
 * @param windowInteractor WindowInteractor dependency.
 */
export default (windowInteractor: WindowInteractor): OpenBrowserCommand => (url: string) =>
    windowInteractor.openInBrowser(url)
