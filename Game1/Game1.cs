﻿using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private Texture2D _blank;

        private Texture2D _texture;
        private Effect _effect;
        private bool _enabled = true;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Components.Add(new FrameRateCounter(this));
            Components.Add(new Input(this));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _texture = Content.Load<Texture2D>("EagleIsland");
            _font = Content.Load<SpriteFont>("font");
            _blank = new Texture2D(GraphicsDevice, 1, 1);
            _blank.SetData(new[] {Color.White.PackedValue});

            _graphics.PreferredBackBufferWidth = _texture.Width;
            _graphics.PreferredBackBufferHeight = _texture.Height;
            _graphics.ApplyChanges();

            _effect = Content.Load<Effect>("crt-lottes-mg");
            _effect.Parameters["brightboost"].SetValue(0.92f);

            var texSize = new Vector2(_texture.Width, _texture.Height);
            _effect.Parameters["textureSize"]?.SetValue(texSize);
            _effect.Parameters["videoSize"]?.SetValue(texSize);
            var outSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _effect.Parameters["outputSize"]?.SetValue(outSize);

            var vp = GraphicsDevice.Viewport;
            var output = new RenderTarget2D(GraphicsDevice, vp.Width, vp.Height);
            GraphicsDevice.SetRenderTarget(output);

            _spriteBatch.Begin(effect: _effect);
            _spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            using (var fs = File.OpenWrite("../../../../../img.png"))
                output.SaveAsPng(fs, output.Width, output.Height);
            output.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Input.IsDown(Keys.Escape))
                Exit();
            if (Input.IsPressed(Keys.T))
                _enabled = !_enabled;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(effect: _enabled ? _effect : null);
            _spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
