module.exports = (api) => {
    api.cache(true);
    return {
        babelrcRoots: [
            // Keep the root as a root
            '.',
        ],
        presets: [
            ["@babel/env",
                {
                    modules: false,
                    targets: "> 0.25%",
                    corejs: "3.31.0",
                    useBuiltIns: "usage"
                }
            ],
            "@babel/preset-react",
            "@babel/preset-typescript",

        ],
        plugins: [
            "macros",
            "@babel/plugin-syntax-dynamic-import",
            "@babel/proposal-class-properties",
            "@babel/proposal-object-rest-spread",
            "@babel/plugin-proposal-throw-expressions",
            "@babel/plugin-transform-export-namespace-from"
        ],
        env: {
            test: {
                presets: [
                    [
                        '@babel/env',
                        {
                            useBuiltIns: 'usage',
                            targets: {
                                browsers: ['> 1%'],
                            },
                        },
                    ],
                    '@babel/typescript',
                    '@babel/react',
                ],
            },
        },
    };
}