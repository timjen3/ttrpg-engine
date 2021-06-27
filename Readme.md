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

#### Concrete Implementations of ISequenceItems

There are several types of sequence items included in the package:

1. Die Sequence Items: These declare a `ResultName`. After being resolved the result will be injected into the inputs collection for use by following sequence items, conditions, and mappings. Use these to chain sequence item algorithms or to inform actions.

2. Data Sequence Items: These are generics that carry a data payload that can be used by a consuming application.

3. Message Sequence Items: Produce custom messages.

4. *Other*: Custom Sequence Items can be created by implementing the ISequenceItem interface or BaseSequenceItem abstract class.

#### Custom Functions

When using the AddTTRPGEngineServices ServiceCollection extension method, several custom functions will be available to equations for ease of use. This is a list of available custom functions:

    random(n,minRange,maxRange) : get sum of n random numbers all having values between minRange and maxRange.
    toss(0) : returns 0 or 1 at random; parameter does nothing (mxParser requires >=1 param for user defined functions)

Variables from inputs/roles can be injected into custom functions.

    random(1,minRange,maxRange)

To add additional user defined functions simply add singletons of type org.mariuszgromada.math.mxparser.Function to the service collection.

## Sequences

Sequences pull everything together. They contain sequence items that are resolved in the configured order when Process is called.

#### Conditions 

Conditions can be added for items in the sequence. Conditions that fail can either result in the whole sequence failing or steps of the sequence being skipped.

#### Mappings

Mappings can be added for items in the sequence. Mappings add aliases to input variable names (or result names from previous DieSequenceItems) for a specified item. These makes an item's equations as clear as possible without coupling it to a specific sequence.

Mappings without a specific item specified will be applied to all items in the sequence.

#### Roles

Roles contain attributes that can be used within equations. To inject role attributes into equations you must add a RoleMapping. Role Mappings can apply to specific items, or to all items.

## Exceptions

    ConditionFailedException
    CustomFunctionArgumentException
    EquationInputArgumentException
    MappingFailedException
    MissingRoleException
    UnknownCustomFunctionException
