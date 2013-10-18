module PlatformerAnimation

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

let FrameWidth = 32
let FrameHeight = 32

type Animation =
    {
        TextureStrip : Texture2D;
        FrameCount : int;
        CurrentFrame : int;
        CurrentTime : int;
        TimePerFrame : int;
    }

let CreateAnimation (texture:Texture2D) frameLength =
    let frameCount = texture.Width / FrameWidth
    { TextureStrip = texture; FrameCount = frameCount; CurrentFrame = 0; CurrentTime = 0; TimePerFrame = frameLength }

let UpdateAnimation (gameTime:GameTime) animation =
    let time = animation.CurrentTime + (int gameTime.ElapsedGameTime.TotalMilliseconds)
    let newFrame = if time > animation.TimePerFrame then
                        let newFrame' = animation.CurrentFrame + 1
                        if newFrame' >= animation.FrameCount then
                            0
                        else newFrame'
                    else
                        animation.CurrentFrame
    let counter = if time > animation.TimePerFrame then 0
                  else time
    { animation with CurrentFrame = newFrame; CurrentTime = counter; }

let DrawAnimation (spriteBatch:SpriteBatch) animation (position:Vector2) =
    let rect = System.Nullable(Rectangle(animation.CurrentFrame * FrameWidth, 0, FrameWidth, FrameHeight))
    spriteBatch.Draw(animation.TextureStrip, position, rect, Color.White)