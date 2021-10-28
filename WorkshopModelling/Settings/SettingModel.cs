namespace WorkshopModelling.Settings
{
    public class SettingModel
    {
        public int Time { get; set; }

        public int DetailsCount { get; set; }

        public int Threshold { get; set; }

        public ActivitySetting GetDetailActivity { get; set; }

        public ActivitySetting OrderActivity { get; set; }

        public ActivitySetting ComplectDetailsActivity { get; set; }

        public ActivitySetting DeliverActivity { get; set; }
    }
}
