using BusinessLogic;
using System;
using System.Collections.Generic;

namespace PresentationModel
{
    internal class ModelApi : ModelAbstractApi
    {
        private readonly LogicAbstractApi _logicApi;

        public override event EventHandler<IEnumerable<IBallStatus>>? ModelUpdated;

        public override int BoardWidth => _logicApi.BoardWidth;
        public override int BoardHeight => _logicApi.BoardHeight;

        public ModelApi()
        {
            _logicApi = LogicAbstractApi.CreateApi();
            _logicApi.SimulationUpdated += (sender, args) => ModelUpdated?.Invoke(this, args);
        }

        public override void Start(int ballsCount)
        {
            _logicApi.CreateBalls(ballsCount, 600, 400);
            _logicApi.StartSimulation();
        }

        public override void Stop()
        {
            _logicApi.StopSimulation();
        }
    }
}