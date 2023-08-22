# CorgiServerAuthoritativeMovement
Integrates PredictionReconcilliationNetwork(https://github.com/TCleard/PredictionReconciliationNetwork), Corgi Engine (https://corgi-engine.moremountains.com/), and Netcode for GameObjects to provide server authoritative movement

Current state: horizontal movement and jumps are syncrhonized correctly. Button inputs take advantage of the Corgi Engine Button class ButtonDownRecently method to look for the ButtonDown state. This is necessary because we only run the Corgi update loop manually "OnTick" when the processessing method gathers and applies input, and the jump button's CurrentState was never ButtonDown (required to initiate jump) when we checked it directly.
Other abilities have not yet been tested and presumably will not work. Generally, they will require new "Network" derivatives of the classes to replace Time.deltaTime with our network deltaTime, and/or similar button handling to the CharacterJump ability.

Sample folder includes scene and player prefab that demonstrates how to configure a player object. Requires Unity 2020.3 or later, Netcode for GameObjects, Corgi Engine, and the PredictionReconcilliationNetwork dll--HOWEVER, this currently requires a change that has not been merged yet. So currently you would need to build the dll from my fork at: https://github.com/angusmf/PredictionReconciliationNetwork/
