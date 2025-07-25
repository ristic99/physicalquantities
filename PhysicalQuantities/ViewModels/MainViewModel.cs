using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PhysicalQuantities.Core.Enums;
using PhysicalQuantities.Core.Physics;
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
        [ObservableProperty]
        private PhysicalQuantity _inputVoltage = new(12.0, PhysicalQuantityType.Voltage);

        [ObservableProperty]
        private PhysicalQuantity _inputResistance = new(6.0, PhysicalQuantityType.Resistance);

        [ObservableProperty]
        private PhysicalQuantity _resultCurrent;

        public MainViewModel()
        {
            CompleteInitialization();
            Calculate();
        }

        [RelayCommand]
        private void Clear()
        {
            InputVoltage = new PhysicalQuantity(0, PhysicalQuantityType.Voltage);
            InputResistance = new PhysicalQuantity(0, PhysicalQuantityType.Resistance);
            ResultCurrent = new PhysicalQuantity(0, PhysicalQuantityType.Current);
        }

        partial void OnInputVoltageChanged(PhysicalQuantity value)
        {
            if (IsInitialized)
                Calculate();
        }

        partial void OnInputResistanceChanged(PhysicalQuantity value)
        {
            if (IsInitialized)
                Calculate();
        }

        private void Calculate()
        {
            var a = new PhysicalQuantity(1, PhysicalQuantityType.Power, 1, UnitPrefix.Kilo);
            var b = new PhysicalQuantity(1, PhysicalQuantityType.Power, 1, UnitPrefix.Base);

            var e = new PhysicalQuantity(1, PhysicalQuantityType.Power, 2, UnitPrefix.Kilo);

            var volt = new PhysicalQuantity(1, PhysicalQuantityType.Voltage, 1, UnitPrefix.Base);
            var voltInMilivolts = new PhysicalQuantity(1000, PhysicalQuantityType.Voltage, 1, UnitPrefix.Milli);

            if (volt == voltInMilivolts)
            {
                MessageBox.Show("Equal");
            }

            var c = a - b;

            var test = DimensionalAnalysisEngine.FindAllWaysToCreate(PhysicalQuantityType.Capacitance);

            var voltage = new PhysicalQuantity(12.0, PhysicalQuantityType.Voltage);
            var time = new PhysicalQuantity(5.0, PhysicalQuantityType.Time);
            //var error = voltage + time; // COMPILE ERROR! Can't add voltage and time

            ResultCurrent = (InputVoltage / InputResistance);
        }
    }
}