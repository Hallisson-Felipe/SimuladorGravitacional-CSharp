using System;
using System.Collections.Generic;
using System.Text;

namespace SimuladorGavitacional
{
    public class PanelUniverso : Panel
    {
        //classe personalizada que herda de Panel para melhorar a renderizacao dos corpos no universo
        public PanelUniverso()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
        }
    }
}
