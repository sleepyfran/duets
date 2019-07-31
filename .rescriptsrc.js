/**
 * We need Rescript to load a custom target into Webpack but we don't want to have to eject the whole React App.
 */
module.exports = [require.resolve('./.webpack.config.js')]
