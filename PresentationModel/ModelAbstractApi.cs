using BusinessLogic;
using System;
using System.Collections.Generic;

namespace PresentationModel
{
    public abstract class ModelAbstractApi
    {
        public abstract void Start(int ballsCount);
        public abstract void Stop();

        public abstract event EventHandler<IEnumerable<IBallStatus>>? ModelUpdated;

        public abstract int BoardWidth { get; }
        public abstract int BoardHeight { get; }

        public static ModelAbstractApi CreateApi()
        {
            return new ModelApi();
        }
    }
}