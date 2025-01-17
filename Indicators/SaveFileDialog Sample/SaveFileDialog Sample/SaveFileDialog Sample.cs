using System;
using cAlgo.API;
using System.Text;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None, IsOverlay = true)]
    public class SaveFileDialogSample : Indicator
    {
        private SaveFileDialog _saveFileDialog;

        protected override void Initialize()
        {
            _saveFileDialog = new SaveFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Title = "My Save File Dialog Title"
            };

            var showSaveFileDialogText = new Button { Text = "Show Save File Dialog (Set Content as text)" };
            var showSaveFileDialogBytes = new Button { Text = "Show Save File Dialog (Set Content as bytes)" };

            showSaveFileDialogText.Click += showSaveFileDialogText_Click;
            showSaveFileDialogBytes.Click += showSaveFileDialogBytes_Click;

            var panel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            panel.AddChild(showSaveFileDialogText);
            panel.AddChild(showSaveFileDialogBytes);
            Chart.AddControl(panel);
        }

        private void showSaveFileDialogText_Click(ButtonClickEventArgs obj)
        {
            var result = _saveFileDialog.ShowDialog();
            Print($"Result: {result}");
            if (result != FileDialogResult.Cancel)
            {
                _saveFileDialog.WriteToFile("Test in text");
            }
        }

        private void showSaveFileDialogBytes_Click(ButtonClickEventArgs obj)
        {
            var result = _saveFileDialog.ShowDialog();
            Print($"Result: {result}");
            if (result != FileDialogResult.Cancel)
            {
                _saveFileDialog.WriteToFile(Encoding.UTF8.GetBytes("Test in bytes"));
            }
        }

        public override void Calculate(int index)
        {
        }
    }
}