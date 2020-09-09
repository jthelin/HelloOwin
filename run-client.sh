#!/usr/bin/env bash

BUILD_CONFIGURATION=${1:-"Debug"}

mono "HelloOwinClient/bin/${BUILD_CONFIGURATION}"/*/HelloOwinClient.exe
