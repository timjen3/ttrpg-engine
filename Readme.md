﻿[![Build & Tests](https://github.com/timjen3/ttrpg-engine/actions/workflows/dotnet.yml/badge.svg)](https://github.com/timjen3/ttrpg-engine/actions/workflows/dotnet.yml)
 
 [![NuGet version (TTRPG.Engine)](https://img.shields.io/nuget/v/TTRPG.Engine.svg?style=flat)](https://www.nuget.org/packages/TTRPG.Engine/)
 
 # About

Use TTRPG.Engine to create ttrpg or crpg algorithms. Algorithms are referred to as "sequences" in this library. Each step of the sequence is called a "sequence item".

The easiest way to learn how this package works is to review the demo.

# Concepts

## Strings all the way down.

The philosophy of this project is that it should be possible to build out a game engine entirely with text. This will facilitate the serialization of algorithms into a data store. Consequently there should be no reason to do conversions or math in code since sequences can be used for that.

## Sequence Items (ISequenceItems)

Little more than a container for a plaintext algorithm to be parsed by the mxParser math expression parser library.

See mxParser for tutorials: http://mathparser.org/

Sequence Items declare a `ResultName`. After being resolved the result is injected into the inputs collection for use by following sequence items, conditions, and mappings. Use these to chain sequence item algorithms or to inform actions.

There are two types of sequence items differentiated by the SequenceItemType enum.

1. Algorithm: As the name implies this is an algorithm to be processed through mxParser. All variables are usable, but be aware that the order of the item matters when items use the results from other items. For instance `(damage + damage_modifier) / 2`.

2. Message: The Equation property contains a format string to be populated with variables. For instance `{damage} damage was dealt to the target.`

#### Custom Functions

When using the AddTTRPGEngineServices ServiceCollection extension method, several custom functions will be available to equations for ease of use. This is a list of available custom functions:

    random(n,minRange,maxRange) : get sum of n random numbers all having values between minRange and maxRange.
    toss(0) : returns 0 or 1 at random; parameter does nothing (mxParser requires >=1 param for user defined functions)

Variables from inputs/roles can be injected into custom functions.

    random(1,minRange,maxRange)

To add additional user defined functions simply add singletons of type org.mariuszgromada.math.mxparser.Function to the service collection.

## Sequences

Sequences are currently the way to model an event with the library. They contain a list of sequence items that are resolved in the configured order when `Process()` is called.

#### Conditions 

Conditions can be added for items in the sequence. Conditions that fail can either result in the whole sequence failing or steps of the sequence being skipped.

Conditions can specify dependencies for SequenceItems. Equations are not processed unless the dependencies are fulfilled since equations may rely on the results from previously processed SequenceItes.

Example: `toHit > dodge`

##### Sequence Conditions

When a condition does not specify sequence items to apply to it will be considered a Sequence-level condition. When a Sequence is checked these conditions will be evaluated to determine if the sequence is suitable to be processed with the provided parameters.

#### Mappings

Mappings can be added for items in the sequence. Mappings add aliases to input variable names (or result names from previous SequenceItems) for a specified item.

Mappings without a specific item specified will be applied to all items in the sequence.

#### Roles

Roles have attributes that can be used within equations. To inject role attributes into equations you must add a Mapping with a Role property set. Setting the Role property will make the mapping source the role with the specified name.

## Exceptions

    ConditionFailedException
    CustomFunctionArgumentException
    EquationInputArgumentException
    MappingFailedException
    MissingRoleException
    UnknownCustomFunctionException
