## ECS powered prototype of an autobattle between two teams. 

<img width="1025" height="565" alt="image" src="https://github.com/user-attachments/assets/873090d1-46fe-4dfd-8333-f1bb0f925259" />

### ECS Logic.
- Powered by my own solution **[UniversalEntities](https://github.com/vikle/UniversalEntities)**

### Graphics.
- Powered by my own solution **[URPImageEffectsAdapter](https://github.com/vikle/URPImageEffectsAdapter)**

### Start of the battle and restart.
1. When the game starts, the player sees an empty game board and the "continue" button;
2. After clicking on "continue", the characters appear and the battle begins;
3. Round ends after die all characters on one team, and the "restart" button appears.

### Life and armor indicators for each character.
1. The health bar of the team on the bottom left are green, while those of opposing team (on the top right) are red;
2. The armor bar is white and located above the health bar;

### Character and weapon modifier system.
1. At start of the round, each character is randomly assigned 3 modifiers and 2 modifiers for their weapons;
2. Modifiers can change:
	* For characters: Accuracy, Dexterity, MaxHealth, MaxArmor, AimTime;
	* For weapons: Accuracy, FireRate, ClipSize, ReloadTime;
3. Modifier parameters are applied to character prefabs and passed to the ECS;
 
 <details>
  <summary>[click] Details</summary>

<img width="345" height="243" alt="image" src="https://github.com/user-attachments/assets/a7f8b4ad-b608-4ed6-acab-965f44b480e0" />
<img width="345" height="260" alt="image" src="https://github.com/user-attachments/assets/40c754b7-2a20-4da4-ab99-958bbc773d2b" />

</details>

### Interaction between UI and ECS.
1. A lightweight model with function delegates is created for each UI controller;
2. When the UI is initialized, methods are bound to delegates and called from ECS;
3. Model instances are stored in DI;
4. Inspired by MVVM, but without Binders;

### Own lightweight DI and its role.
1. It serves to pass dependencies for closed ECS systems;
2. DI can be accessed from anywhere;
3. It can create instances of classes;
4. It can store single instances Mono Behaviour and Scriptable Object;
