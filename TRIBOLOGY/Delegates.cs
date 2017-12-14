using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRIBOLOGY
{
    internal delegate void StopMoto(object sender, EventArgs e);
    internal delegate void SaveFiles(object sender, EventArgs e);
    internal delegate void InterfaceUpdate(string str, PlotPoint Pt);
    internal delegate void EditPlot(object sender,PlotEventArgs e);
    internal delegate void SysParaUpdate(object sender, EventArgs e);
}
