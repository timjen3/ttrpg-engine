[![Build & Tests](https://github.com/timjen3/ttrpg-engine/actions/workflows/dotnet.yml/badge.svg)](https://github.com/timjen3/ttrpg-engine/actions/workflows/dotnet.yml)
[![NuGet version (TTRPG.Engine)](https://img.shields.io/nuget/v/TTRPG.Engine.svg?style=flat)](https://www.nuget.org/packages/TTRPG.Engine/)
 
 # About

Use TTRPG.Engine to create ttrpg or crpg algorithms. Algorithms are referred to as "sequences" in this library. Each step of the sequence is called a "sequence item".

The easiest way to learn how this package works is to review the demo.

#### Strings all the way down

The philosophy of this project is that it should be possible to build out a game engine entirely with text. This will facilitate the serialization of algorithms into a data store. Consequently there should be no reason to do conversions or math in code since sequences can be used for that.

# Concepts

## Sequences

Sequences contain a list of sequence items that are resolved in the configured order. They also contain various interstitial parts.

### Sequence Items

Each item in a sequence resolves a single calculation, the result of which is available to subsequent items. The result can be referenced by subsequent items using the `ResultName`.

After being resolved the result is injected into the inputs collection for use by following sequence items, conditions, and mappings.

A single sequence item can be processed with the equation service without being part of a sequence along with 0 or 1 Entity, and 0 or 1 input collection. Since there is no mapping, all of the entity's attributes will be available for equations.

#### Custom Functions

When using the AddTTRPGEngineServices ServiceCollection extension method, several custom functions will be available to equations for ease of use. This is a list of available custom functions:

    rnd(n,minRange,maxRange) : get sum of n random numbers all having values between minRange and maxRange.
    toss() : returns 0 or 1 at random
    roundx(n,digits) : rounds number to the specified number of digits
    also see list of built in functions: [Jace](https://github.com/pieterderycke/Jace/blob/master/Jace/CalculationEngine.cs#L388)

Variables from inputs/entities can be injected into custom functions.

    rnd(1,minRange,maxRange)

todo: #113

### Conditions 

Example: `toHit > dodge`

Conditions can be added for items in the sequence. Conditions that fail can either result in the whole sequence failing or steps of the sequence being skipped.

If a sequence item relies on the results from a previous sequence item it should declare a dependency on that item. That will prevent it from running without the pre-requisite.

##### Sequence Conditions

When a condition does not specify sequence items to apply to it will be considered a Sequence-level condition. When a Sequence is checked these conditions will be evaluated to determine if the sequence is suitable to be processed with the provided parameters.

### Entity Conditions

Sequences may define 0+ requirements for entities. These are always applied to the sequence as a whole. Trying to process a sequence with invalid entities will result in a EntityConditionFailedException being thrown.

### Mappings

Mappings can be added for items in the sequence. Mappings add aliases to input variable names (or result names from previous SequenceItems) for a specified item.

Mappings without a specific item specified will be applied to all items in the sequence.

From and To inside a mapping support replacement characters. ex: `"from": "{rename_me}"`

### Entities

Entities have attributes that can be used within equations. To inject entity attributes into equations you must add a Mapping with a Entity property set. Setting the Entity property will make the mapping source the entity with the specified name.

Derived Attributes are attributes that are themselves formulas. They might be derived from other attributes, a random number (ie: "rnd(1,1,6)"), or any other calculation. These are supported with some caveats. The attribute key must be surrounded by [[key]]. Equations that use these derived attributes must reference them in the same manner, ie: [[attribute]].

Entities can have 0+ categories for organizational purposes.

Sequences require entities to be aliased according to their purpose in the sequence. To alias an entity just clone it with the required name. This will create a deep copy with the alias set accordingly.

When no EntityName is specified for an Entity mapping the first entity passed will be chosen.

## TTRPGEngine

The engine has built in parsers for processing different kinds of commands.

### Inventory Commands

todo

### Equation Commands

todo

### Registering Messages

todo

## Exceptions

    ConditionFailedException
    CustomFunctionArgumentException
    EquationInputArgumentException
    MappingFailedException
    MissingEntityException
    UnknownCustomFunctionException
