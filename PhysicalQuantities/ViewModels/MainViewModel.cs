using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PhysicalQuantities.Core.Enums;
using PhysicalQuantities.Core.Physics;
using PhysicalQuantities.Models;
using PhysicalQuantities.ViewModels.Common;

namespace PhysicalQuantities.ViewModels
{
    //I just spent hours debugging why my calculation was wrong, only to discover I mixed up milliwatts and watts
    //There HAS to be a better way.
    //What if the computer actually understood physics instead of just doing blind arithmetic?

    //Tired of manual unit conversion errors in engineering software
    //Annoyed by calculators that just do math without understanding physics

    //It's a type-safe unit system with automatic dimensional analysis. The PhysicalQuantity struct carries both value and dimensional information.
    //The DimensionalAnalysisEngine uses dimensional formulas to determine result types from operations.
    //WPF behaviors handle the UI binding with automatic unit conversion.

    public partial class MainViewModel : ObservableViewModel
    {
        private readonly ElectricalCircuit _circuit;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Current))]
        private PhysicalQuantity _voltage;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Current))]
        private PhysicalQuantity _resistance;

        public PhysicalQuantity Current => _circuit.Current;

        public MainViewModel()
        {
            _circuit = new ElectricalCircuit(
                new PhysicalQuantity(12.0, PhysicalQuantityType.Voltage),
                new PhysicalQuantity(6.0, PhysicalQuantityType.Resistance)
            );

            Voltage = _circuit.Voltage;
            Resistance = _circuit.Resistance;

            CompleteInitialization();
        }

        partial void OnVoltageChanged(PhysicalQuantity value)
        {
            if (IsInitialized)
            {
                _circuit.Voltage = value;
            }
        }

        partial void OnResistanceChanged(PhysicalQuantity value)
        {
            if (IsInitialized)
            {
                _circuit.Resistance = value;
            }
        }

        [RelayCommand]
        private void Clear()
        {
            Voltage = new PhysicalQuantity(0, PhysicalQuantityType.Voltage);
            Resistance = new PhysicalQuantity(0, PhysicalQuantityType.Resistance);
        }
    }
}