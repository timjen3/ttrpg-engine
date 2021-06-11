[![Build & Tests](https://github.com/timjen3/dice-engine/actions/workflows/dotnet.yml/badge.svg)](https://github.com/timjen3/dice-engine/actions/workflows/dotnet.yml)
 
 # About

This project handles die rolls and sequences of die rolls. "Die Roll" is a bit of an artificial flavoring in this case, as the die can be complex algorithms. Ultimately it is intended to serve as a component in ttrpg style applications.

# Concepts

## Die

Little more than a container for a plaintext algorithm to be parsed by the mxParser math expression parser library.

See mxParser for tutorials: http://mathparser.org/

For ease of use a custom function feature has been added. The custom function must be enclosed in square brackets. The function will be evaluated and replaced with the result prior to running the function through mxParser. This is a list of available custom functions:

    [Dice:n,s] : roll n die with s sides.

Inputs can be injected into custom functions. To do so, wrap your input variable in curly braces. Inputs that have been mapped in the sequence are available through this method. Some keywords cannot be used through this method because they are reserved by the SmartFormat library.

    [Dice:{minRoll},{maxRoll}]

## Die Sequences

Die Sequences are orchestrators of multiple die. The die are rolled in the configured order, with the result of earlier die available to algorithms in later die.

#### Conditions 

Conditions can be added for Die in the sequence. Conditions that fail can either result in the whole sequence failing or steps of the sequence being skipped.

#### Mappings

Mappings can be added for die in the sequence. Mappings add aliases to input variable names (or result names from previous die) for a specified die. These makes a die's equations as clear as possible without coupling it to a specific sequence.

# Todo

#### 1. Actions

One or more actions can be added to a die sequence. Action have conditions just like die rolls. While it is limiting, for simplicity all actions are ran after all die rolls.

#### 2. More robust use of dependency injection

Add IServiceCollection extensions helper. All classes should be injected into the service collection instead of new'd up. It should be possible to inject ICustomFunctions.

#### 3. Create demo project

Add a demo project winform that allows you to create a dice sequence, conditions, and mappings. Load from json text.

#### 4. Control constants via configuration

Currently the mxParser constants are always removed because it is can cause unexpected results due to the existence of "c" and other constants. This should be controllable by configuration instead in case someone wants access to these kinds of things. Additionally, a way to specify global constants could be very useful.

#### 5. Die-Sequence-Roles & Entities

Die Sequences will declare 0+ roles. In order to be rolled, a die sequence will require entities to be passed in to fill each role. An entity has a set of attributes. Equations can reference the roles with some special syntax (for instance maybe: `@entity1:health@`). Mappings are performed between the entity attributes and the die inputs. For instance, input defines hp but entity defines health. The mapping will inject entity1 with an extra attribute named hp.