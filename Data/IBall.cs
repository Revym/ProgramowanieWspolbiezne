using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Data
{
    public interface IBall : INotifyPropertyChanged
    {
        double X { get; set; }
        double Y { get; set; }
        double Radius { get; }
        double Mass { get; }
        Vector2D Velocity { get; set; }
    }
}
