/**
 * Set `electron-renderer` as the target of Webpack. We need this in order to use modules such as `fs`.
 */
module.exports = config => {
    config.target = 'electron-renderer'
    return config
}
