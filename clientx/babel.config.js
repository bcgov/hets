/* eslint-env node */

module.exports = function(api) {
  api.cache(true);

  const presets = [
    ['@babel/preset-env', {modules: 'commonjs'}],
    '@babel/preset-react',
  ];

  const plugins = [
    ['@babel/plugin-proposal-class-properties', { 'loose': true, 'useBuiltIns': true }],
    ['@babel/plugin-proposal-object-rest-spread', { 'loose': true, 'useBuiltIns': true }],
    'react-hot-loader/babel',
  ];

  return {
    presets,
    plugins,
    env: {
      test: {
        presets: [
          ['@babel/preset-env', {modules: false}],
        ],
      },
      production: {
        plugins: ['transform-react-remove-prop-types'],
      },
    },
  };
};
