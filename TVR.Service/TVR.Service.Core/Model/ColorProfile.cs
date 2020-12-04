namespace TVR.Service.Core.Model
{
    public struct ColorProfile
    {
        public TrackerColor Color { get; set; }

        public ColorRange[] Ranges { get; set; }

        public ColorProfile(TrackerColor color, ColorRange[] ranges)
        {
            Color = color;
            Ranges = ranges;
        }
    }
}
