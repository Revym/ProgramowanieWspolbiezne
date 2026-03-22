using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    public abstract class LogicAbstractApi
    {
        public abstract void CreateBalls(int count, int boardWidth, int boardHeight);
        public abstract IEnumerable<IBall> GetBalls();

        public abstract void StartSimulation();
        public abstract void StopSimulation();

        
        public static LogicAbstractApi CreateApi(DataAbstractApi? dataApi = default)
        {
            return new LogicApi(dataApi ?? DataAbstractApi.CreateApi());
        }
    }
}
