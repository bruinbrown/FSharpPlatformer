module PlatformerGame

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open PlatformerActor
open PlatformerAnimation
open PlatformerInput
open PlatformerPhysics

type Game1 () as x =
    inherit Game()

    do x.Content.RootDirectory <- "Content"
    #if INTERACTIVE
    do x.Content.RootDirectory <- __SOURCE_DIRECTORY__ + @"bin\Debug\Content"
    #endif
    let graphics = new GraphicsDeviceManager(x)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>

    let CreateActor' = CreateActor x.Content

    let mutable WorldObjects = lazy ([("animtest.png", Player(Nothing), Vector2(10.f,28.f), Vector2(32.f,32.f), false);
                                      ("obstacle.png", Obstacle, Vector2(10.f,60.f), Vector2(32.f,32.f), true);
                                      ("", Obstacle, Vector2(42.f,60.f), Vector2(32.f,32.f), true);]
                                     |> List.map CreateActor')

    let DrawActor (sb:SpriteBatch) actor =
        if actor.Animation.IsSome then 
            do DrawAnimation sb actor.Animation.Value actor.Position
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
        let HandleInput' = HandleInput (Keyboard.GetState ())
        let UpdateActorAnimation' = UpdateActorAnimation gameTime
        let current = WorldObjects.Value
        do WorldObjects <- lazy (current
                                 |> List.map HandleInput'
                                 |> List.map AddGravity'
                                 |> List.map AddFriction
                                 |> HandleCollisions
                                 |> List.map ResolveVelocities
                                 |> List.map UpdateActorAnimation')
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