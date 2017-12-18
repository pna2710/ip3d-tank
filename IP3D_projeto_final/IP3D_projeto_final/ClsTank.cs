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
    class ClsTank
    {
        Model myModel;

        Matrix world;

        ModelBone turretBone;
        ModelBone cannonBone;
        ModelBone rFrontWheelBone;
        ModelBone lFrontWheelBone;
        ModelBone rBackWheelBone;
        ModelBone lBackWheelBone;
        ModelBone rSteerBone;
        ModelBone lSteerBone;
        ModelBone rEngineBone;
        ModelBone lEngineBone;
        ModelBone hatchBone;

        Matrix matrixrotacao;
        Matrix cannonTransform;
        Matrix turretTransform;
        Matrix rFrontWheelTransform;
        Matrix lFrontWheelTransform;
        Matrix rBackWheelTransform;
        Matrix lBackWheelTransform;
        Matrix rSteerTransform;
        Matrix lSteerTransform;
        Matrix rEngineTransform;
        Matrix lEngineTransform;
        Matrix hatchTransform;
        Matrix[] bonetransforms;

        Vector3 normalant;
        Vector3 tankRight;
        Vector3 tanknormal;
        Vector3 tankdirecao;
        public Vector3 direction = new Vector3(1, 0, 0);
        public Vector3 Aux;

        float wheelRotationValue = 0, steerRotationValue = 0, turretRotationValue = 0, cannonRotationValue = 0;
        float scale = 0.005f, yaw = 0, speed = 0.3f;
        public Vector3 positionTank;
        Bullet bala;
<<<<<<< HEAD
        BoundingSphere spheretank;
        public int playernumber;
        KeyboardState kb;
=======
        BoundingSphere sphereTank, sphereTankEnemy;
>>>>>>> master


        public ClsTank(GraphicsDevice device, ContentManager content, Terreno terreno, int playernumber)
        {
            myModel = content.Load<Model>("tank");
            world = Matrix.CreateScale(0.005f);//Matrix.Identity; 
            positionTank = new Vector3(64f, 10f, 64f);
            //spheretank1 = new BoundingSphere(positionTank, (float)2f);
            this.playernumber = playernumber;

            //modelos individuas do tank. 
            turretBone = myModel.Bones["turret_geo"];
            cannonBone = myModel.Bones["canon_geo"];
            hatchBone = myModel.Bones["hatch_geo"];
            rFrontWheelBone = myModel.Bones["r_front_wheel_geo"];
            lFrontWheelBone = myModel.Bones["l_front_wheel_geo"];
            rBackWheelBone = myModel.Bones["r_back_wheel_geo"];
            lBackWheelBone = myModel.Bones["l_back_wheel_geo"];
            rSteerBone = myModel.Bones["r_steer_geo"];
            lSteerBone = myModel.Bones["l_steer_geo"];
            rEngineBone = myModel.Bones["r_engine_geo"];
            lEngineBone = myModel.Bones["l_engine_geo"];

            //tranformacoes dos modelos individuais do tank 
            turretTransform = turretBone.Transform;
            cannonTransform = cannonBone.Transform;
            rFrontWheelTransform = rFrontWheelBone.Transform;
            lFrontWheelTransform = lFrontWheelBone.Transform;
            rBackWheelTransform = rBackWheelBone.Transform;
            lBackWheelTransform = lBackWheelBone.Transform;
            rSteerTransform = rSteerBone.Transform;
            rEngineTransform = rEngineBone.Transform;
            lSteerTransform = lSteerBone.Transform;
            lEngineTransform = lEngineBone.Transform;
            hatchTransform = hatchBone.Transform;

            bonetransforms = new Matrix[myModel.Bones.Count];
        }

<<<<<<< HEAD
        public void UpdatePlayer(KeyboardState kb, Terreno terreno)
=======
        public void UpdatePlayer(KeyboardState kb, Camera camera, Terreno terreno, ClsTank tankEnemy)
>>>>>>> master
        {
            KeyboardState key = Keyboard.GetState();


            if (key.IsKeyDown(Keys.LeftShift))
                speed = 0.2f;
            else speed = 0.12f;

            // RODAR PITCH E YAW DO CANHAO 
            if (key.IsKeyDown(Keys.Up))
            {
                if (cannonRotationValue > -1.6f)
                    cannonRotationValue -= 0.01f;
                cannonBone.Transform = Matrix.CreateRotationX(MathHelper.ToRadians(cannonRotationValue)) * cannonTransform;
            }
            if (key.IsKeyDown(Keys.Down))
            {
                if (cannonRotationValue < 0.2f)
                    cannonRotationValue += 0.01f;
                cannonBone.Transform = Matrix.CreateRotationX(MathHelper.ToRadians(cannonRotationValue)) * cannonTransform;
            }
            if (key.IsKeyDown(Keys.Left))
            {
                turretRotationValue += 0.02f;
                turretBone.Transform = Matrix.CreateRotationY(MathHelper.ToRadians(turretRotationValue)) * turretTransform;
            }
            if (key.IsKeyDown(Keys.Right))
            {
                turretRotationValue -= 0.02f;
                turretBone.Transform = Matrix.CreateRotationY(MathHelper.ToRadians(turretRotationValue)) * turretTransform;
            }

            // MOVER TANK 
            if ((key.IsKeyUp(Keys.D) && key.IsKeyUp(Keys.A)) || (key.IsKeyDown(Keys.D) && key.IsKeyDown(Keys.A)))
            {
                steerRotationValue = 0;
                yaw = 0;
            }
            else if (key.IsKeyDown(Keys.A))
            {
                yaw = MathHelper.ToRadians(1f);
                if (steerRotationValue < 0.6f)
                    steerRotationValue += 0.1f;

            }
            else if (key.IsKeyDown(Keys.D))
            {
                yaw = -MathHelper.ToRadians(1f);
                if (steerRotationValue > -0.6f)
                    steerRotationValue -= 0.1f;

            }

            Vector3 aux1 = (positionTank + (direction * speed));
            if ((key.IsKeyUp(Keys.W) && key.IsKeyUp(Keys.S)) || (key.IsKeyDown(Keys.W) && key.IsKeyDown(Keys.S)))
            {

                wheelRotationValue = 0;

            }
            else if (key.IsKeyDown(Keys.W))
            {
                wheelRotationValue += 0.07f;

                matrixrotacao = Matrix.CreateFromYawPitchRoll(yaw, 0, 0);
                direction = Vector3.Transform(direction, matrixrotacao);


                positionTank -= direction * speed;
            }

            Vector3 aux = (positionTank + (direction * speed));

            if (key.IsKeyDown(Keys.S))
            {
                wheelRotationValue -= 0.07f;

                matrixrotacao = Matrix.CreateFromYawPitchRoll(-yaw, 0, 0);
                direction = Vector3.Transform(direction, matrixrotacao);

                positionTank += direction * speed;
            }

<<<<<<< HEAD
            // Dispara Bala
            /* if (key.IsKeyDown(Keys.Space) && bala.IsMove == false)
              {
                  bala.position = tank.positionTank + (tank.tanknormal * 1.6f);
                  Vector3 direcaoTorre = tank.Aux;
                  direcaoTorre.Y = -direcaoTorre.Y;
                  direcaoTorre = Vector3.Transform(direcaoTorre, Matrix.CreateFromAxisAngle(tank.tanknormal, MathHelper.ToRadians(tank.turretRotationValue)));
                  Vector3 direitaTorre = Vector3.Cross(direcaoTorre, tank.tanknormal);
                  direcaoTorre = Vector3.Transform(direcaoTorre, Matrix.CreateFromAxisAngle(direitaTorre, MathHelper.ToRadians(-tank.cannonRotationValue)));
                  direcaoTorre = Vector3.Transform(direcaoTorre, Matrix.CreateRotationY(MathHelper.ToRadians(180f)));
                  float ajuste = (float)Math.Atan2(tank.tanknormal.Length(), Vector3.Up.Length());
                  ajuste += MathHelper.ToRadians(90);
                  direcaoTorre = Vector3.Transform(direcaoTorre, Matrix.CreateFromAxisAngle(direcaoTorre, MathHelper.ToRadians(ajuste)));
                  bala.Init(direcaoTorre);
              }

              if (bala.IsMove == true)
              {

                  // Testa se a bala está dentro do terreno
                  if (bala.position.X > 0 && bala.position.X < terreno.with && bala.position.Z > 0 && bala.position.Z < terreno.height)
                  {
                      // Caso a altura da bala seja menor ou igual à altura do terreno para a posicao actual, desactiva a bala
                      if (bala.position.Y <= (camera.SurfaceFollow(bala.position,terreno.alturasdata) - 2.5f))
                      {
                          bala.IsMove = false;
                      }
                  }
                  // Está fora do terreno, desactiva.
                  else
                  {
                      bala.IsMove = false;
                  }
              }*/
        }
=======
>>>>>>> master

        public void UpdateEnemy(Terreno terreno, GameTime time,ClsTank otherTank)
        {
            float distancia = Vector3.Distance(positionTank, otherTank.positionTank);

            // MOVER TANK
            Vector3 DirectFut = (otherTank.positionTank + tankdirecao) - positionTank;
            Vector3 a = (DirectFut - direction);
            a.Normalize();
           

            Vector3 v =  Vector3.Zero + a * speed;

            direction = v;
            direction = direction * -1;
            direction.Normalize();

            matrixrotacao = Matrix.Identity;

<<<<<<< HEAD
            if (distancia > 5f)
            {
                //ANDAR
                wheelRotationValue += 0.07f;

                matrixrotacao = Matrix.CreateFromYawPitchRoll(yaw, 0, 0);
                direction = Vector3.Transform(direction, matrixrotacao);
                positionTank -= direction * 0.05f;
            }

        }

        public void update(Terreno terreno, GameTime time, GraphicsDevice device, Camera camera , KeyboardState kb, ClsTank otherTank)
        {
=======
            myModel.Root.Transform = Matrix.CreateScale(scale) * rotacao * translacao;
            turretBone.Transform = Matrix.CreateRotationY(turretRotationValue) * turretTransform;
            cannonBone.Transform = Matrix.CreateRotationX(cannonRotationValue) * cannonTransform;
            rFrontWheelBone.Transform = Matrix.CreateRotationX(wheelRotationValue) * rFrontWheelTransform;
            lFrontWheelBone.Transform = Matrix.CreateRotationX(wheelRotationValue) * lFrontWheelTransform;
            rBackWheelBone.Transform = Matrix.CreateRotationX(wheelRotationValue) * rBackWheelTransform;
            lBackWheelBone.Transform = Matrix.CreateRotationX(wheelRotationValue) * lBackWheelTransform;
            rSteerBone.Transform = Matrix.CreateRotationY(steerRotationValue) * rSteerTransform;
            lSteerBone.Transform = Matrix.CreateRotationY(steerRotationValue) * lSteerTransform;
            myModel.CopyAbsoluteBoneTransformsTo(bonetransforms);

            /*Surface Follow e Normal Follow para interpolação na translação em y e na rotação do tanque, respetivamente, 
             *de forma a acompanhar as mudanças de altitude no terreno */
            positionTank.Y = SurfaceFollow(positionTank, terreno.alturasdata);
            tanknormal = NormalFollow(positionTank, terreno);

            if (IsColliding(myModel, tankEnemy.myModel, world, tankEnemy.world))
            {
                positionTank.X += 20;
            }

        }

        public void UpdateEnemy(KeyboardState kb, Camera camera, Terreno terreno)
        {
            KeyboardState key = Keyboard.GetState();

            if (key.IsKeyDown(Keys.LeftShift))
                speed = 0.2f;
            else speed = 0.12f;

            // RODAR PITCH E YAW DO CANHAO 
            if (key.IsKeyDown(Keys.NumPad8))
            {
                if (cannonRotationValue > -1.6f)
                    cannonRotationValue -= 0.01f;
                cannonBone.Transform = Matrix.CreateRotationX(MathHelper.ToRadians(cannonRotationValue)) * cannonTransform;
            }
            if (key.IsKeyDown(Keys.NumPad2))
            {
                if (cannonRotationValue < 0.2f)
                    cannonRotationValue += 0.01f;
                cannonBone.Transform = Matrix.CreateRotationX(MathHelper.ToRadians(cannonRotationValue)) * cannonTransform;
            }
            if (key.IsKeyDown(Keys.NumPad4))
            {
                turretRotationValue += 0.02f;
                turretBone.Transform = Matrix.CreateRotationY(MathHelper.ToRadians(turretRotationValue)) * turretTransform;
            }
            if (key.IsKeyDown(Keys.NumPad6))
            {
                turretRotationValue -= 0.02f;
                turretBone.Transform = Matrix.CreateRotationY(MathHelper.ToRadians(turretRotationValue)) * turretTransform;
            }

            // MOVER TANK 
            if ((key.IsKeyUp(Keys.F) && key.IsKeyUp(Keys.H)) || (key.IsKeyDown(Keys.F) && key.IsKeyDown(Keys.H)))
            {
                steerRotationValue = 0;
                yaw = 0;
            }
            if (key.IsKeyDown(Keys.F))
            {
                yaw = MathHelper.ToRadians(1f);
                if (steerRotationValue < 0.6f)
                    steerRotationValue += 0.1f;

            }
            if (key.IsKeyDown(Keys.H))
            {
                yaw = -MathHelper.ToRadians(1f);
                if (steerRotationValue > -0.6f)
                    steerRotationValue -= 0.1f;

            }


            if ((key.IsKeyUp(Keys.T) && key.IsKeyUp(Keys.G)) || (key.IsKeyDown(Keys.T) && key.IsKeyDown(Keys.G)))
            {

                wheelRotationValue = 0;

            }
            else if (key.IsKeyDown(Keys.T))
            {
                wheelRotationValue += 0.07f;

                matrixrotacao = Matrix.CreateFromYawPitchRoll(yaw, 0, 0);
                direction = Vector3.Transform(direction, matrixrotacao);

                positionTank -= direction * speed;
            }
            else if (key.IsKeyDown(Keys.G))
            {
                wheelRotationValue -= 0.07f;

                matrixrotacao = Matrix.CreateFromYawPitchRoll(-yaw, 0, 0);
                direction = Vector3.Transform(direction, matrixrotacao);

                positionTank += direction * speed;
            }




>>>>>>> master
            if (this.positionTank.Z >= 126)
            {
                positionTank.Z = 126;
            }
            if (this.positionTank.Z <= 1)
            {
                positionTank.Z = 1;
            }
            if (this.positionTank.X >= 126)
            {
                positionTank.X = 126;
            }
            if (this.positionTank.X <= 1)
            {
                positionTank.X = 1;
            }

            Matrix translacao = Matrix.CreateTranslation(positionTank);
            Matrix rotacao = Matrix.Identity;

            normalant = tanknormal;
            tankRight = Vector3.Cross(direction, tanknormal);
            tankdirecao = Vector3.Cross(tanknormal, tankRight);

            rotacao.Forward = tankdirecao;
            rotacao.Up = tanknormal;
            rotacao.Right = tankRight;

            myModel.Root.Transform = Matrix.CreateScale(scale) * rotacao * translacao;

            turretBone.Transform = Matrix.CreateRotationY(turretRotationValue) * turretTransform;
            cannonBone.Transform = Matrix.CreateRotationX(cannonRotationValue) * cannonTransform;
            rFrontWheelBone.Transform = Matrix.CreateRotationX(wheelRotationValue) * rFrontWheelTransform;
            lFrontWheelBone.Transform = Matrix.CreateRotationX(wheelRotationValue) * lFrontWheelTransform;
            rBackWheelBone.Transform = Matrix.CreateRotationX(wheelRotationValue) * rBackWheelTransform;
            lBackWheelBone.Transform = Matrix.CreateRotationX(wheelRotationValue) * lBackWheelTransform;
            rSteerBone.Transform = Matrix.CreateRotationY(steerRotationValue) * rSteerTransform;
            lSteerBone.Transform = Matrix.CreateRotationY(steerRotationValue) * lSteerTransform;
            myModel.CopyAbsoluteBoneTransformsTo(bonetransforms);

            /*Surface Follow e Normal Follow para interpolação na translação em y e na rotação do tanque, respetivamente, 
             *de forma a acompanhar as mudanças de altitude no terreno */
            positionTank.Y = SurfaceFollow(positionTank, terreno.alturasdata);
            tanknormal = NormalFollow(positionTank, terreno);
        }

        private bool IsColliding(Model tank1, Model tank2, Matrix world1, Matrix world2)
        {
            for (int i = 0; i < tank1.Meshes.Count; i++)
            {
                sphereTank = tank1.Meshes[i].BoundingSphere;
                sphereTank = sphereTank.Transform(world1);

                for (int j = 0; j < tank2.Meshes.Count; j++)
                {
                    sphereTankEnemy = tank2.Meshes[j].BoundingSphere;
                    sphereTankEnemy = sphereTankEnemy.Transform(world2);

<<<<<<< HEAD
            if (playernumber == 1)
            {
                UpdatePlayer(kb,terreno);
            }
            else if(playernumber == 2)
            {
                UpdateEnemy(terreno, time,otherTank);
            }
=======
                    if (sphereTank.Intersects(sphereTankEnemy))
                        return true;
                }
            }
            return false;
>>>>>>> master
        }

       /* public bool bater (BoundingSphere spheretank1, BoundingSphere spheretank2)
        {
            spheretank1 = new BoundingSphere(positionTank, 2f);
            spheretank2 = new BoundingSphere(positionTank, 2f);



        }*/

        //#region Surface Follow
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

            return (altura);
        }
        //#endregion

        #region Normal Follow
        public Vector3 NormalFollow(Vector3 pos, Terreno terreno)
        {
            Vector3 normal = new Vector3(0, 0, 0);
            Vector3 altura12, altura34;

            int x = (int)pos.X;
            int z = (int)pos.Z;

            Vector3 y1 = terreno.DevolveNormal(new Vector3(pos.X, pos.Y, pos.Z));
            Vector3 y2 = terreno.DevolveNormal(new Vector3(pos.X + 1, pos.Y, pos.Z));
            Vector3 y3 = terreno.DevolveNormal(new Vector3(pos.X, pos.Y, pos.Z + 1));
            Vector3 y4 = terreno.DevolveNormal(new Vector3(pos.X + 1, pos.Y, pos.Z + 1));

            float d1x = pos.X - x;
            float d2x = 1 - d1x;
            float d3x = d1x;
            float d4x = 1 - d3x;

            float d1z = pos.Z - z;
            float d3z = 1 - d1z;

            // interpolacao bilinear. Encontrar a altura(saber o valor)
            altura12 = (d2x * y1) + (d1x * y2);
            altura34 = (d4x * y3) + (d3x * y4);
            normal = altura12 * d3z + altura34 * d1z;

            return (normal);
        }
        #endregion

        public Vector3 TankFollow(Vector3 position, Vector3 direcao, Terreno terreno)
        {
            Vector3 posCam;
            Vector3 posA;
            posCam = (position + 10 * direcao);
            posA = terreno.vertices[(int)position.Z + (int)position.X].Position;
            posCam.Y = SurfaceFollow(position, terreno.alturasdata) + 4.5f;
            return posCam;
        }

        public void Draw(GraphicsDevice device, Camera camera)
        {

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = bonetransforms[mesh.ParentBone.Index];
                    effect.View = camera.viewMatrix;
                    effect.Projection = camera.projectionMatrix;
                    effect.EnableDefaultLighting();
                }
                // Draw each mesh of the model
                mesh.Draw();
            }
        }
    }
}
