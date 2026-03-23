using BusinessLogic;
using Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PresentationModel
{
    internal class ModelApi : ModelAbstractApi
    {
        private readonly LogicAbstractApi _logicApi;
        public override ObservableCollection<IBall> Balls { get; } = new ObservableCollection<IBall>();

        public ModelApi()
        {
            _logicApi = LogicAbstractApi.CreateApi();
        }

        public override void Start(int ballsCount)
        {
            _logicApi.CreateBalls(ballsCount, 600, 400); // 600x400 to przykładowy rozmiar stołu
            Balls.Clear();
            foreach (var ball in _logicApi.GetBalls())
            {
                Balls.Add(ball);
            }
            _logicApi.StartSimulation();
        }

        public override void Stop()
        {
            _logicApi.StopSimulation();
        }
    }
}
