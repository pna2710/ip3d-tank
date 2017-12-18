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
        Vector3 direcao;
        public Vector3 position = new Vector3(30, 5, 40);
        float acelaracao = 20f;
        BasicEffect effect;
        Matrix worldMatrix;
        BoundingSphere EsfBala;
        Vector3 gravidade = new Vector3(0, -9.8f, 0);

        public Bullet(ContentManager content, GraphicsDevice device, Vector3 initialPosition, Vector3 rotation)
        {
            effect = new BasicEffect(device);
            effect.VertexColorEnabled = true;
            effect.TextureEnabled = false;
            effect.LightingEnabled = false;
            bullet = content.Load<Model>("bala");
            worldMatrix = Matrix.Identity;
            EsfBala = new BoundingSphere(position, 1.0f);

            velocidade = direcao * acelaracao;
        }

        
        public void Update(GameTime gt, Terreno terreno, ClsTank tank)
        {
            KeyboardState key = Keyboard.GetState();

            PrevPos = position; // Posicao antiga. Necessária para o calculo da colisao

            velocidade += (gravidade * (float)gt.ElapsedGameTime.TotalSeconds);
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
            foreach (BasicEffect effect1 in bullet.Root.Meshes[0].Effects)
            {
                effect1.World = worldMatrix;
                effect1.View = camera.viewMatrix;
                effect1.Projection = camera.projectionMatrix;
                effect1.EnableDefaultLighting();

            }
        }
    }
}
