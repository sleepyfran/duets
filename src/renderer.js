const electron = require('electron')
const path = require('path')

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
 * Creates a new window into duetsWindow.
 */
let duetsWindow
const createWindow = () => {
    duetsWindow = new BrowserWindow({
        width: 1280,
        height: 720,
        frame: false,
        fullscreen: true,
        webPreferences: {
            nodeIntegration: true,
        },
    })

    duetsWindow.loadURL(appUrl)

    if (devMode) {
        duetsWindow.webContents.openDevTools()
    }

    duetsWindow.on('closed', () => (duetsWindow = null))
}

app.on('ready', createWindow)
app.on('window-all-closed', () => app.quit())
