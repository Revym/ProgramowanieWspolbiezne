using PresentationViewModel;

namespace PresentationViewModelTest;

[TestClass]
public class MainViewModelTest
{
    [TestMethod]
    public void MainViewModel_StartCommand_ShouldCreateBalls()
    {
        MainViewModel vm = new MainViewModel();

        vm.BallsCountText = "7"; 
        vm.StartCommand.Execute(null);

        Assert.HasCount(7, vm.Balls, "Lista nie zapełniła się 7 kulami!");
    }
}