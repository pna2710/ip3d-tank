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
        #region Variables
        Model bullet;
        Vector3 up, forward, right;
        Matrix rotationMatrix;

        Vector3 velocidade = Vector3.Zero, PrevPos = new Vector3(30, 5, 40);
        public Vector3 position;
        float acelaracao = 20f;
        BasicEffect effect;
        Matrix worldMatrix;
        BoundingSphere esfBala;
        Vector3 gravidade = new Vector3(0, -9.8f, 0);
        #endregion

        //Para verificar colisão com o chão
        Terreno terreno;

        public Bullet(GraphicsDevice device, ContentManager content, Vector3 initialPosition, Vector3 direcao, Vector3 normal, Terreno terreno)
        {
            bullet = content.Load<Model>("bala");
            effect = new BasicEffect(device);
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.LightingEnabled = true;
            worldMatrix = Matrix.Identity;
            
            esfBala = new BoundingSphere(bullet.Meshes[0].BoundingSphere.Center, bullet.Meshes[0].BoundingSphere.Radius);

            position = initialPosition;
            velocidade = -direcao * acelaracao;
            forward = -direcao;
            up = normal;
            right = Vector3.Cross(up, forward);

            rotationMatrix.Forward = forward;
            rotationMatrix.Up = up;
            rotationMatrix.Right = right;

            this.terreno = terreno;


        }

        
        public void Update(GameTime time, ClsTank tank)
        {

            PrevPos = position; // Posicao antiga. Necessária para o calculo da colisao

            velocidade += (gravidade * (float)time.ElapsedGameTime.TotalSeconds);
            position += (velocidade * (float)time.ElapsedGameTime.TotalSeconds);

            esfBala.Center = position;

            if (VerifyIntersectTank(tank) || VerifyIntersectTerrain())
            {
                bullet = null;
            }
            
            if (esfBala.Center.Z >= 126 || esfBala.Center.Z <= 1 || esfBala.Center.X >= 126 || esfBala.Center.X <= 1)
            {
                bullet = null;
            }
            
        }

        public bool VerifyIntersectTank(ClsTank tank)//para colisao
        {
            if (esfBala.Intersects(tank.Sphere))
                return true;
          
            return false;
        }

        public bool VerifyIntersectTerrain()//para colisao
        {
            if (!(esfBala.Center.Z >= 126 || esfBala.Center.Z <= 1 || esfBala.Center.X >= 126 || esfBala.Center.X <= 1))
            {
                if (esfBala.Center.Y <= terreno.alturasdata[(int)esfBala.Center.X, (int)esfBala.Center.Z].Y)
                    return true;

                return false;
            }
            return true;
            
        }

        public void Draw(GraphicsDevice device, Camera camera)
        {
            worldMatrix = Matrix.CreateScale(0.10f) * Matrix.CreateTranslation(position);
            // Draw the model.
            if (bullet != null)
            {
                foreach (ModelMesh mesh in bullet.Meshes)
                {
                    foreach (BasicEffect effect in bullet.Meshes[0].Effects)
                    {
                        effect.World = worldMatrix;
                        effect.View = camera.viewMatrix;
                        effect.Projection = camera.projectionMatrix;
                        effect.EnableDefaultLighting();

                    }
                    mesh.Draw();
                }

            }

        }
    }
}
