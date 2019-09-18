
# reap

  Monitor and unlink old files from the specified directories.

## Installation

```
$ npm install -g reap
```

## Usage

```

  Usage: reap [options] <dirs>

  Options:

    -h, --help            output usage information
    -t, --threshold <ms>  file unlink threshold [1 hour]
    -s, --single          run a single time and exit

```

  Output example:

```

$ reap /tmp --threshold 30m &

  ...
  rm /tmp/BTServer_stderr.log 6.55kb
  rm /tmp/BTServer_stdout.log 1.42kb
  rm /tmp/c0099727b6166cb763872993127be6ba 15b
  rm /tmp/c1a7c9fe6bb077b1579b359e84d58c00 15b
  rm /tmp/c2d9781e513984f9670795a8201144ca 15b
  rm /tmp/c5ee5e1c81927f5f64b4f5d97ea6514b 13b
  rm /tmp/c61c471fdb1c81f867096dc46195cc03 13b
  rm /tmp/c73bace4c4e22f20ae62d7710766bf89 13b
  rm /tmp/d80572662454aede91a682ae3090e12a 4b
  rm /tmp/dbbc22df43f249a5f81bb073f47d2672 15b
  rm /tmp/dc5ca69e6f5bd14c07c6f2de10175e2a 9b
  rm /tmp/e5b3698ff787fd0dabd2796925ffc7b3 15b
  rm /tmp/ef4541776a569003280fb2787c36ee06 9b
  rm /tmp/efc7bd0b9e43ecb7edb257bb40eb8236 15b
  rm /tmp/f66de9d15c4c7fe21d3e265eb2988888 15b
  rm /tmp/mocha 5b
  rm /tmp/mocha-glob.txt 169b
  rm /tmp/stack-logs.21793.BTServer.IrA1vy.index 63.88kb

  74 files
  72.77kb total

````

## License

  MIT
