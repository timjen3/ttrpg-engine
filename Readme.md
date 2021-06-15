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

    [Dice:n,s] : roll n die with s sides.

Inputs can be injected into custom functions. To do so, wrap your input variable in curly braces. Inputs that have been mapped in the sequence are available through this method.

    [Dice:{minRoll},{maxRoll}]

## Sequences

Sequences are orchestrators of complexity. The items are resolved in the configured order.

#### Conditions 

Conditions can be added for items in the sequence. Conditions that fail can either result in the whole sequence failing or steps of the sequence being skipped.

#### Mappings

Mappings can be added for items in the sequence. Mappings add aliases to input variable names (or result names from previous DieSequenceItems) for a specified item. These makes an item's equations as clear as possible without coupling it to a specific sequence.

# Todo

#### 1. Control constants via configuration

Currently the mxParser constants are always removed because it is can cause unexpected results due to the existence of "c" and other constants. This should be controllable by configuration instead in case someone wants access to these kinds of things. Additionally, a way to specify global constants could be very useful.

#### 2. Create demo project

Add a demo project winform that creates a sequence from json text and allows you to roll it.

#### 3. Sequence-Roles & Entities

Sequences will declare 0+ roles. In order to be rolled, a sequence will require entities to be passed in to fill each role. An entity has a set of attributes. Equations can reference the roles with some special syntax (for instance maybe: `@entity1:health@`). Mappings are performed between the entity attributes and the die inputs. For instance, input defines hp but entity defines health. The mapping will inject entity1 with an extra attribute named hp.

#### 4. Create nuget package and rename to AlgorithmSequencer (something like that)

This package can be made more generic and added on the nuget feed. Create a new project called DiceEngine that consumes this project and adds the Dice custom function (and others). Reference that project (and others) in this readme as examples of how to use the package.
