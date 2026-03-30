#!/bin/bash
UNITY="C:/Program Files/Unity/Hub/Editor/2022.3.62f3/Editor/Unity.exe"
PROJECT="$(cd "$(dirname "$0")" && pwd -W)"
LOG="e:/tmp/unity-build.log"

echo "Compiling: $PROJECT"
"$UNITY" -projectPath "$PROJECT" -batchmode -quit -logFile "$LOG"
EXIT_CODE=$?

ERRORS=$(grep -iE "error CS[0-9]" "$LOG" 2>/dev/null)
if [ -n "$ERRORS" ]; then
  echo "COMPILE ERRORS:"
  echo "$ERRORS"
  exit 1
elif [ "$EXIT_CODE" -ne 0 ]; then
  echo "Unity exited with code $EXIT_CODE — see $LOG"
  exit 1
else
  echo "OK — no errors"
fi
