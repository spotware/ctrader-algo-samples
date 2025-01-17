using System;
using cAlgo.API;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None, IsOverlay = true)]
    public class OpenFolderDialogSample : Indicator
    {
        private OpenFolderDialog _openFolderDialog;

        protected override void Initialize()
        {
            _openFolderDialog = new OpenFolderDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Title = "My Open Folder Dialog Title"
            };

            var showOpenFolderDialog = new Button { Text = "Show Open Folder Dialog" };
            showOpenFolderDialog.Click += showOpenFolderDialog_Click;

            var panel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            panel.AddChild(showOpenFolderDialog);
            Chart.AddControl(panel);
        }

        private void showOpenFolderDialog_Click(ButtonClickEventArgs obj)
        {
            var result = _openFolderDialog.ShowDialog();
            Print($"Result: {result} | FolderName: {_openFolderDialog.FolderName}");
        }

        public override void Calculate(int index)
        {
        }
    }
}