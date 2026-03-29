#!/bin/bash
UNITY="C:/Program Files/Unity/Hub/Editor/2022.3.62f3/Editor/Unity.exe"
PROJECT="$(cd "$(dirname "$0")" && pwd)"
LOG="e:/tmp/unity-build.log"

case "$1" in
  compile)
    echo "Compiling: $PROJECT"
    "$UNITY" -projectPath "$PROJECT" -batchmode -quit -logFile "$LOG"
    if grep -q "error CS" "$LOG" 2>/dev/null; then
      echo "COMPILE ERRORS:"
      grep "error CS" "$LOG"
      exit 1
    else
      echo "OK — no errors"
    fi
    ;;
  *)
    echo "Opening Unity Editor: $PROJECT"
    "$UNITY" -projectPath "$PROJECT" &
    ;;
esac
