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
        public Vector3 direcao, position;
        float acelaracao = 20f;
        BasicEffect effect;
        Matrix worldMatrix;
        BoundingSphere EsfBala;
        Vector3 gravidade = new Vector3(0, -9.8f, 0);

        public Bullet(GraphicsDevice device, ContentManager content, Vector3 initialPosition, Vector3 direcao)
        {
            bullet = content.Load<Model>("bala");
            effect = new BasicEffect(device);
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.LightingEnabled = true;
            worldMatrix = Matrix.Identity;
            
            EsfBala = new BoundingSphere(initialPosition, 1.0f);

            position = initialPosition;
            velocidade = direcao * acelaracao;
        }

        
        public void Update(GameTime gt)
        {
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
              foreach (BasicEffect effect in bullet.Root.Meshes[0].Effects)
            {
                effect.World = worldMatrix;
                effect.View = camera.viewMatrix;
                effect.Projection = camera.projectionMatrix;
                effect.EnableDefaultLighting();

            }
                  return false;
          }*/

        public void Draw(GraphicsDevice device, Camera camera)
        {
            worldMatrix = Matrix.CreateScale(0.10f) * Matrix.CreateTranslation(position);
            // Draw the model.
            foreach (BasicEffect effect in bullet.Meshes[0].Effects)
            {
                effect.World = worldMatrix;
                effect.View = camera.viewMatrix;
                effect.Projection = camera.projectionMatrix;
                effect.EnableDefaultLighting();

            }


        }
    }
}
