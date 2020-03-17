/**
 * We need Rescript to modify certain properties but we don't want to have to eject the whole React App.
 */
module.exports = [
    'env',
    require.resolve('./.webpack.config.js'),
    {
        jest: config => {
            return {
                ...config,
                moduleNameMapper: {
                    '@config': '<rootDir>/src/config.ts',
                    '@ui/(.*)': '<rootDir>/src/ui/$1',
                    '@core/(.*)': '<rootDir>/src/core/$1',
                    '@engine/(.*)': '<rootDir>/src/engine/$1',
                    '@storage/(.*)': '<rootDir>/src/storage/$1',
                    '@infrastructure/(.*)': '<rootDir>/src/infrastructure/$1',
                    '@utils/(.*)': '<rootDir>/src/utils/$1',
                },
            }
        },
    },
]
