/*eslint-env node*/

const path = require('path');
const webpack = require('webpack');
const _ = require('lodash');

const IS_PRODUCTION = process.env.NODE_ENV === 'production';

// Include your SmUserId on the command line.
const DEV_USER = process.env.HETS_DEV_USER || '';

var webpackPlugins = [
  new webpack.optimize.CommonsChunkPlugin({ name: 'vendor', filename: 'vendor.js' }),
  new webpack.LoaderOptionsPlugin({
    options: {
      eslint: {
        failOnWarning: IS_PRODUCTION,
        failOnError: true,
      },
    },
  }),
];

if(IS_PRODUCTION) {
  webpackPlugins.push(new webpack.optimize.UglifyJsPlugin({
    compress: {
      warnings: false,
    },
    mangle: {
      except: [ '$super', '$', 'exports', 'require' ],
    },
  }));
  // TODO: See if this is necessary
  webpackPlugins.push(new webpack.DefinePlugin({
    'process.env':{
      'NODE_ENV': JSON.stringify('production'),
    },
  }));
  webpackPlugins.push(new webpack.NoEmitOnErrorsPlugin());
} else {
  webpackPlugins.push(new webpack.HotModuleReplacementPlugin());
  webpackPlugins.push(new webpack.DefinePlugin({
    'process.env':{
      'NODE_ENV': JSON.stringify('development'),
      'DEV_USER': JSON.stringify(DEV_USER),
    },
  }));
}

module.exports = {
  bail: IS_PRODUCTION,
  devtool: IS_PRODUCTION ? 'source-map' : 'eval',
  entry: {
    app: _.compact([
      IS_PRODUCTION ? null : 'webpack-hot-middleware/client',
      './src/js/init.js',
    ]),
    vendor: [
      'jquery',
      'bluebird',
      'lodash',
      'react',
      'react-bootstrap',
      'react-bootstrap-datetimepicker',
      'react-dom',
      'react-redux',
      'react-router',
      'react-router-bootstrap',
      'redux',
    ],
  },
  output: {
    path: path.join(__dirname, '/dist/'),
    filename: 'app.js',
    publicPath: '/',
  },
  plugins: webpackPlugins,
  module: {
    rules: [
      {
        enforce: 'pre',
        test: /\.jsx?$/,
        loader: 'eslint-loader',
        exclude: /node_modules/,
        options: { emitError: !IS_PRODUCTION, failOnError: IS_PRODUCTION },
      },
      {
        test: /\.jsx?$/,
        exclude: /node_modules/,
        loaders: ['react-hot-loader', 'babel-loader'],
      },
    ],
  },
};
