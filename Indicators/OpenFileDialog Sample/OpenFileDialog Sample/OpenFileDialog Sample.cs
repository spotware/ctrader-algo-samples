using System;
using cAlgo.API;
using System.Text;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None, IsOverlay = true)]
    public class OpenFileDialogSample : Indicator
    {
        private OpenFileDialog _openFileDialog;

        protected override void Initialize()
        {
            _openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Multiselect = true,
                Title = "My Open File Dialog Title"
            };

            var showOpenFileDialog = new Button { Text = "Show Open File Dialog" };
            showOpenFileDialog.Click += showOpenFileDialog_Click;

            var panel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            panel.AddChild(showOpenFileDialog);
            Chart.AddControl(panel);
        }

        private void showOpenFileDialog_Click(ButtonClickEventArgs obj)
        {
            var result = _openFileDialog.ShowDialog();
            Print($"Result: {result} | FileName: {_openFileDialog.FileName} | FileNames: {string.Join(',', _openFileDialog.FileNames)}");
        }

        public override void Calculate(int index)
        {
        }
    }
}