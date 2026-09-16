#!/bin/sh
mkdir -p /app/data
exec dotnet FnBook.API.dll
