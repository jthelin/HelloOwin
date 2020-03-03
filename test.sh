#!/usr/bin/env bash

BUILD_CONFIGURATION=${1:-"Debug"}

mono ~/.nuget/packages/xunit.runner.console/*/tools/net47/xunit.console.exe \
    HelloOwinTests/bin/${BUILD_CONFIGURATION}/*/HelloOwinTests.dll \
    -xml TestResults.xml
