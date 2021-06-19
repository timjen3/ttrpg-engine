[![Build & Tests](https://github.com/timjen3/dice-engine/actions/workflows/dotnet.yml/badge.svg)](https://github.com/timjen3/dice-engine/actions/workflows/dotnet.yml)
 
 # About

This project handles die rolls and sequences of die rolls. "Die Roll" is a bit of an artificial flavoring, as the die can be complex algorithms. Ultimately this library is intended to serve as a component in ttrpg style applications. But it is built to be generic and could be used for other applications.

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

Sequences are orchestrators of complexity. The items are resolved in the configured order.

#### Conditions 

Conditions can be added for items in the sequence. Conditions that fail can either result in the whole sequence failing or steps of the sequence being skipped.

#### Mappings

Mappings can be added for items in the sequence. Mappings add aliases to input variable names (or result names from previous DieSequenceItems) for a specified item. These makes an item's equations as clear as possible without coupling it to a specific sequence.

Mappings without an order specified will be applied to all Sequence Items.

#### Roles

Roles contain attributes that can be used within equations. To inject role attributes into equations you must add a RoleMapping.

# Todo

#### 1. Try out mxparser custom functions

If usable, it will eliminate a lot of redundant code in this repo

#### 1. Control constants via configuration

Currently the mxParser constants are always removed because it is can cause unexpected results due to the existence of "c" and other constants. This should be controllable by configuration instead in case someone wants access to these kinds of things. Additionally, a way to specify global constants could be very useful.

#### 2. Create nuget package and rename to AlgorithmSequencer (something like that)

This package can be made more generic and added on the nuget feed. Create a new project called DiceEngine that consumes this project and adds the Dice custom function (and others). Reference that project (and others) in this readme as examples of how to use the package.
