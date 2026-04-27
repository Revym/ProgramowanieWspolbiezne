using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    public interface IBallStatus
    {
        double X { get; }
        double Y { get; }
        double Radius { get; }
    }
}
