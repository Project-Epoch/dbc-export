#!/bin/bash

START_DIR=$(pwd)
BINARIES_DIR=$START_DIR/bin/Release/net5.0
APP_DIR=$START_DIR/built/app

rm -rf $START_DIR/bin
rm -rf $APP_DIR

dotnet build --configuration Release

mkdir -p built/app

mv $BINARIES_DIR/dbc-export.exe $APP_DIR
mv $BINARIES_DIR/dbc-export.dll $APP_DIR
mv $BINARIES_DIR/dbc-export.runtimeconfig.json $APP_DIR
mv $BINARIES_DIR/Microsoft.Extensions.* $APP_DIR
mv $BINARIES_DIR/MySqlConnector.dll $APP_DIR
mv $BINARIES_DIR/Newtonsoft.Json.dll $APP_DIR

cp $START_DIR/definitions.json $APP_DIR
cp $START_DIR/appsettings.json.dist $APP_DIR

rm -rf $START_DIR/bin