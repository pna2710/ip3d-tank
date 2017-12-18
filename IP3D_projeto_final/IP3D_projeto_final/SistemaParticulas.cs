using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3D_projeto_final
{
    class SistemaParticulas
    {
        Vector3 centro;
        int raio = 6;
        Vector3 posicaoInicial;

        Random num;
        List<Particula> Po;
        Vector3 direcao = Vector3.Up;

        float tempo = 0f;

        public bool po = false;
        BasicEffect effect;
        Matrix worldMatrix;
        Vector3 gravidade = new Vector3(0, -9.8f, 0);// vector gravidade que é exercido em todos as particulas

        public SistemaParticulas(GraphicsDevice device)
        {
            num = new Random();
            Po = new List<Particula>();

            effect = new BasicEffect(device);
            worldMatrix = Matrix.Identity;


            effect.LightingEnabled = false;
            effect.VertexColorEnabled = true;
        }

        public void Iniciar(Vector3 pos)
        {
            centro = pos;
        }


        public void adicionarParticulas(ClsTank tank)
        {
            for (int i = 0; i < 5; i++)
            {

                double rNumber1 = num.NextDouble();
                double rNumber2 = num.NextDouble();

                posicaoInicial.X = (float)(tank.positionTank.X + raio * rNumber1);
                posicaoInicial.Z = (float)(tank.positionTank.Z + raio * rNumber2);

                direcao.Normalize();

                Po.Add(new Particula(posicaoInicial, direcao));
            }
        }

        public void Update(GameTime gameTime, ClsTank tank)
        {
            tempo += (float)gameTime.ElapsedGameTime.TotalSeconds;
            adicionarParticulas(tank);

            foreach (Particula particula in Po)
            {
                particula.velocidade += gravidade * (float)gameTime.ElapsedGameTime.TotalSeconds * 0.5f;
                particula.posicao += (particula.velocidade * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            foreach (Particula particula in Po)
            {
                particula.verticesP[0] = new VertexPositionColor(particula.posicao, Color.Brown);
                particula.verticesP[1] = new VertexPositionColor(particula.posicao + particula.velocidade * 0.05f, Color.Brown);
            }
           


        }
        //ApagaParticulas
        public void ApagaParticulas(Camera camera, Vector3[,] alturasdatas)// apaga as particulas que estao abaixo do plano, ou seja y<0
        {
            for (int i = Po.Count - 1; i >= 0; i--)
            {
                if (Po[i].posicao.Y > (camera.SurfaceFollow(centro, alturasdatas) + 5f))
                {
                    Po.RemoveAt(i);
                }
            }
        }


        public void Draw(GraphicsDevice device, Camera camera)
        {
            Console.WriteLine(Po.Count);
            foreach (Particula particula in Po)
            {
                Vector3 dir = new Vector3(64, -50, 64);
                dir.Normalize();

                effect.World = worldMatrix;
                effect.View = Matrix.CreateLookAt(new Vector3(64, 50, 64), dir, Vector3.Up);
                effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), (device.Viewport.Width / device.Viewport.Height), 0.1f, 1000f);
                effect.CurrentTechnique.Passes[0].Apply();

                device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, particula.verticesP, 0, 1);
            }
        }


    }
}
