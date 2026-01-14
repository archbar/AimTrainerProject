/*
 * GM - Contains references to canvas, score, and self as a singleton
 * 
 * HC - Displays Health, Updates health,
 * 
 * TargetManager - change target position when target below certain health threshold, changes how many targets in scene according to scenario time
 * 
 * TargetBehaviour - Establish base target movement (stationary) and initialisation (scoreWorth) as protected virtual void MoveTarget(), protected virtual void InitialiseTarget() - generically establishes score worth
 *  - needs a way to tell target manager its dead and therefore to update their position and the score probs OnCollisionEnter/Stay
 * - negative target, sets score as negative value
 * 
 * MovingTargetBehaviour - Start() { InitialiseTarget() } overriden - check position in world, set an a and b point (a and b's is the same as spawn y,
 * a's x is the minimum x we can reach while b's x is the maximum, a and b's z is also the same as target z)
 * determine if move left or right, determine speed,
 * Update() { MoveTarget() } overriden - lerp from a to b until distance value is less than amount then lerp from b to a, do at speed, in the direction
 * 
 * AdvancedMovingTargetBehaviour - Start() { InitialiseTarget() } overridden - call base initialisation but also change the y and the z
 * Update() { MoveTarget() } overriden - call base MoveTarget but also increment or decrement speed every now and then, destroy self if lifetime value below 0,
 * change the direction every now and then to be opposite (maybe randomise distance value, or change it over time)
 * 
 * 
 * 
 * 
 * FPSGC - Operates Gun System
 * 
 * FPSPC - Operates player
 * 
 * FPSCC - Operates camera
 * 
 * BB - Operates Bullet
 * 
 * 
 * 
 */