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
                    '@config': './src/config.ts',
                    '@ui': './src/ui',
                    '@core': './src/core',
                    '@engine': './src/engine',
                    '@infrastructure': './src/infrastructure',
                    '@storage': './src/storage',
                    '@utils': './src/utils',
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
