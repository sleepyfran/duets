# State
This assembly holds the state of the game as described in the Entity `State`. It uses an agent
to do so and can accept `Effect`s that are raised from the Simulation layer to do modifications
on the state and thus completely decoupling the Simulation layer from storage and also making
it completely stateless and more easily testable.