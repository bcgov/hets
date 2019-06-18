/*eslint-env node*/

const path = require('path');
const webpack = require('webpack');
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');

const IS_PRODUCTION = process.env.NODE_ENV === 'production';

// Include your SmUserId on the command line.
const DEV_USER = process.env.HETS_DEV_USER || '';

var webpackPlugins = [
  new webpack.ContextReplacementPlugin(/moment[/\\]locale$/, /en/),
];
var eslintDevRule = {};

const entrypoints = {
  app: [
    '@babel/polyfill',
    './src/js/init.js',
  ],
};

if(!IS_PRODUCTION) {
  webpackPlugins.push(new webpack.DefinePlugin({
    'process.env':{
      'DEV_USER': JSON.stringify(DEV_USER),
    },
  }));

  webpackPlugins.push(new webpack.HotModuleReplacementPlugin());
  entrypoints.app = [
    entrypoints.app[0],
    'react-hot-loader/patch', // has to be after @babel/polyfill
    'webpack-hot-middleware/client',
    entrypoints.app[1],
  ];

  eslintDevRule = {
    enforce: 'pre',
    test: /\.jsx?$/,
    loader: 'eslint-loader',
    exclude: /node_modules/,
  };
}


module.exports = {
  mode: IS_PRODUCTION ? 'production' : 'development',
  bail: IS_PRODUCTION,
  devtool: IS_PRODUCTION ? 'source-map' : 'source-map',
  entry: entrypoints,
  output: {
    path: path.resolve(__dirname, 'dist'),
    filename: '[name].js',
    chunkFilename: '[name].js',
    sourceMapFilename: 'maps/[file].map',
    publicPath: '/',
  },
  resolve: {
    alias: {
      'react-dom': IS_PRODUCTION ? 'react-dom' : '@hot-loader/react-dom',
    },
  },
  plugins: webpackPlugins,
  module: {
    rules: [
      eslintDevRule,
      {
        test: /\.jsx?$/,
        exclude: /node_modules/,
        loader: 'babel-loader',
      },
    ],
  },
  optimization: {
    noEmitOnErrors: IS_PRODUCTION,
    concatenateModules: true,
    splitChunks: {
      cacheGroups: {
        vendor: {
          test: /node_modules/,
          chunks: 'initial',
          name: 'vendor',
          enforce: true,
        },
      },
    } ,
    minimizer: [
      new UglifyJsPlugin({
        sourceMap: true,
      }),
    ],
  },
  performance: {
    hints: IS_PRODUCTION ? 'warning' : false,
    maxAssetSize: 1 * 1024 * 1024,
    maxEntrypointSize: 2 * 1024 * 1024,
  },
};