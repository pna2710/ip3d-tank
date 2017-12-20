using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3D_projeto_final
{
        class Particula
        {
            public Vector3 posicao;
            public Vector3 velocidade;
            public VertexPositionColor[] verticesP;

            public Particula(Vector3 p, Vector3 v)
            {
                this.posicao = p;
                this.velocidade = v;
                verticesP = new VertexPositionColor[2];
            }
    }
}
