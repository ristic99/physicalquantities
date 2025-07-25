using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;
using PhysicalQuantities.Core.Enums;
using PhysicalQuantities.Core.Physics;

namespace PhysicalQuantities.Core.Behaviors
{
    /// <summary>
    /// Simple behavior with red border when empty
    /// </summary>
    public class PhysicalQuantityBehavior : Behavior<TextBox>
    {
        private bool _isUpdating = false;

        // Updated to work with nullable struct
        public static readonly DependencyProperty QuantityProperty =
            DependencyProperty.Register(nameof(Quantity), typeof(PhysicalQuantity?), typeof(PhysicalQuantityBehavior),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnQuantityChanged));

        public static readonly DependencyProperty DisplayUnitProperty =
            DependencyProperty.Register(nameof(DisplayUnit), typeof(UnitPrefix), typeof(PhysicalQuantityBehavior),
                new PropertyMetadata(UnitPrefix.Base, OnDisplayPrefixChanged));

        public PhysicalQuantity? Quantity
        {
            get => (PhysicalQuantity?)GetValue(QuantityProperty);
            set => SetValue(QuantityProperty, value);
        }

        public UnitPrefix DisplayUnit
        {
            get => (UnitPrefix)GetValue(DisplayUnitProperty);
            set => SetValue(DisplayUnitProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.TextChanged += OnTextChanged;
            UpdateDisplay();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject != null)
                AssociatedObject.TextChanged -= OnTextChanged;
        }

        private static void OnQuantityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PhysicalQuantityBehavior { _isUpdating: false } behavior)
            {
                behavior.UpdateDisplay();
            }
        }

        private static void OnDisplayPrefixChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PhysicalQuantityBehavior behavior)
            {
                behavior.UpdateDisplay();
            }
        }

        private void UpdateDisplay()
        {
            if (AssociatedObject == null || !Quantity.HasValue)
            {
                if (AssociatedObject != null)
                {
                    AssociatedObject.Text = string.Empty;
                }
                return;
            }

            _isUpdating = true;
            try
            {
                var displayValue = Quantity.Value.GetValueIn(DisplayUnit);
                AssociatedObject.Text = displayValue.ToString("G6");
                ClearErrorState();
            }
            finally
            {
                _isUpdating = false;
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdating)
                return;

            // Show red border if empty
            if (string.IsNullOrWhiteSpace(AssociatedObject.Text))
            {
                ShowErrorState();
                return;
            }

            if (double.TryParse(AssociatedObject.Text, out double value))
            {
                _isUpdating = true;
                try
                {
                    // Create quantity with display prefix, then convert to base for storage
                    var quantityType = Quantity?.Type ?? PhysicalQuantityType.Voltage;
                    Quantity = new PhysicalQuantity(value, quantityType, prefix: DisplayUnit);
                    ClearErrorState();
                }
                finally
                {
                    _isUpdating = false;
                }
            }
            else
            {
                // Invalid number - show red border
                ShowErrorState();
            }
        }

        private void ShowErrorState()
        {
            // Red border for empty or invalid input
            AssociatedObject.BorderBrush = System.Windows.Media.Brushes.Red;
            AssociatedObject.BorderThickness = new Thickness(2);
        }

        private void ClearErrorState()
        {
            // Reset to default appearance
            AssociatedObject.ClearValue(Control.BorderBrushProperty);
            AssociatedObject.ClearValue(Control.BorderThicknessProperty);
        }
    }
}