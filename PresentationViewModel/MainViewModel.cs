using BusinessLogic;
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
        public double LogicalWidth => _model.BoardWidth;
        public double LogicalHeight => _model.BoardHeight;

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

        public MainViewModel() : this(ModelAbstractApi.CreateApi()) {}

        public MainViewModel(ModelAbstractApi model)
        {
            _model = model;

            _model.ModelUpdated += (sender, statuses) =>
            {
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() => UpdateBallsList(statuses));
                }
                else
                {
                    UpdateBallsList(statuses);
                }
            };

            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
        }

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


        private void UpdateBallsList(IEnumerable<IBallStatus> statuses)
        {
            var statusList = new List<IBallStatus>(statuses);

            if (Balls.Count != statusList.Count)
            {
                Balls.Clear();
                foreach (var status in statusList)
                {
                    Balls.Add(new BallVM { X = status.X, Y = status.Y, Radius = status.Radius, Scale = this.Scale });
                }
            }
            else
            {
                for (int i = 0; i < statusList.Count; i++)
                {
                    Balls[i].X = statusList[i].X;
                    Balls[i].Y = statusList[i].Y;
                }
            }
        }

        private void Start()
        {
            if (int.TryParse(BallsCountText, out int count) && count > 0)
            {
                Stop();

                Balls.Clear();

                _model.Start(count);
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
