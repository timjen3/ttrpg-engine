[![Build & Tests](https://github.com/timjen3/ttrpg-engine/actions/workflows/dotnet.yml/badge.svg)](https://github.com/timjen3/ttrpg-engine/actions/workflows/dotnet.yml)
 
 # About

Use this project to create ttrpg or crpg algorithms.

On Nuget: https://www.nuget.org/packages/TTRPG.Engine

# Concepts

## Sequence Items

Little more than a container for a plaintext algorithm to be parsed by the mxParser math expression parser library.

See mxParser for tutorials: http://mathparser.org/

#### Sub-Types

1. Die Sequence Items: These declare a `ResultName`. After being resolved the result will be injected into the inputs collection for use by following sequence items, conditions, and mappings. Use these to chain sequence item algorithms or to inform actions.

2. Data Sequence Items: These are generics that carry a data payload that can be used by a consuming application.

3. *Other*: Custom Sequence Items can be created by implementing the ISequenceItem interface or BaseSequenceItem abstract class.

#### Custom Functions

For ease of use several custom functions can be used inside of sequence item equations (for any items implementing the BaseSequenceItem abstract class). Custom functions are denoted by square brackets. The function will be evaluated and replaced (from square bracket to square bracket) with the result prior to running the function through mxParser. This is a list of available custom functions:

    [random:n,minRange,maxRange] : get sum of n random numbers all having values between minRange and maxRange.

Inputs can be injected into custom functions. To do so, wrap your input variable in curly braces. Inputs that have been mapped in the sequence are available through this method.

    [random:1,{minRange},{maxRange}]

## Sequences

Sequences pull everything together. They contain sequence items that are resolved in the configured order when Process is called.

#### Conditions 

Conditions can be added for items in the sequence. Conditions that fail can either result in the whole sequence failing or steps of the sequence being skipped.

#### Mappings

Mappings can be added for items in the sequence. Mappings add aliases to input variable names (or result names from previous DieSequenceItems) for a specified item. These makes an item's equations as clear as possible without coupling it to a specific sequence.

Mappings without a specific item specified will be applied to all items in the sequence.

#### Roles

Roles contain attributes that can be used within equations. To inject role attributes into equations you must add a RoleMapping.

## Exceptions

    ConditionFailedException
    CustomFunctionArgumentException
    EquationInputArgumentException
    MappingFailedException
    MissingRoleException
    UnknownCustomFunctionException
