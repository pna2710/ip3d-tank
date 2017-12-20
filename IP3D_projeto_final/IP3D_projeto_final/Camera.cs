using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3D_projeto_final
{
    class Camera
    {
        // matrix for camera view and projection
        public Matrix projectionMatrix, viewMatrix, MatrixRotacao;
        BasicEffect effect;
        short cam;
        float yaw, pitch, aspectoRatio, scale = 0.5f, speed = 0.2f;

        // actual camera position, direction
        public Vector3 position;
        Vector3 direction;
    

        //For use by TankFollow
        Vector3 posCam;
        Vector3 turretForward, turretRight;

        Vector2 posRato, diferenca = new Vector2(0f, 0f);
        // centro do ecra
        Vector2 centro;
        Vector3 directionInX;

        float heightFree;

        public Camera(GraphicsDevice device, Terreno terreno)
        {
            scale = MathHelper.ToRadians(15) / 50;
            position = new Vector3(50f, 10f, 50f);
            aspectoRatio = (device.Viewport.Width / device.Viewport.Height);
            yaw = -0.5f;
            pitch = 0.4f;

            effect = new BasicEffect(device);

            effect.LightingEnabled = false;
            effect.VertexColorEnabled = true;

            heightFree = SurfaceFollow(position, terreno.alturasdata);
        }

        public void Update(GraphicsDevice device, Terreno terreno, ClsTank tank, ClsTank tankEnemy)
        {
            MouseState rato = Mouse.GetState();
            KeyboardState key = Keyboard.GetState();

            // Alterar cameras
            if (key.IsKeyDown(Keys.F1))
            {
                cam = 1;
            }
            else if (key.IsKeyDown(Keys.F2))
            {
                cam = 2;
            }
            else if (key.IsKeyDown(Keys.F3))
            {
                cam = 3;
            }
            else if (key.IsKeyDown(Keys.F4))
            {
                cam = 4;
            }

            #region Camera Boundings
            if (this.position.Z >= 126)
            {
                position.Z = 126;
            }
            else if (this.position.Z <= 1)
            {
                position.Z = 1;
            }
            else if (this.position.X >= 126)
            {
                position.X = 126;
            }
            else if (this.position.X <= 1)
            {
                position.X = 1;
            }
            #endregion

            switch (cam)
            {
               
                
                case 2:
                    turretForward = Vector3.Normalize(Vector3.Transform(tank.direction, Matrix.CreateFromAxisAngle(tank.tankNormal, tank.turretRotationValue)));
                    turretRight = Vector3.Normalize(Vector3.Cross(turretForward, tank.tankNormal));
                    posCam = (tank.positionTank + 10 * turretForward);
                    position = TankFollow(posCam, terreno);
                    viewMatrix = Matrix.CreateLookAt(position, tank.positionTank, Vector3.Up);
                    break;
                case 3:
                    #region CameraSurfaceFollow

                    diferenca = new Vector2(0f, 0f);
                    // centro do ecra
                    centro = new Vector2(device.Viewport.Width / 2, device.Viewport.Height / 2);
                    // direction do exio Z
                    direction = new Vector3(0.0f, 0.0f, 1f);
                    // calcular a normal entre o vetor Up e o vetor direction, que da o vetor direction do eixos X
                    directionInX = Vector3.Cross(Vector3.Up, direction);
                    posRato.X = rato.X;
                    posRato.Y = rato.Y;
                    // diferença entre o centro e a nova posiçao do rato
                    diferenca = posRato - centro;
                    // atraves da diferença conseguimos o yaw e o pitch
                    yaw -= diferenca.X * scale;

                    pitch -= diferenca.Y * scale;



                    //atualizar
                    MatrixRotacao = Matrix.CreateFromYawPitchRoll(yaw, pitch, 0);
                    direction = Vector3.Transform(direction, MatrixRotacao);
                    directionInX = Vector3.Transform(directionInX, MatrixRotacao);

                    // MOVIMENTAR COM IJKL
                    //corrida com shift
                    if (key.IsKeyDown(Keys.LeftShift))
                        speed = 0.5f;
                    else speed = 0.2f;
                    //WASD
                    if (key.IsKeyDown(Keys.NumPad8))
                        position += direction * speed;
                    if (key.IsKeyDown(Keys.NumPad2))
                        position -= direction * speed;
                    if (key.IsKeyDown(Keys.NumPad4))
                        position += directionInX * speed;
                    if (key.IsKeyDown(Keys.NumPad6))
                        position -= directionInX * speed;

                    position.Y = SurfaceFollow(position, terreno.alturasdata);

                    viewMatrix = Matrix.CreateLookAt(position, (position + direction), Vector3.Up);

                    #endregion
                    break;
                case 4:
                    #region CameraFree
                    diferenca = new Vector2(0f, 0f);
                    // centro do ecra
                    centro = new Vector2(device.Viewport.Width / 2, device.Viewport.Height / 2);
                    // direction do exio Z
                    direction = new Vector3(0.0f, 0.0f, 1f);
                    // calcular a normal entre o vetor Up e o vetor direction, que da o vetor direction do eixos X
                    directionInX = Vector3.Cross(Vector3.Up, direction);
                    posRato.X = rato.X;
                    posRato.Y = rato.Y;
                    // diferença entre o centro e a nova posiçao do rato
                    diferenca = posRato - centro;
                    // atraves da diferença conseguimos o yaw e o pitch
                    yaw -= diferenca.X * scale;
                    
                    pitch -= diferenca.Y * scale;



                    //atualizar
                    MatrixRotacao = Matrix.CreateFromYawPitchRoll(yaw, pitch, 0);
                    direction = Vector3.Transform(direction, MatrixRotacao);
                    directionInX = Vector3.Transform(directionInX, MatrixRotacao);

                    // MOVIMENTAR COM IJKL
                    //corrida com shift
                    if (key.IsKeyDown(Keys.LeftShift))
                        speed = 0.5f;
                    else speed = 0.2f;
                    //WASD
                    if (key.IsKeyDown(Keys.NumPad8))
                        position += direction * speed;
                    if (key.IsKeyDown(Keys.NumPad2))
                        position -= direction * speed;
                    if (key.IsKeyDown(Keys.NumPad4))
                        position += directionInX * speed;
                    if (key.IsKeyDown(Keys.NumPad6))
                        position -= directionInX * speed;

                    

                    if (key.IsKeyDown(Keys.NumPad1))
                        heightFree -= speed;
                    if (key.IsKeyDown(Keys.NumPad7))
                        heightFree += speed;

                    position.Y = heightFree;

                    viewMatrix = Matrix.CreateLookAt(position, (position + direction), Vector3.Up);

                    #endregion
                    break;
                default:
                    posCam = (tank.positionTank + 12 * tank.direction);
                    position = TankFollow(posCam, terreno);
                    viewMatrix = Matrix.CreateLookAt(position, tank.positionTank, Vector3.Up);
                    break;


            }

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), aspectoRatio, 0.1f, 1000f);




        }

        public float SurfaceFollow(Vector3 pos, Vector3[,] alturasdata)
        {
            float altura12, altura34, altura;

            int x = (int)pos.X;
            int z = (int)pos.Z;

            float y1 = alturasdata[x, z].Y;
            float y2 = alturasdata[x + 1, z].Y;
            float y3 = alturasdata[x, z + 1].Y;
            float y4 = alturasdata[x + 1, z + 1].Y;

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

            return (altura + 1.5f);
        }

        public Vector3 TankFollow(Vector3 pos, Terreno terreno)
        {
            pos.Y= SurfaceFollow(position, terreno.alturasdata) + 2.0f;
            return pos;
        }

    }
}
