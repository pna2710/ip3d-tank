using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3D_projeto_final
{
    class Bullet
    {
        Model bullet;
        Vector3 velocidade = Vector3.Zero, PrevPos = new Vector3(30, 5, 40);
        public Vector3 position = new Vector3(30, 5, 40);
        float acelaracao = 20f;
        BasicEffect effect;
        Matrix worldMatrix;
        public bool IsMove = false;
        BoundingSphere EsfBala;
        Vector3 GRAVIDADE = new Vector3(0, -9.8f, 0);
        List<Bullet> balas;

        public Bullet(ContentManager content, GraphicsDevice device, Terreno terreno)
        {
            effect = new BasicEffect(device);
            effect.VertexColorEnabled = true;
            effect.TextureEnabled = false;
            effect.LightingEnabled = false;
            bullet = content.Load<Model>("bala");
            IsMove = false;
            worldMatrix = Matrix.Identity;
            EsfBala = new BoundingSphere(position, 1.0f);
        }

        public void addbullet(Bullet bullet, GameTime c)
        {
            float i = c.ElapsedGameTime.Seconds;
            if(i > 5)
            {
                balas.Add(bullet);
            }
            else
            {
                Console.WriteLine("no bullet");
            }
        }

        public void Init(Vector3 direcaoB) // Recebe a direcao do canhao e a posicao do tanque
        {
            EsfBala.Center = position;
            EsfBala.Radius = 1.0f;

            velocidade = direcaoB * acelaracao;
            IsMove = true;
        }

        public void Update(GameTime gt, Terreno terreno, ClsTank tank)
        {
            KeyboardState key = Keyboard.GetState();

            PrevPos = position; // Posicao antiga. Necessária para o calculo da colisao

            velocidade += (GRAVIDADE * (float)gt.ElapsedGameTime.TotalSeconds);
            position += (velocidade * (float)gt.ElapsedGameTime.TotalSeconds);

            EsfBala.Center = position;
        }

        /*  public bool Intersect(ClsTank tank)//para colisao
          {
              float dist = Vector3.Distance(tank.SphereTank.Center, EsfBala.Center);
              if (dist < 3)
                  return true;
              else
                  return false;
          }*/

        public void Draw(GraphicsDevice device, Camera camera)
        {
            worldMatrix = Matrix.CreateScale(0.10f) * Matrix.CreateTranslation(position);
            // Draw the model.
            foreach (Bullet b in balas)
            {
                foreach (BasicEffect effect1 in b.bullet.Root.Meshes[0].Effects)
                {
                    effect1.World = worldMatrix;
                    effect1.View = camera.viewMatrix;
                    effect1.Projection = camera.projectionMatrix;
                    effect1.EnableDefaultLighting();
                }
                b.bullet.Root.Meshes[0].Draw();
            }
        }
    }
}
