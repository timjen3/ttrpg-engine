[![Build & Tests](https://github.com/timjen3/ttrpg-engine/actions/workflows/dotnet.yml/badge.svg)](https://github.com/timjen3/ttrpg-engine/actions/workflows/dotnet.yml)
[![NuGet version (TTRPG.Engine)](https://img.shields.io/nuget/v/TTRPG.Engine.svg?style=flat)](https://www.nuget.org/packages/TTRPG.Engine/)

# TTRPG Engine

This is a library intended to simplify and accelerate the implementation of algorithms and how they affect entities in a game.

The philosophy of the project is that you should be able to store all your game algorithms in _data_. Your game engine will interact with the algorithms through the `TTRPGEngine` class using text commands. 

Take a look at the demo project (a survival game) to see how this works. All the main capabilities of the engine have features implemented in the demo.

## Sequences

These are the underpinning of this library. A sequence is a set of algorithms that produce 1 or more events.

Currently there are two kinds of events that can be produced by sequences:
1. Messages: display some message
2. UpdateAttributes: update the attributes of one of the involved entities, typically using one of your sequence items results

## Automatic Commands

You can register commands to run automatically. There are 2 kinds of automatic commands:
1. AutomaticCommands: Runs after a sequence and all sub-sequent SequenceAutoCommands are processed.
2. SequenceAutoCommands: Run after sequences with the specified category.
