module PlatformerActor

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Content
open PlatformerAnimation

type BodyType =
    | Static
    | Dynamic of Vector2

type PlayerState =
    | Nothing
    | Jumping

type ActorType =
    | Player of PlayerState
    | Obstacle

type WorldActor =
    {
        ActorType : ActorType;
        Position : Vector2;
        Size : Vector2;
        Animation : Animation option;
        BodyType : BodyType
    }
    member this.CurrentBounds
        with get () = Rectangle((int this.Position.X),(int this.Position.Y),(int this.Size.X),(int this.Size.Y))

    member this.DesiredBounds
        with get () = let desiredPos = match this.BodyType with
                                       | Dynamic(s) -> this.Position + s
                                       | _-> this.Position
                      Rectangle((int desiredPos.X), (int desiredPos.Y), (int this.Size.X), (int this.Size.Y))

let CreateActor (content:ContentManager) (textureName, actorType, position, size, isStatic) =
    let tex = if not (System.String.IsNullOrEmpty textureName) then
                  let tex = content.Load<Texture2D>(textureName)
                  let anim = CreateAnimation tex 100
                  Some(anim)
              else
                  None
    let bt = if isStatic then
                Static
             else
                Dynamic(Vector2(0.f,0.f))
    { ActorType = actorType; Position = position; Size = size; Animation = tex; BodyType = bt; }

let UpdateActorAnimation gameTime (actor:WorldActor) =
    let animation = if actor.Animation.IsSome then
                        Some(UpdateAnimation gameTime actor.Animation.Value)
                    else None
    { actor with Animation = animation }