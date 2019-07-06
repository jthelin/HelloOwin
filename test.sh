#!/usr/bin/env bash

BUILD_CONFIGURATION=${1:-"Debug"}

mono packages/xunit.runner.console.*/tools/net46/xunit.console.exe \
    HelloOwinTests/bin/${BUILD_CONFIGURATION}/HelloOwinTests.dll \
    -xml TestResults.xml
