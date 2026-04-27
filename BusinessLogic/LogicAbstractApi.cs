using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    public abstract class LogicAbstractApi
    {
        public abstract int BoardWidth { get; }
        public abstract int BoardHeight { get; }

        public abstract void CreateBalls(int count, int boardWidth, int boardHeight);
        public abstract IEnumerable<IBallStatus> GetBallsStatus();

        public abstract void StartSimulation();
        public abstract void StopSimulation();

        public abstract event EventHandler<IEnumerable<IBallStatus>>? SimulationUpdated;

        
        public static LogicAbstractApi CreateApi(DataAbstractApi? dataApi = default)
        {
            return new LogicApi(dataApi ?? DataAbstractApi.CreateApi());
        }
    }
}
