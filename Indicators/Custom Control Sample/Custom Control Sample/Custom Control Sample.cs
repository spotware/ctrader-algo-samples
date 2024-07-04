// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using System.Collections.Generic;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class CustomControlSample : Indicator
    {
        protected override void Initialize()
        {
            var comboBox = new ComboBox
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            comboBox.AddItem("Item 1");
            comboBox.AddItem("Item 2");
            comboBox.AddItem("Item 3");
            comboBox.AddItem("Item 4");
            comboBox.AddItem("Item 5");

            Chart.AddControl(comboBox);
        }

        public override void Calculate(int index)
        {
            // Calculate value at specified index
            // Result[index] = ...
        }
    }

    public class ComboBox : CustomControl
    {
        private TextBox _textBox;

        private Button _button;

        private Grid _itemsGrid;

        private StackPanel _panel;

        private readonly List<object> _items = new List<object>();

        private bool _isExpanded;

        public ComboBox()
        {
            _textBox = new TextBox
            {
                Width = 100,
                IsReadOnly = true,
                IsReadOnlyCaretVisible = false
            };

            _button = new Button
            {
                Text = "▼"
            };

            _button.Click += Button_Click;

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            stackPanel.AddChild(_textBox);
            stackPanel.AddChild(_button);

            _panel = new StackPanel
            {
                Orientation = Orientation.Vertical
            };

            _panel.AddChild(stackPanel);

            AddChild(_panel);
        }

        public void AddItem(object item)
        {
            _items.Add(item);
        }

        public bool RemoveItem(object item)
        {
            return _items.Remove(item);
        }

        private void Button_Click(ButtonClickEventArgs obj)
        {
            if (_itemsGrid != null)
                _panel.RemoveChild(_itemsGrid);

            if (_isExpanded)
            {
                _isExpanded = false;

                return;
            }

            _isExpanded = true;

            _itemsGrid = new Grid(_items.Count, 1);

            for (int i = 0; i < _items.Count; i++)
            {
                var item = _items[i];

                _itemsGrid.AddChild(new TextBlock
                {
                    Text = item.ToString()
                }, i, 0);
            }

            _panel.AddChild(_itemsGrid);
        }
    }
}
