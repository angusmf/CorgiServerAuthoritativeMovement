# CorgiServerAuthoritativeMovement
Integrates PredictionReconcilliationNetwork(https://github.com/TCleard/PredictionReconciliationNetwork), Corgi Engine (https://corgi-engine.moremountains.com/), and Netcode for GameObjects to provide server authoritative movement

Current state: movement is synchronized seeminly correctly, but button input is not captured correctly. A feature like CharacterJump which waits for the ButtonDown event will never fire. This is due to the input being queried once per server tick instead of on Update.

Sample folder includes scene and player prefab that demonstrates how to configure a player object.
