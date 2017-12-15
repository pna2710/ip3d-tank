using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_Fase2
{
    public class Camera
    {
        // matrix for camera view and projection
        public Matrix projectionMatrix, viewMatrix, MatrixRotacao;
        BasicEffect effect;
        bool Cam1 = false, CamT1 = true, CamT2 = false;
        float yaw, pitch, aspectoRatio, scale = 0.5f, speed = 0.2f;

        // actual camera position, direction
        Vector3 position;
        Vector3 direction;

        public Camera(GraphicsDevice device)
        {
            scale = MathHelper.ToRadians(15) / 50;
            position = new Vector3(50f, 10f, 50f);
            aspectoRatio = (device.Viewport.Width / device.Viewport.Height);
            yaw = -0.5f;
            pitch = 0.4f;

            effect = new BasicEffect(device);

            effect.LightingEnabled = false;
            effect.VertexColorEnabled = true;

        }

        public void Update(GraphicsDevice device, Terreno terreno, ClsTank tank, ClsTank tankenemy)
        {
            MouseState rato = Mouse.GetState();
            Vector2 posRato, diferenca = new Vector2(0f, 0f);
             // centro do ecra
             Vector2 centro = new Vector2(device.Viewport.Width / 2, device.Viewport.Height / 2);
             // direction do exio Z
             direction = new Vector3(0.0f, 0.0f, 1f);
             // calcular a normal entre o vetor Up e o vetor direction, que da o vetor direction do eixos X
             Vector3 directionInX = Vector3.Cross(Vector3.Up, direction);
             posRato.X = rato.X;
             posRato.Y = rato.Y;
             // diferença entre o centro e a nova posiçao do rato
             diferenca = posRato - centro;
             // atraves da diferença conseguimos o yaw e o pitch
             yaw -= diferenca.X * scale;
             
             pitch += diferenca.Y * scale;
            
             
            
             //atualizar
             MatrixRotacao = Matrix.CreateFromYawPitchRoll(yaw, pitch, 0);
             direction = Vector3.Transform(direction, MatrixRotacao);
             directionInX = Vector3.Transform(directionInX, MatrixRotacao);


            if (this.position.Z >= 126)
            {
                position.Z = 126;
            }
            if (this.position.Z <= 1)
            {
                position.Z = 1;
            }
            if (this.position.X >= 126)
            {
                position.X = 126;
            }
            if (this.position.X <= 1)
            {
                position.X = 1;
            }
            


            // MOVIMENTAR COM WASD
            KeyboardState key = Keyboard.GetState();
            //corrida com shift
            if (key.IsKeyDown(Keys.LeftShift))
                speed = 0.5f;
            else speed = 0.2f;
            //WASD
            if (key.IsKeyDown(Keys.I))
                position += direction * speed;
            if (key.IsKeyDown(Keys.K))
                position -= direction * speed;
            if (key.IsKeyDown(Keys.J))
                position += directionInX * speed;
            if (key.IsKeyDown(Keys.L))
                position -= directionInX * speed;
            // Alterar cameras
            if (key.IsKeyDown(Keys.F1))
            {
                Cam1 = false;
                CamT1 = true;
                CamT2 = false;
            }
            if (key.IsKeyDown(Keys.F2))
            {
                CamT1 = false;
                Cam1 = true;
                CamT2 = false;
            }
            if(key.IsKeyDown(Keys.F3))
            {
                CamT1 = false;
                Cam1 = false;
                CamT2 = true;
            }
            // atualizar alturas com o surface follow
            if (Cam1)
            {
                // atualizar alturas com o surface follow
                position.Y = SurfaceFollow(position, terreno.alturasdata);
                Console.WriteLine("altura: " + SurfaceFollow(position, terreno.alturasdata));
                //move camera to new position
                viewMatrix = Matrix.CreateLookAt(position, (position + direction), Vector3.Up);
                projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), aspectoRatio, 0.1f, 1000f);
            }
            // seguir o tank 1
            else if (CamT1)
            {
                position = tank.TankFollow(tank.positionTank, tank.direction, terreno);
                viewMatrix = Matrix.CreateLookAt(position, (tank.positionTank + direction), Vector3.Up);
            }
            else if (CamT2)
            {
                position = tankenemy.TankFollow(tankenemy.positionTank, tankenemy.direction, terreno);
                viewMatrix = Matrix.CreateLookAt(position, (tankenemy.positionTank + direction), Vector3.Up);
            }



            //move camera to new position
            viewMatrix = Matrix.CreateLookAt(position, (position + direction), Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), aspectoRatio, 0.1f, 1000f);

            
        }

        #region SurfaceFollow
        public float SurfaceFollow(Vector3 pos, Vector3[,] alturasdata)
        {
            float altura12, altura34, altura;

            int x = (int)pos.X;
            int z = (int)pos.Z;

            float y1 = alturasdata[x, z].Y;
            float y2 = alturasdata[x+1, z].Y;
            float y3 = alturasdata[x, z+1].Y;
            float y4 = alturasdata[x+1, z+1].Y;

            float d1x = pos.X - x;
            float d2x = 1 - d1x;
            float d3x = d1x;
            float d4x = 1 - d3x;

            float d1z = pos.Z - z;
            float d3z = 1 - d1z;

            // interpolacao bilinear. Encontrar a altura(saber o valor)
            altura12 = (d2x * y1) + (d1x * y2);
            altura34 = (d4x * y3) + (d3x * y4);
            altura = altura12 * d3z + altura34 * d1z;

            return (altura + 2.0f);
        }
        #endregion
    }
}