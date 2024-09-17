#!/bin/sh

cd MineStarter/

dotnet build -c release

if tmux has-session -t 'MineStarter' 2> /dev/null; then
    tmux kill-session -t MineStarter
fi

tmux new-session -d -s MineStarter

tmux send-keys -t MineStarter 'dotnet bin/release/net8.0/MineStarter.dll' Enter

