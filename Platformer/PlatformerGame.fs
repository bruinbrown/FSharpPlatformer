module PlatformerGame

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open PlatformerActor
open PlatformerPhysics

type Game1 () as x =
    inherit Game()

    do x.Content.RootDirectory <- "Content"
    let graphics = new GraphicsDeviceManager(x)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>

    let CreateActor' = CreateActor x.Content

    let mutable WorldObjects = lazy ([("player.png", Player(Nothing), Vector2(10.f,28.f), Vector2(32.f,32.f), false);
                                      ("obstacle.png", Obstacle, Vector2(10.f,60.f), Vector2(32.f,32.f), true);
                                      ("", Obstacle, Vector2(42.f,60.f), Vector2(32.f,32.f), true);]
                                     |> List.map CreateActor')

    let DrawActor (sb:SpriteBatch) actor =
        if actor.Texture.IsSome then 
            do sb.Draw(actor.Texture.Value, actor.Position, Color.White)
        ()

    override x.Initialize() =
        do spriteBatch <- new SpriteBatch(x.GraphicsDevice)
        do base.Initialize()
        ()

    override x.LoadContent() =
        do WorldObjects.Force () |> ignore
        ()

    override x.Update (gameTime) =
        let AddGravity' = AddGravity gameTime
        let current = WorldObjects.Value
        do WorldObjects <- lazy (current
                                 |> List.map AddGravity'
                                 |> HandleCollisions
                                 |> List.map ResolveVelocities)
        do WorldObjects.Force () |> ignore
        ()

    override x.Draw (gameTime) =
        do x.GraphicsDevice.Clear Color.CornflowerBlue
        let DrawActor' = DrawActor spriteBatch
        do spriteBatch.Begin ()
        WorldObjects.Value
        |> List.iter DrawActor'
        do spriteBatch.End ()
        ()