using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PresentationViewModel
{
    public class BallVM : INotifyPropertyChanged
    {
        private double _x;
        private double _y;
        private double _radius;
        private double _scale = 1.0;

        public BallVM() { }

        public double X
        {
            get => _x;
            set { _x = value; OnPropertyChanged(); OnPropertyChanged(nameof(Left)); }
        }

        public double Y
        {
            get => _y;
            set { _y = value; OnPropertyChanged(); OnPropertyChanged(nameof(Top)); }
        }

        public double Radius
        {
            get => _radius;
            set
            {
                _radius = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Diameter));
                OnPropertyChanged(nameof(Left));
                OnPropertyChanged(nameof(Top));
            }
        }

        public double Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Left));
                OnPropertyChanged(nameof(Top));
                OnPropertyChanged(nameof(Diameter));
            }
        }

        public double Left => (X - Radius) * Scale;
        public double Top => (Y - Radius) * Scale;
        public double Diameter => (Radius * 2) * Scale;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}