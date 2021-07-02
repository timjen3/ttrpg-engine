[![Build & Tests](https://github.com/timjen3/ttrpg-engine/actions/workflows/dotnet.yml/badge.svg)](https://github.com/timjen3/ttrpg-engine/actions/workflows/dotnet.yml)
 
 # About

Use this project to create ttrpg or crpg algorithms.

On Nuget: https://www.nuget.org/packages/TTRPG.Engine

# Demo

The easiest way to learn how this package works is to review the demo. It will illustrate the concepts below in action.

# Concepts

## Sequence Items (ISequenceItems)

Little more than a container for a plaintext algorithm to be parsed by the mxParser math expression parser library.

See mxParser for tutorials: http://mathparser.org/

#### ISequenceItems

Sequence Items declare a `ResultName`. After being resolved the result is injected into the inputs collection for use by following sequence items, conditions, and mappings. Use these to chain sequence item algorithms or to inform actions.

There are two types of sequence items included in the package as differentiated by the SequenceItemType enum.

1. Algorithm: As the name implies this is an algorithm to be processed through mxParser. All variables are usable, but be aware that the order of the item matters when items use the results from other items. For instance `(damage + damage_modifier) / 2`.

2. Message: The Equation property contains a format string to be populated with variables. For instance `{damage} damage was dealt to the target.`

3. *Other*: Custom Sequence Items can be created by implementing the ISequenceItem interface.

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

Conditions can specify dependencies for SequenceItems. If a dependenct SequenceItem is not resolved (due to a failed condition) then the condition will fail.

If the dependencies were processed and the condition has an Equation it will be evaluated. The Equation can contain the ResultNames from previously processed SequenceItes as variables.

Example: `toHit > dodge`

#### Mappings

Mappings can be added for items in the sequence. Mappings add aliases to input variable names (or result names from previous SequenceItems) for a specified item.

Mappings without a specific item specified will be applied to all items in the sequence.

#### Roles

Roles have attributes that can be used within equations. To inject role attributes into equations you must add a RoleMapping. Like regular Mappings, Role Mappings can apply to specific items or to all items.

## Exceptions

    ConditionFailedException
    CustomFunctionArgumentException
    EquationInputArgumentException
    MappingFailedException
    MissingRoleException
    UnknownCustomFunctionException
