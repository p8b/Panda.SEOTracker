import * as Webpack from 'webpack';
import * as path from 'path';
import 'webpack-dev-server';
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const TerserPlugin = require("terser-webpack-plugin");
const CopyPlugin = require("copy-webpack-plugin");
const HtmlWebPackPlugin = require("html-webpack-plugin");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");

const isDevelopment = true;
const appName = "SEO Tracker";

const config: Webpack.Configuration = {
    performance: {
        //hints: 'error',
        //maxEntrypointSize: 1024000,
        //maxAssetSize: 1024000
    },
    devServer: {
        server: {
            type: 'http'
        },
        compress: true,
        port: 8082,
        open: true,
        host: "localhost",
        historyApiFallback: true,
    },
    mode: (isDevelopment ? "development" : "production"),
    devtool: (isDevelopment ? "eval-source-map" : false),
    resolve: { extensions: [".js", ".tsx", ".css", ".scss", ".ts"] },
    entry: {
        index: {
            import: path.resolve(__dirname, "src/index.tsx"),
        },
    },
    output: {
        path: path.resolve(__dirname, "build"),
        filename: `public/js/${appName}.[name].[contenthash].bundle.js`,
        publicPath: "/",
        clean: true,
        asyncChunks: true,
    },
    externals: {
        //"react": "React",
        //"react-dom": "ReactDOM",
        //"react-router": "ReactRouter",
        //"react-router-dom": "ReactRouterDOM",
    },
    module: {
        rules: [
            {
                test: /\.(ts|js)x?$/,
                loader: "babel-loader",
                options: {
                    rootMode: 'upward',
                },
                exclude: [/node_modules/]
            },
            {
                test: /\.(s(a|c)ss)$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    {
                        loader: "css-loader",
                        options: {
                            url: false,
                            modules: false,
                            sourceMap: true,
                            importLoaders: 2
                        }
                    },
                    {
                        loader: "sass-loader",
                        options: {
                            sourceMap: true,
                            sassOptions: {
                                outputStyle: "compressed",
                            },
                        },
                    },
                ]
            },
        ]
    },
    plugins: [
        new CopyPlugin({
            patterns: [
                {
                    from: path.resolve(__dirname, "./server/web.config"),
                    to: path.resolve(__dirname, `./build/`)
                },
                {
                    from: path.resolve(__dirname, "./server/server.js"),
                    to: path.resolve(__dirname, `./build/`)
                },
                {
                    from: path.resolve(__dirname, "./public/favicon.ico"),
                    to: path.resolve(__dirname, './build/public/')
                },
                {
                    from: path.resolve(__dirname, "./public/robot.txt"),
                    to: path.resolve(__dirname, './build/public/')
                },
                {
                    from: path.resolve(__dirname, "./public/manifest.json"),
                    to: path.resolve(__dirname, './build/public/')
                },
                {
                    from: path.resolve(__dirname, "./public/images/"),
                    to: path.resolve(__dirname, './build/public/images/')
                }]
        }),
        new CleanWebpackPlugin(),
        new MiniCssExtractPlugin({
            filename: "public/css/[name]-[contenthash].css",
            chunkFilename: "public/css/[name]-[contenthash].css",
        }),
        new HtmlWebPackPlugin({
            template: path.join(__dirname, "/public/index.html"),
            filename: "index.html",
            scriptLoading: "defer",
        })
    ],
    optimization: {
        minimize: !isDevelopment,
        minimizer: [new TerserPlugin({
            parallel: true,
            terserOptions: {
                compress: { drop_console: true }
            }
        })],
        runtimeChunk: "single",
        splitChunks: {
            chunks: "all",
        },
    },
};
export default config;