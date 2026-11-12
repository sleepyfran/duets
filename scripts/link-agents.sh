#!/bin/sh
# Links the .agents folder to the folder a given coding agent expects.
set -eu

root="$(cd "$(dirname "$0")/.." && pwd)"
source_dir=".agents"

if [ ! -d "$root/$source_dir" ]; then
  echo "Could not find $source_dir in $root" >&2
  exit 1
fi

echo "Which agent do you want to link $source_dir for?"
echo "  1) Claude Code"
printf "Choice [1]: "
read -r choice

case "${choice:-1}" in
  1 | claude | "Claude Code")
    target_dir=".claude"
    ;;
  *)
    echo "Unknown agent: $choice" >&2
    exit 1
    ;;
esac

target="$root/$target_dir"

if [ -L "$target" ]; then
  if [ "$(readlink "$target")" = "$source_dir" ]; then
    echo "$target_dir already links to $source_dir, nothing to do."
    exit 0
  fi
  echo "$target_dir is a symlink to a different location, remove it and run again." >&2
  exit 1
fi

if [ -e "$target" ]; then
  echo "$target_dir already exists and is not a symlink, move it and run again." >&2
  exit 1
fi

ln -s "$source_dir" "$target"
echo "Linked $target_dir -> $source_dir"
