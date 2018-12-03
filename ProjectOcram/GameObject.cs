using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOcram
{
    public class GameObject
    {
        Texture2D texture;
        Vector2 Position;


        public Rectangle CollisionBox

        {

            get

            {

                return new Rectangle(

                    (int)Position.X,

                    (int)Position.Y,

                    texture.Width,

                    texture.Height);

            }
        }

            public GameObject(Texture2D texture, Vector2 position)

        {

            this.texture = texture;

            this.Position = position;

        }

 

        public GameObject(Texture2D texture, Vector2 position, Vector2 velocity)

        {

            this.texture = texture;

            this.Position = position;

            

        }


        public void Draw(SpriteBatch spriteBatch)

        {

            spriteBatch.Draw(texture, Position, Color.White);

        }
        }
    }
