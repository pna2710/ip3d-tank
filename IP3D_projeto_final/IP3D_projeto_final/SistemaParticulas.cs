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
        #region Variables
        Vector3 centro;
        float raio;
        Vector3 posicaoInicial;

        Random num;
        List<Particula> Chuvas;
        Vector3 direcao;

        float aspectoRatio;
        BasicEffect effect;
        Matrix worldMatrix;
        Vector3 gravidade = new Vector3(0, -9.8f, 0);// vector gravidade que é exercido em todos as particulas
        #endregion

        public SistemaParticulas(GraphicsDevice device)
        {
            num = new Random();
            Chuvas = new List<Particula>();

            centro = new Vector3(62f, 100f, 62f);//"Nuvem"
            direcao = Vector3.Down;// Direcao da particula
            raio = 3f;// tamanho da "Nuvem"(area onde existe particulas)

            effect = new BasicEffect(device);
            worldMatrix = Matrix.Identity;


            effect.LightingEnabled = false;
            effect.VertexColorEnabled = true;
        }

        //adiciona particulas a lista chuva e define-lhes a possição inicial. 
        public void adicionarParticulas()
        {
            for (int i = 0; i < 8; i++)
            {

                double rNumber1 = num.NextDouble();
                double rNumber2 = num.NextDouble();

                posicaoInicial.X = (float)(centro.X + raio * rNumber1);
                posicaoInicial.Z = (float)(centro.Z + raio * rNumber2);
                posicaoInicial.Y = centro.Y - 50;

                //direcao = Vector3.Down;
                direcao.Normalize();

                Chuvas.Add(new Particula(posicaoInicial, direcao));
            }
        }

        //A função update é onde se adiciona a particula e da-se valor as propriedades da particula e atuacilza a velocidade e a posição.
        public void Update(GameTime gameTime)
        {
            adicionarParticulas();

            foreach (Particula particula in Chuvas)
            {
                particula.velocidade += gravidade * (float)gameTime.ElapsedGameTime.TotalSeconds * 0.3f;
                particula.posicao += (particula.velocidade * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            foreach (Particula particula in Chuvas)
            {
                particula.verticesP[0] = new VertexPositionColor(particula.posicao, Color.White);
                particula.verticesP[1] = new VertexPositionColor(particula.posicao + particula.velocidade * 0.03f, Color.White);
            }
            ApagaParticulas();
        }        public void ApagaParticulas()// apaga as particulas que estao abaixo do plano
        {
            for (int i = Chuvas.Count - 1; i >= 0; i--)
            {
                if (Chuvas[i].posicao.Y < 48.72f)
                {
                    Chuvas.RemoveAt(i);
                }
            }
        }


        public void Draw(GraphicsDevice device)
        {
            Console.WriteLine(Chuvas.Count);
            foreach (Particula part in Chuvas)
            {
                Vector3 dir = new Vector3(64, -50, 64);
                dir.Normalize();
                aspectoRatio = (device.Viewport.Width / device.Viewport.Height);

                effect.World = worldMatrix;
                effect.View = Matrix.CreateLookAt(new Vector3(64, 50, 64), dir, Vector3.Up);
                effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), aspectoRatio, 0.1f, 1000f);
                effect.CurrentTechnique.Passes[0].Apply();
                device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, part.verticesP, 0, 1);
            }
        }
    }
}