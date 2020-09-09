#!/usr/bin/env bash

BUILD_CONFIGURATION=${1:-"Debug"}

mono "HelloOwinServer/bin/${BUILD_CONFIGURATION}"/*/HelloOwinServer.exe
