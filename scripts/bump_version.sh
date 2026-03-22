#!/usr/bin/env bash
# Reads the highest semver git tag, bumps the patch component,
# creates and pushes the new tag, then prints the new version string.
# Git credentials must already be configured in the environment.

set -euo pipefail

latest=$(git tag --sort=-version:refname \
  | grep -E '^v?[0-9]+\.[0-9]+\.[0-9]+$' \
  | head -1)

if [[ -z "$latest" ]]; then
  major=0; minor=0; patch=0
else
  version="${latest#v}"
  IFS='.' read -r major minor patch <<< "$version"
fi

patch=$(( patch + 1 ))
new_version="${major}.${minor}.${patch}"

git tag "v${new_version}"
git push origin "v${new_version}"

echo "$new_version"
