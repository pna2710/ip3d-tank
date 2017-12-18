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
        Vector3 gravidade = new Vector3(0, 9.8f, 0);// vector gravidade que é exercido em todos as particulas

        public void Zona(GraphicsDevice device)
        {
            num = new Random();
            Po = new List<Particula>();

            effect = new BasicEffect(device);
            worldMatrix = Matrix.Identity;


            effect.LightingEnabled = false;
            effect.VertexColorEnabled = true;
        }

        public void Iniciar(ClsTank tank)
        {
            centro = tank.positionTank;
        }


        public void adicionarParticulas()
        {
            for (int i = 0; i < 10; i++)
            {

                double rNumber1 = num.NextDouble();
                double rNumber2 = num.NextDouble();

                posicaoInicial.X = (float)(centro.X + raio * rNumber1);
                posicaoInicial.Z = (float)(centro.Z + raio * rNumber2);

                direcao.Normalize();

                Po.Add(new Particula(posicaoInicial, direcao));
            }
        }

        public void Update(GameTime gameTime, Terreno terra)
        {
            tempo += (float)gameTime.ElapsedGameTime.TotalSeconds;
            adicionarParticulas();

            foreach (Particula particula in Po)
            {
                particula.velocidade += gravidade * (float)gameTime.ElapsedGameTime.TotalSeconds * 0.3f;
                particula.posicao += (particula.velocidade * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            foreach (Particula particula in Po)
            {
                particula.verticesP[0] = new VertexPositionColor(particula.posicao, Color.Brown);
                particula.verticesP[1] = new VertexPositionColor(particula.posicao + particula.velocidade * 0.05f, Color.Brown);
            }
            //ApagaParticulas(terra);


        }

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
            foreach (Particula part in Po)
            {
                effect.World = worldMatrix;
                effect.View = camera.viewMatrix;
                effect.Projection = camera.projectionMatrix;
                effect.CurrentTechnique.Passes[0].Apply();
                effect.EnableDefaultLighting();

                device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, part.verticesP, 0, 1);
            }
        }


    }
}
