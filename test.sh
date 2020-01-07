#!/usr/bin/env bash

BUILD_CONFIGURATION=${1:-"Debug"}

mono ~/.nuget/packages/xunit.runner.console/*/tools/net46/xunit.console.exe \
    HelloOwinTests/bin/${BUILD_CONFIGURATION}/net46/HelloOwinTests.dll \
    -xml TestResults.xml
