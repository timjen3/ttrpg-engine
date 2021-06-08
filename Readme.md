[![Build & Tests](https://github.com/timjen3/dice-engine/actions/workflows/dotnet.yml/badge.svg)](https://github.com/timjen3/dice-engine/actions/workflows/dotnet.yml)
 
 # About

This project handles die rolls and sequences of die rolls. "Die Roll" is a bit of an artificial flavoring in this case, as the die can be complex algorithms. Ultimately it is intended to serve as a component in ttrpg style applications.

## Concepts

### Die

Little more than a container for a plaintext algorithm to be parsed by the mxParser math expression parser library.

See mxParser for tutorials: http://mathparser.org/

For ease of use a custom function feature has been added. The custom function must be enclosed in square brackets. The function will be evaluated and replaced with the result prior to running the function through mxParser. This is a list of available custom functions:

    [Dice:n,s] : roll n die with s sides.

### Die Sequences

Die Sequences are orchestrators of multiple die. The die are rolled in the configured order, with the result of earlier die available to algorithms in later die.

Conditions can be added for Die in the sequence. Conditions that fail can either result in the whole sequence failing or steps of the sequence being skipped.

Mappings can be added for die in the sequence. Mappings add aliases to input variable names (or result names from previous die) for a specified die. These makes a die's equations as clear as possible without coupling it to a specific sequence.

## Todo

#### 1. Inject input variables into custom functions

Custom functions should be able to be called with variables from the input dictionary.

#### 2. Control constants via configuration

Currently the mxParser constants are always removed because it is can cause unexpected results due to the existence of "c" and other constants. This should be controllable by configuration instead in case someone wants access to these kinds of things. Additionally, a way to specify global constants could be very useful.
