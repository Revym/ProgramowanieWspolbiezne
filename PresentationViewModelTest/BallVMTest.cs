using Data;
using PresentationViewModel;
using System.ComponentModel;

namespace PresentationViewModelTest;
public class FakeBall : IBall
{
    public double X { get; set; } = 50;
    public double Y { get; set; } = 50;
    public int Radius { get; set; } = 15;

    double IBall.Radius => Radius;

    public event PropertyChangedEventHandler? PropertyChanged;

    public void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

[TestClass]
public class BallVMTest
{
    [TestMethod]
    public void BallVM_ShouldCorrectlyCalculateLeftAndTop()
    {
        IBall fakeBall = new FakeBall();

        BallVM ballVm = new BallVM(fakeBall);

        Assert.AreEqual(35, ballVm.Left);
        Assert.AreEqual(35, ballVm.Top);

        Assert.AreEqual(30, ballVm.Diameter);
    }
}