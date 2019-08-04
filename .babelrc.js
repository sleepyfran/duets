/**
 * Override Babel settings to allow the use of various things either not supported by CRA or not supported after
 * overriding the settings with Rescripts (Oh, what a wonderful time I'm having with all this!).
 */
module.exports = {
    presets: ['react-app'],
    plugins: [
        // This allows to define import aliases to the project.
        [
            'module-resolver',
            {
                root: '.',
                alias: {
                    '@ui': './src/ui',
                    '@core': './src/core',
                    '@infrastructure': './src/infrastructure',
                    '@persistence': './src/persistence',
                },
            },
        ],
        // This allows importing SVG files as React components.
        [
            'babel-plugin-named-asset-import',
            {
                loaderMap: {
                    svg: {
                        ReactComponent: '@svgr/webpack?-svgo,+ref![path]',
                    },
                },
            },
        ],
    ],
}
