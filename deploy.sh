#!/bin/bash

rsync -r --delete $TRAVIS_BUILD_DIR/publish/ travis@rancor.mortimer.nu:/var/travis/runit-api/$TRAVIS_BUILD_ID
ssh -t travis@rancor.mortimer.nu "rm /var/travis/runit-api/current && ln -s /var/travis/runit-api/$TRAVIS_BUILD_ID /var/travis/runit-api/current"