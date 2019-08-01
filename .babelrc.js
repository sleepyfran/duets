/**
 * Override Babel settings to allow the use of path aliases.
 */
module.exports = {
    presets: ['react-app'],
    plugins: [
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
    ],
}
