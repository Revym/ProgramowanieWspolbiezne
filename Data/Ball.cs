using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Data
{
    internal class Ball : IBall
    {
        private double _x;
        private double _y;
        private readonly double _radius;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Ball(double x, double y, double radius)
        {
            _x = x;
            _y = y;
            _radius = radius;
        }

        public double X
        {
            get => _x;
            set
            {
                if (_x != value)
                {
                    _x = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Y
        {
            get => _y;
            set
            {
                if (_y != value)
                {
                    _y = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Radius => _radius;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
