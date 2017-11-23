# Developer Guide
1. [Introduction](#introduction)
2. [Setting Up](#setting-up)
    * 2.1 Prerequisites
    * 2.2 Importing the Project into Unity
3. [Design](#design)

## 1. Introduction
This game is a physics-based platformer that utilises a grappling hook to traverse the level and to solve puzzles.

## 2. Setting up
Guide for setting up this project on your computer

### 2.1 Prerequisites

1. **Unity `2017.2.0f3`**<br>
   > May not work with earlier or older versions of Unity
2. Visual Studio Community (Install along with Unity)

### 2.2 Importing the Project into Unity

1. Fork this repo, and clone the fork to your computer
2. Open Unity 2017.2.0f3
3. Click `Open`
5. Locate the project's directory & highlight it
6. Click `Select Folder`
7. On main page, select GrappleHookPlatformer 

## 3. Prefabs
A brief overview of the important prefabs that make up the game

### 3.1 GrapplePlayer Prefab
- Consists of 2 Colliders one Circle2D & one Box2D
- Uses Rigidbody2D physics for movement
- Animation Controller: "Astronaut"
- Tagged "Player" on Layer "Player"
- DistanceJoint2D Component simulates the grapple rope
- LineRenderer Component displays the grapple rope
- Scripts attached:
> Player.cs
> RopeSystem.cs
> PlayerMovement.cs


## 4. Scripts
A brief overview of the important scripts that make up the game

### 4.1 Player.cs Script
- Manages Player Health & Invinciblity Frames
- Kills Player if they fall below certain point

### 4.2 RopeSystem.cs Script
- Handles grapple hook firing from player input
- Handles grapple rope wrapping / unwrapping around objects which are tagged "GrappleTerrain" and contains a PolygonCollider2D
- Handles grapple rope with objects tagged "GrappleObject" allowing them to be moved using the physics engine
- Handles rendering of the grapple rope
- Manages length of rope when player moves up and down on it


### 4.3 PlayerMovement.cs Script
- Handles Player Horizontal & Vertical Movement on ground
- Handles Player Horizontal & Vertical Movement when swinging
- Checks if player is actually touching the ground using a circlecast with a layermask
