namespace TVR.Service.Core.Model
{
    public struct ColorProfile
    {
        public ColorRange[] Ranges { get; set; }

        public ColorProfile(ColorRange[] ranges)
        {
            Ranges = ranges;
        }
    }
}
