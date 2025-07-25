using CommunityToolkit.Mvvm.ComponentModel;

namespace PhysicalQuantities.ViewModels.Common;

public partial class ObservableViewModel : ObservableObject
{
    private bool _isInitializing = true;

    /// <summary>
    /// Gets whether the ViewModel has completed initialization
    /// </summary>
    protected bool IsInitialized => !_isInitializing;

    /// <summary>
    /// Marks the initialization as complete. Call this at the end of your constructor
    /// after all properties have been set.
    /// </summary>
    protected void CompleteInitialization()
    {
        _isInitializing = false;
    }
}