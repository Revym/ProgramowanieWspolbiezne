using System.ComponentModel;
using System.Runtime.CompilerServices;
using Data;

namespace PresentationViewModel
{
    public class BallVM : INotifyPropertyChanged
    {
        private readonly IBall _ball;

        public BallVM(IBall ball)
        {
            _ball = ball;

            _ball.PropertyChanged += Ball_PropertyChanged;
        }

        private void Ball_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "X")
            {
                OnPropertyChanged(nameof(Left));
            }
            else if (e.PropertyName == "Y")
            {
                OnPropertyChanged(nameof(Top));
            }
        }

        private double _scale = 1.0;
        public double Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                OnPropertyChanged(nameof(Left));
                OnPropertyChanged(nameof(Top));
                OnPropertyChanged(nameof(Diameter));
            }
        }

        public double Left => (_ball.X - _ball.Radius) * Scale;
        public double Top => (_ball.Y - _ball.Radius) * Scale;
        public double Diameter => (_ball.Radius * 2) * Scale;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            _ball.PropertyChanged -= Ball_PropertyChanged;
        }
    }
}