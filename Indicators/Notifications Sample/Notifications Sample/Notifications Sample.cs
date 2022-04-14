using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample indicator shows how to use API notifications to play sound or send an email
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class NotificationsSample : Indicator
    {
        private int _lastNotifiedBarIndex;

        [Parameter("Sound File Path", DefaultValue = "C:\\Windows\\Media\\notify.wav")]
        public string SoundFilePath { get; set; }

        [Parameter("Sender Email")]
        public string SenderEmail { get; set; }

        [Parameter("Receiver Email")]
        public string ReceiverEmail { get; set; }

        protected override void Initialize()
        {
        }

        public override void Calculate(int index)
        {
            if (!IsLastBar || _lastNotifiedBarIndex == index)
                return;

            _lastNotifiedBarIndex = index;

            if (Bars.Last(1).Close > Bars.Last(1).Open)
            {
                Notify("Up Bar Closed");
            }
            else if (Bars.Last(1).Close < Bars.Last(1).Open)
            {
                Notify("Down Bar Closed");
            }
        }

        private void Notify(string message)
        {
            if (!string.IsNullOrWhiteSpace(SoundFilePath))
            {
                Notifications.PlaySound(SoundFilePath);
            }

            if (!string.IsNullOrWhiteSpace(SenderEmail) && !string.IsNullOrWhiteSpace(ReceiverEmail))
            {
                Notifications.SendEmail(SenderEmail, ReceiverEmail, "Notification", message);
            }
        }
    }
}
