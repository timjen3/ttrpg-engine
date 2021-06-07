# About

This project handles die rolls and sequences of die rolls

## Concepts

### Die

Equations: How the result of this die is calculated

### Die Sequences

A sequence is an ordered list of Die where each Die has a Condition for being rolled

After being rolled, the result of a die is available to the subsequent die as a variable named ResultName for the rolled die

#### Roll Conditions

Each die in the sequence must have a condition for whether or not it should be rolled.

The condition consists of an equation that will be evaluated where a value >= 1 will mean it should be rolled, and otherwise it should not.

When a condition fails for a die in the sequence a RollConditionFailedException is thrown

## Todo

#### 1. Inject attributes into custom functions

Entity attributes should be injectable into custom functions. In V1 of this application it was done by a preliminary string.replace where variables inside functions must be inside of curly braces. Ex: [dice:{mymin},{mymax}] would be converted into [dice:1,6] prior to the custom function being performed. On the one hand it would be nice if this was done inside the DiceEngine as part of the die roll. On the other hand, it requires the Dice Engine to pull in complexity it shouldn't need. Maybe there is a way to redesign that can reduce the complexity and allow it to be in this project.

#### 2. Failure types for roll conditions

When a condition fails it can mean at least 2 different things. One is that the user should never have been able to roll the die; it's an invalid roll. The other is that the roll is valid, but some check failed and so part of the sequence will not be performed. For instance, if a dodge succeeds then the take damage die should not be performed.
