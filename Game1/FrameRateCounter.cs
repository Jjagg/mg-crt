// Adapted from the following blog post by Shawn Hargreaves.
// https://blogs.msdn.microsoft.com/shawnhar/2007/06/08/displaying-the-framerate/

using System;
using Game1.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class FrameRateCounter : DrawableGameComponent
    {
        private readonly ContentManager _content;
        private SpriteBatch _spriteBatch;

        private int _frameRate;
        private int _frameCounter;
        private TimeSpan _elapsedTime = TimeSpan.Zero;

        public Vector2 Position = new Vector2(5, 5);
        private SpriteFont _font;
        private int _fontLineSpacing;

        public SpriteFont Font
        {
            get { return _font; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _font = value;
                _fontLineSpacing = Font.LineSpacing;
            }
        }

        /// <summary>
        /// Create a new frame counter for the given game.
        /// </summary>
        /// <param name="game"></param>
        public FrameRateCounter(Game game) : base(game)
        {
            var resourceManager = Resources.ResourceManager;
            _content = new ResourceContentManager(game.Services, resourceManager);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Font = _content.Load<SpriteFont>("fpsFont");
        }

        protected override void UnloadContent()
        {
            _content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;

            if (_elapsedTime > TimeSpan.FromSeconds(1))
            {
                _elapsedTime -= TimeSpan.FromSeconds(1);
                _frameRate = _frameCounter;
                _frameCounter = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _frameCounter++;

            var fps = $"fps: {_frameRate}";
            var spf = $"spf: {(_frameRate == 0 ? 1 : 1000f/_frameRate).ToString("n4")}";

            _spriteBatch.Begin();

            _spriteBatch.DrawString(Font, fps, Position, Color.Black);
            _spriteBatch.DrawString(Font, fps, Position - Vector2.One, Color.White);

            _spriteBatch.DrawString(Font, spf, Position + _fontLineSpacing * Vector2.UnitY, Color.Black);
            _spriteBatch.DrawString(Font, spf, Position - Vector2.One + _fontLineSpacing * Vector2.UnitY, Color.White);

            _spriteBatch.End();
        }
    }
}