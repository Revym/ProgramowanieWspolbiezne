using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace BusinessLogicTest
{
    internal class FakeBall : IBall
    {
        private double _x;
        private double _y;
        public double Radius { get; } = 15.0;
        public Vector2D Velocity { get; set; } = new Vector2D(1, 1);
        public double Mass { get; set; }

        public FakeBall(double x, double y, double radius, double mass, Vector2D velocity)
        {
            _x = x;
            _y = y;
            Radius = radius;
            Mass = mass;
            Velocity = velocity;
        }

        public double X
        {
            get => _x;
            set { _x = value; OnPropertyChanged("X"); }
        }

        public double Y
        {
            get => _y;
            set { _y = value; OnPropertyChanged("Y"); }
        }

        public void Move(double newX, double newY)
        {
            _x = newX;
            _y = newY;
            OnPropertyChanged("Position");
            OnPropertyChanged("X");
            OnPropertyChanged("Y");

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
