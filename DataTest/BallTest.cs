using Data;

namespace DataTest;

[TestClass]
public class BallTest
{
    [TestMethod]
    public void Ball_PropertyChanged_ShouldRaiseEventWhenXChanges()
    {
        IBall ball = DataAbstractApi.CreateApi().GetBalls().FirstOrDefault() ?? new Ball(10, 10, 5, new Vector2D(1,2));
        bool eventRaised = false;

        ball.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == "X") eventRaised = true;
        };

        ball.X = 20;

        Assert.IsTrue(eventRaised);
    }
}
