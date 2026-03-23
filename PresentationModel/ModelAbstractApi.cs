using Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PresentationModel
{
    public abstract class ModelAbstractApi
    {
        public abstract ObservableCollection<IBall> Balls { get; }
        public abstract void Start(int ballsCount);
        public abstract void Stop();

        public static ModelAbstractApi CreateApi()
        {
            return new ModelApi();
        }
    }
}
