const electron = require('electron')
const path = require('path')
const { default: installExtension, REDUX_DEVTOOLS } = require('electron-devtools-installer')

const app = electron.app
const BrowserWindow = electron.BrowserWindow

/**
 * Checks if the app is running in dev mode or not.
 * Based on https://github.com/sindresorhus/electron-is-dev/blob/master/index.js
 */
const devMode = (() => {
    const environmentSet = 'ELECTRON_IS_DEV' in process.env
    const getFromEnv = parseInt(process.env.ELECTRON_IS_DEV, 10) === 1

    return environmentSet ? getFromEnv : !app.isPackaged
})()

/**
 * Returns the URL to load based on whether we're in dev mode or not.
 */
const appUrl = (() => (devMode ? 'http://localhost:3000' : `file://${path.join(__dirname, '../build/index.html')}`))()

/**
 * Returns whether the app should be started in windowed mode or not.
 */
const windowedMode = (() => {
    const args = process.argv
    const windowMode = args.length > 2 && args[2]
    return windowMode === 'windowed'
})()

/**
 * Creates a new window into duetsWindow.
 */
let duetsWindow
const createWindow = () => {
    duetsWindow = new BrowserWindow({
        width: 1280,
        height: 720,
        frame: false,
        fullscreen: !windowedMode,
        webPreferences: {
            nodeIntegration: true,
        },
    })

    duetsWindow.loadURL(appUrl)

    if (devMode) {
        duetsWindow.webContents.openDevTools()

        // Install the Redux DevTools for easier debugging.
        installExtension(REDUX_DEVTOOLS)
            .then(name => console.log(`Added Extension:  ${name}`))
            .catch(err => console.log('An error occurred: ', err))
    }

    duetsWindow.on('closed', () => (duetsWindow = null))
}

app.on('ready', createWindow)
app.on('window-all-closed', () => app.quit())
