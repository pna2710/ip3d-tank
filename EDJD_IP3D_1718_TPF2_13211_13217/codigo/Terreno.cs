using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_Fase2
{
    public class Terreno
    {
        Texture2D terreno, terreno_tex;
        int vertexCount;
        public BasicEffect effect;
        public VertexPositionNormalTexture[] vertices;
        public VertexPositionNormalTexture v;
        public Vector3[,] alturasdata;
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
        short[] indices;
        Matrix worldMatrix;
        Color color;
        Color[] ColorShades;
        float yaltura;

        public Terreno(GraphicsDevice device, ContentManager content)
        {
            worldMatrix = Matrix.Identity;
            terreno = content.Load<Texture2D>("HeightMap");
            terreno_tex = content.Load<Texture2D>("terrenotexture");

            effect = new BasicEffect(device);

            //Calcula a aspectRatio, a View Matrix e a projeção
            float aspectRatio = (float)device.Viewport.Width / device.Viewport.Height;
            effect.View = Matrix.CreateLookAt(new Vector3(1.0f, 2.0f, 2.0f), Vector3.Zero, Vector3.Up);
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 0.1f, 10.0f);
            effect.LightingEnabled = true;
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.Texture = terreno_tex;

            effect.DirectionalLight0.DiffuseColor = new Vector3(2, 2, 2);
            //Cor do brilho, para simular um aspeto metálico com uma reflexão da cor da lata
            effect.DirectionalLight0.SpecularColor = new Vector3(0.1f, 0.1f, 0.1f);
            //Direção da luz, negativa para ir de fora para a origem do objeto
            effect.DirectionalLight0.Direction = new Vector3(0.5f, 0.5f, 0.5f);
            effect.DirectionalLight0.Enabled = true;

            alturasdata = CriarVertices(device);
            CalculateNormals();
        }

        private Vector3[,] CriarVertices(GraphicsDevice device)
        {

            float escala = 0.04f;
            ColorShades = new Color[terreno.Width * terreno.Height];
            terreno.GetData<Color>(ColorShades);
            
            vertexCount = ColorShades.Length;

            vertices = new VertexPositionNormalTexture[vertexCount];
            alturasdata = new Vector3[terreno.Width, terreno.Height];

            // criar vertices
            for (int x = 0; x < terreno.Width; x++)
            { 
                for (int z = 0; z < terreno.Height; z++)
                {
                    color = ColorShades[z * terreno.Width + x];
                    yaltura = color.R * escala;
                    v = new VertexPositionNormalTexture(new Vector3(x, yaltura , z), Vector3.Up, new Vector2(x, z));
                    alturasdata[x, z] = new Vector3(x, yaltura, z);
                    vertices[z * terreno.Width + x] = v;
                }
            }
            vertexBuffer = new VertexBuffer(device, typeof(VertexPositionNormalTexture), vertices.Length, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionNormalTexture>(vertices);

            //Criar o Array de Indices
            indices = new short[terreno.Height * 2 * (terreno.Width - 1)];

            for (int x = 0; x < terreno.Width - 1; x++)
            {
                for (int z = 0; z < terreno.Height; z++)
                {
                    indices[2*z+0+x*2*terreno.Height] = (short)(z*terreno.Width+x);
                    indices[2*z+1+x*2*terreno.Height] = (short)(z*terreno.Width+1+x);
                }
            }
            indexBuffer = new IndexBuffer(device, typeof(short), indices.Length, BufferUsage.None);
            indexBuffer.SetData(indices);

            return alturasdata;
        }


        private void CalculateNormals()
        {

            for (int x = 0; x < vertices.Length; x++)
                vertices[x].Normal = Vector3.Zero;
            Vector3 v1, v2, v3, v4, v5, v6, v7, v8;
            Vector3 n1, n2, n3, n4, n5, n6, n7, n8;



            for (int z = 0; z < terreno.Width; z++)
            {
                for (int x = 0; x < terreno.Height; x++)
                {
                    if (z == 0 && x == 0)
                    {
                        // back left corner - 2 tri 3 vertices
                        v1 = alturasdata[x, z + 1] - alturasdata[x, z];
                        v2 = alturasdata[x + 1, z + 1] - alturasdata[x, z];
                        v3 = alturasdata[x + 1, z] - alturasdata[x, z];
                        n1 = Vector3.Cross(v1, v2); n2 = Vector3.Cross(v2, v3);
                        vertices[z * terreno.Width + x].Normal = (n1 + n2) / 2.0f;
                    }
                    else if ((z > 0 && z < (terreno.Width - 1)) && x == 0)
                    {
                        // left edge - 4 tri 5 vertices
                        v1 = alturasdata[x, z - 1] - alturasdata[x, z];
                        v2 = alturasdata[x + 1, z - 1] - alturasdata[x, z];
                        v3 = alturasdata[x + 1, z] - alturasdata[x, z];
                        v4 = alturasdata[x + 1, z + 1] - alturasdata[x, z];
                        v5 = alturasdata[x, z + 1] - alturasdata[x, z];
                        n1 = Vector3.Cross(v1, v2); n2 = Vector3.Cross(v2, v3); n3 = Vector3.Cross(v3, v4); n4 = Vector3.Cross(v4, v5);
                        vertices[z * terreno.Width + x].Normal = (n1 + n2 + n3 + n4) / 4.0f;
                    }
                    else if (z == (terreno.Height - 1) && x == 0)
                    {
                        // front left corner - 2 tri 3 vertices
                        v1 = alturasdata[x, z - 1] - alturasdata[x, z];
                        v2 = alturasdata[x + 1, z - 1] - alturasdata[x, z];
                        v3 = alturasdata[x + 1, z] - alturasdata[x, z];
                        n1 = Vector3.Cross(v1, v2); n2 = Vector3.Cross(v2, v3);
                        vertices[z * terreno.Width + x].Normal = (n1 + n2) / 2.0f;
                    }
                    else if (z == (terreno.Height - 1) && (x > 0 && x < (terreno.Width - 1)))
                    {
                        // front edge - 4 tri 5 vertices
                        v1 = alturasdata[x - 1, z] - alturasdata[x, z];
                        v2 = alturasdata[x - 1, z - 1] - alturasdata[x, z];
                        v3 = alturasdata[x, z - 1] - alturasdata[x, z];
                        v4 = alturasdata[x + 1, z - 1] - alturasdata[x, z];
                        v5 = alturasdata[x + 1, z] - alturasdata[x, z];
                        n1 = Vector3.Cross(v1, v2); n2 = Vector3.Cross(v2, v3); n3 = Vector3.Cross(v3, v4); n4 = Vector3.Cross(v4, v5);
                        vertices[z * terreno.Width + x].Normal = (n1 + n2 + n3) / 3.0f;
                    }
                    else if (z == (terreno.Height - 1) && x == (terreno.Width - 1))
                    {
                        // front right corner - 2 tri 3 vertices
                        v1 = alturasdata[x - 1, z] - alturasdata[x, z];
                        v2 = alturasdata[x - 1, z - 1] - alturasdata[x, z];
                        v3 = alturasdata[x - 1, z] - alturasdata[x, z];
                        n1 = Vector3.Cross(v1, v2); n2 = Vector3.Cross(v2, v3);
                        vertices[z * terreno.Width + x].Normal = (n1 + n2) / 2.0f;
                    }
                    else if ((z > 0 && z < (terreno.Height - 1)) && x == (terreno.Width - 1))
                    {
                        // right edge - 4 tri 5 vertices
                        v1 = alturasdata[x, z - 1] - alturasdata[x, z];
                        v2 = alturasdata[x - 1, z - 1] - alturasdata[x, z];
                        v3 = alturasdata[x - 1, z] - alturasdata[x, z];
                        v4 = alturasdata[x - 1, z + 1] - alturasdata[x, z];
                        v5 = alturasdata[x, z + 1] - alturasdata[x, z];
                        n1 = Vector3.Cross(v1, v2); n2 = Vector3.Cross(v2, v3); n3 = Vector3.Cross(v3, v4); n4 = Vector3.Cross(v4, v5);
                        vertices[z * terreno.Width + x].Normal = (n1 + n2 + n3 + n4) / 4.0f;
                    }
                    else if (z == 0 && x == (terreno.Width - 1))
                    {
                        // back right corner - 2 tri 3 vertices
                        v1 = alturasdata[x - 1, z] - alturasdata[x, z];
                        v2 = alturasdata[x - 1, z + 1] - alturasdata[x, z];
                        v3 = alturasdata[x, z + 1] - alturasdata[x, z];
                        n1 = Vector3.Cross(v1, v2); n2 = Vector3.Cross(v2, v3);
                        vertices[z * terreno.Width + x].Normal = (n1 + n2) / 2.0f;
                    }
                    else if (z == 0 && (x > 0 && x < (terreno.Width - 1)))
                    {
                        // back edge - 4 tri 5 vertices
                        v1 = alturasdata[x - 1, z] - alturasdata[x, z];
                        v2 = alturasdata[x - 1, z + 1] - alturasdata[x, z];
                        v3 = alturasdata[x, z + 1] - alturasdata[x, z];
                        v4 = alturasdata[x + 1, z + 1] - alturasdata[x, z];
                        v5 = alturasdata[x + 1, z] - alturasdata[x, z];
                        n1 = Vector3.Cross(v1, v2); n2 = Vector3.Cross(v2, v3); n3 = Vector3.Cross(v3, v4); n4 = Vector3.Cross(v4, v5);
                        vertices[z * terreno.Width + x].Normal = (n1 + n2 + n3 + n4) / 4.0f;
                    }
                    else
                    {
                        // internal - 8 tri 8 vertices
                        v1 = alturasdata[x, z - 1] - alturasdata[x, z];
                        v2 = alturasdata[x + 1, z - 1] - alturasdata[x, z];
                        v3 = alturasdata[x + 1, z] - alturasdata[x, z];
                        v4 = alturasdata[x + 1, z + 1] - alturasdata[x, z];
                        v5 = alturasdata[x, z + 1] - alturasdata[x, z];
                        v6 = alturasdata[x - 1, z + 1] - alturasdata[x, z];
                        v7 = alturasdata[x - 1, z] - alturasdata[x, z];
                        v8 = alturasdata[x - 1, z - 1] - alturasdata[x, z];

                        n1 = Vector3.Cross(v1, v2); n2 = Vector3.Cross(v2, v3); n3 = Vector3.Cross(v3, v4);
                        n4 = Vector3.Cross(v4, v5); n5 = Vector3.Cross(v5, v6); n6 = Vector3.Cross(v6, v7);
                        n7 = Vector3.Cross(v7, v8); n8 = Vector3.Cross(v8, v1);
                        vertices[z * terreno.Width + x].Normal = (n1 + n2 + n3 + n4 + n5 + n6 + n7 + n8) / 8.0f;
                    }
                }

                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i].Normal.Normalize();
                }

                vertexBuffer.SetData(vertices);
            }
        }

        public Vector3 DevolveNormal(Vector3 position)
        {
            Vector3 normal = -vertices[(int)position.Z * terreno.Width + (int)position.X].Normal;
            return normal; 
        }

        

        public void Draw(GraphicsDevice device, Camera camera)
        {
            effect.World = worldMatrix;
            effect.View = camera.viewMatrix;
            effect.Projection = camera.projectionMatrix;
            effect.CurrentTechnique.Passes[0].Apply();
            device.SetVertexBuffer(vertexBuffer);
            device.Indices = indexBuffer;

            for (int i = 0; i < terreno.Height - 1; i++)
            {
                device.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, 0, ColorShades.Length, i * 256, (terreno.Width - 1) * 2);
                
            }

        }

    }

}
