module PlatformerPhysics

open Microsoft.Xna.Framework
open PlatformerActor

let AddGravity (gameTime:GameTime) actor =
    let ms = gameTime.ElapsedGameTime.TotalMilliseconds
    let g = ms * 0.005
    match actor.BodyType with
    | Dynamic(s) -> let d = Vector2(s.X, s.Y + (float32 g))
                    { actor with BodyType = Dynamic(d); }
    | _ -> actor

let ResolveVelocities actor =
    match actor.BodyType with
    | Dynamic (s) -> { actor with Position = actor.Position + s }
    | _ -> actor

