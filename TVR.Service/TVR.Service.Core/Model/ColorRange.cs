namespace TVR.Service.Core.Model
{
    public struct ColorRange
    {
        public HsvColor Minimum { get; set; }

        public HsvColor Maximum { get; set; }

        public ColorRange(HsvColor minimum, HsvColor maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }
    }
}
