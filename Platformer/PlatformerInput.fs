module PlatformerInput

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input
open PlatformerActor

let HandleInput (kbState:KeyboardState) actor =
    let rec HandleKeys keys (currentVelocity:Vector2,state) =
        match keys with
        | [] -> currentVelocity
        | x :: xs -> match x with
                     | Keys.Left -> let newSpeed = if (currentVelocity.X - 0.1f) < -1.f then
                                                       -1.f
                                                   else
                                                       currentVelocity.X - 0.1f
                                    let newV = Vector2(newSpeed, currentVelocity.Y)
                                    HandleKeys xs (newV,state)
                     | Keys.Right -> let newSpeed = if (currentVelocity.X + 0.1f) > 1.f then
                                                       1.f
                                                    else
                                                       currentVelocity.X + 0.1f
                                     let newV = Vector2(newSpeed, currentVelocity.Y)
                                     HandleKeys xs (newV,state)
                     | Keys.Space -> match state with
                                     | Nothing -> let newV = Vector2(currentVelocity.X, currentVelocity.Y - 3.f)
                                                  HandleKeys xs (newV, Jumping)
                                     | Jumping -> HandleKeys xs (currentVelocity,state)
                     | _ -> HandleKeys xs (currentVelocity,state)
    match actor.ActorType with
    | Player(s) -> let initialVelocity = match actor.BodyType with
                                         | Dynamic(v) -> v
                                         | _ -> Vector2()
                   let velocity = HandleKeys (kbState.GetPressedKeys() |> Array.toList) (initialVelocity, s)
                   { actor with BodyType = Dynamic(velocity); ActorType = Player(Jumping) }
    | _ -> actor
