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

Sequence Items declare a `ResultName`. After being resolved the result is injected into the inputs collection for use by following sequence items, conditions, and mappings.

Equations work in different ways depending on the value of SequenceItemEquationType:

1. Algorithm: The Equation property contains a formula to be ran through mxParser. All variables are usable, but be aware that the order of the item matters when items use the results from other items. For instance `(damage + damage_modifier) / 2`. See mxParser for tutorials: http://mathparser.org/

2. Message: The Equation property contains a format string to be populated with variables. For instance `{damage} damage was dealt to the target.`

A single sequence item can be processed with the equation service without being part of a sequence along with 0 or 1 Role, and 0 or 1 input collection. Since there is no mapping, all of the role's attributes will be available for equations.

#### Custom Functions

When using the AddTTRPGEngineServices ServiceCollection extension method, several custom functions will be available to equations for ease of use. This is a list of available custom functions:

    random(n,minRange,maxRange) : get sum of n random numbers all having values between minRange and maxRange.
    toss(0) : returns 0 or 1 at random; parameter does nothing (mxParser requires >=1 param for user defined functions)

Variables from inputs/roles can be injected into custom functions.

    random(1,minRange,maxRange)

To add additional user defined functions simply add singletons of type org.mariuszgromada.math.mxparser.Function to the service collection.

### Conditions 

Example: `toHit > dodge`

Conditions can be added for items in the sequence. Conditions that fail can either result in the whole sequence failing or steps of the sequence being skipped.

If a sequence item relies on the results from a previous sequence item it should declare a dependency on that item. That will prevent it from running without the pre-requisite.

##### Sequence Conditions

When a condition does not specify sequence items to apply to it will be considered a Sequence-level condition. When a Sequence is checked these conditions will be evaluated to determine if the sequence is suitable to be processed with the provided parameters.

### Role Conditions

Sequences may define 0+ requirements for roles. These are always applied to the sequence as a whole. Trying to process a sequence with invalid roles will result in a RoleConditionFailedException being thrown.

### Mappings

Mappings can be added for items in the sequence. Mappings add aliases to input variable names (or result names from previous SequenceItems) for a specified item.

Mappings without a specific item specified will be applied to all items in the sequence.

### Roles

Roles have attributes that can be used within equations. To inject role attributes into equations you must add a Mapping with a Role property set. Setting the Role property will make the mapping source the role with the specified name.

Roles can have 0+ categories for organizational purposes.

Sequences require roles to be aliased according to their purpose in the sequence. To alias a role just clone it with the required name. This will create a deep copy with the alias set accordingly.

When no RoleName is specified for a Role mapping the first role passed will be chosen.

### ResultItems

While the results of all processed SequenceItems are accessible in the SequenceResult object, it is easier to work with results through the ResultItems pattern.

If a RoleName is set the passed role with that Alias will be attached.

If FirstRole is true, then the first role passed will be attached.

A FormatMessage can be set which can be a plain string (ex: `"something"`) or a format string (ex: `"{propa}"`). Format strings will have access to all inputs, all results, and any mappings that do not have ItemNames specified.

## Exceptions

    ConditionFailedException
    CustomFunctionArgumentException
    EquationInputArgumentException
    MappingFailedException
    MissingRoleException
    UnknownCustomFunctionException
