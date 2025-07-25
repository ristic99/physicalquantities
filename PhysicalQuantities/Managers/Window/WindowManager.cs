namespace PhysicalQuantities.Managers.Window;

public class WindowManager : IWindowManager
{
    public void Show(System.Windows.Window window)
    {
        window.Show();
    }

    public void Close(System.Windows.Window window)
    {
        window.Close();
    }
}