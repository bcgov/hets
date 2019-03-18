/*eslint-env node*/

const path = require('path');
const webpack = require('webpack');
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');

const IS_PRODUCTION = process.env.NODE_ENV === 'production';

// Include your SmUserId on the command line.
const DEV_USER = process.env.HETS_DEV_USER || '';

var webpackPlugins = [];
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
  entrypoints.app.unshift('webpack-hot-middleware/client');

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
  devtool: IS_PRODUCTION ? 'source-map' : 'cheap-source-map',
  entry: entrypoints,
  output: {
    path: path.join(__dirname, '/dist/'),
    filename: '[name].js',
    chunkFilename: '[name].js',
    sourceMapFilename: 'maps/[file].map',
    publicPath: '/',
  },
  plugins: webpackPlugins,
  module: {
    rules: [
      eslintDevRule,
      {
        test: /\.jsx?$/,
        exclude: /node_modules/,
        loaders: ['react-hot-loader', 'babel-loader'],
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
