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
        #region Variables
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
        Matrix[] boneTransforms;

        Vector3 normalAnt;
        Vector3 tankRight;
        Vector3 tankNormal;
        Vector3 tankDirecao;
        Matrix translacao;
        Matrix rotacao;

        //tank number, pos and dir
        public int playernumber;
        public Vector3 positionTank;
        public Vector3 tempPosition;
        public Vector3 direction = new Vector3(1, 0, 0);
        
        float wheelRotationValue = 0, steerRotationValue = 0, turretRotationValue = 0, cannonRotationValue = 0;
        float scale = 0.005f, yaw = 0, speed = 0.3f;

        //Collisions
        private BoundingSphere boundingSphere;
        BoundingSphere transformed, sphere;
        Matrix worldTransform;
        


        //Bullets
        Bullet bTank;
        

        #endregion

        //Tank 
        public ClsTank(GraphicsDevice device, ContentManager content, Vector3 position, int playernumber)
        {
            myModel = content.Load<Model>("tank");
            world = Matrix.CreateScale(0.005f);//Matrix.Identity;
            positionTank = position;
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

            boneTransforms = new Matrix[myModel.Bones.Count];

            buildBoundingSphere();
        }

        //Update
        public void Update(GraphicsDevice device, ContentManager content, GameTime time, Terreno terreno, ClsTank tankPlayer)
        {
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

            translacao = Matrix.CreateTranslation(positionTank);
            rotacao = Matrix.Identity;

            normalAnt = tankNormal;
            tankRight = Vector3.Cross(direction, tankNormal);
            tankDirecao = Vector3.Cross(tankNormal, tankRight);

            rotacao.Forward = tankDirecao;
            rotacao.Up = tankNormal;
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
            myModel.CopyAbsoluteBoneTransformsTo(boneTransforms);

            /*Surface Follow e Normal Follow para interpolação na translação em y e na rotação do tanque, respetivamente,
             *de forma a acompanhar as mudanças de altitude no terreno */
            positionTank.Y = SurfaceFollow(positionTank, terreno.alturasdata);
            tankNormal = NormalFollow(positionTank, terreno);

            if (playernumber == 1)
            {
                UpdatePlayer(device, content, time);
            }
            else if(playernumber == 2)
            {
                UpdateEnemy(device, content,tankPlayer);
            }
        }

        //Specific Update 1
        public void UpdatePlayer(GraphicsDevice device, ContentManager content, GameTime time)
        {
            KeyboardState key = Keyboard.GetState();
            tempPosition = positionTank;

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
            else if (key.IsKeyDown(Keys.S))
            {
                wheelRotationValue -= 0.07f;

                matrixrotacao = Matrix.CreateFromYawPitchRoll(-yaw, 0, 0);
                direction = Vector3.Transform(direction, matrixrotacao);

                positionTank += direction * speed;
            }

            // Dispara Bala
            if (key.IsKeyDown(Keys.Space))
            {
                bTank = new Bullet(device, content, cannonTransform.Translation, cannonTransform.Forward);
            }

            //Update da bala
            if(bTank != null)
            {
                bTank.Update(time);
            }

            
        }

        //Specific Update 2
        public void UpdateEnemy(GraphicsDevice device, ContentManager content, ClsTank tankPlayer)
        {
            float distancia = Vector3.Distance(positionTank, tankPlayer.positionTank);

            // MOVER TANK
            Vector3 DirectFut = (tankPlayer.positionTank + tankDirecao) - positionTank;
            Vector3 a = (DirectFut - direction);
            a.Normalize();
           

            Vector3 v =  Vector3.Zero + a * speed;

            direction = v;
            direction = direction * -1;
            direction.Normalize();

            matrixrotacao = Matrix.Identity;

            if (distancia > 5f)
            {
                //ANDAR
                wheelRotationValue += 0.07f;

                matrixrotacao = Matrix.CreateFromYawPitchRoll(yaw, 0, 0);
                direction = Vector3.Transform(direction, matrixrotacao);
                positionTank -= direction * 0.05f;
            }

        }
        
        //Tank Draw
        public void Draw(GraphicsDevice device, Camera camera)
        {

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = boneTransforms[mesh.ParentBone.Index];
                    effect.View = camera.viewMatrix;
                    effect.Projection = camera.projectionMatrix;
                    effect.EnableDefaultLighting();
                }
                // Draw each mesh of the model
                mesh.Draw();
            }

            if(bTank != null)
            {
                bTank.Draw(device, camera);
            }
            
        }

        #region Surface Follow
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
        #endregion

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

        #region BoundingSphere
        public BoundingSphere BoundingSphere
        {
            get
            {
                // No need for rotation, as this is a sphere
                worldTransform = Matrix.CreateScale(scale) * Matrix.CreateTranslation(positionTank);
                transformed = boundingSphere;
                transformed = transformed.Transform(worldTransform);
                return transformed;
            }
        }

        private void buildBoundingSphere()
        {
            sphere = new BoundingSphere(myModel.Meshes[0].BoundingSphere.Center, myModel.Meshes[0].BoundingSphere.Radius * 1.8f);
            foreach (ModelMesh mesh in myModel.Meshes)
            {
                transformed = mesh.BoundingSphere.Transform(boneTransforms[mesh.ParentBone.Index]);
                sphere = BoundingSphere.CreateMerged(sphere, transformed);
            }
            this.boundingSphere = sphere;
        }
        #endregion
    }
}
