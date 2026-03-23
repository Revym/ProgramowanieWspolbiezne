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

        public double Left => _ball.X - _ball.Radius;
        public double Top => _ball.Y - _ball.Radius;
        public double Diameter => _ball.Radius * 2;

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