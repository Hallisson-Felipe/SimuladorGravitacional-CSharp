using System;
using System.Collections.Generic;
using System.Text;

namespace SimuladorGavitacional
{
    internal class Universo
    {
        public Corpo[] C;
        public Universo() 
        {
            C = new Corpo[1000];
        }

        public void GerarCorposAleatorios(int quantidade)
        {
            Random random = new Random();
            for (int i = 0; i < quantidade; i++)
            {
                //tem que achar os limites que Anker falou

                //Corpo corpo = new Corpo();
                //corpo.Massa = ;
                //corpo.Densidade = ;

                //corpo.PosX = ;
                //corpo.PosY = ;

                //corpo.VelX = -10 + ;
                //corpo.VelY = -10 + ;
                //C[i] = corpo;
            }
        }
    }
}
