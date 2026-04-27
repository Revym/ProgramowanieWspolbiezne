using Data;
using PresentationModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace PresentationViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private const double LogicalWidth = 600;
        private const double LogicalHeight = 400;

        private double _actualWidth;
        private double _actualHeight;

        public double ActualWidth
        {
            get => _actualWidth;
            set { _actualWidth = value; OnPropertyChanged(); OnPropertyChanged(nameof(Scale)); UpdateBallScaling(); }
        }

        public double ActualHeight
        {
            get => _actualHeight;
            set { _actualHeight = value; OnPropertyChanged(); OnPropertyChanged(nameof(Scale)); UpdateBallScaling(); }
        }

        public double Scale => Math.Min(ActualWidth / LogicalWidth, ActualHeight / LogicalHeight);

        private void UpdateBallScaling()
        {
            foreach (var ball in Balls)
            {
                ball.Scale = this.Scale;
            }
        }

        private readonly ModelAbstractApi _model;

        private string _ballsCountText = "5";
        public string BallsCountText
        {
            get => _ballsCountText;
            set
            {
                _ballsCountText = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BallVM> Balls { get; } = new ObservableCollection<BallVM>();

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        public MainViewModel()
        {
            _model = ModelAbstractApi.CreateApi();

            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
        }

        private void Start()
        {
            if (int.TryParse(BallsCountText, out int count) && count > 0)
            {
                Stop();
                _model.Start(count);

                foreach (var oldBall in Balls)
                {
                    oldBall.Dispose();
                }
                Balls.Clear();

                foreach (var ball in _model.Balls)
                {
                    Balls.Add(new BallVM(ball));
                }
                UpdateBallScaling();
            }
        }

        private void Stop()
        {
            _model.Stop();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
