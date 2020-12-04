namespace TVR.Service.Core.Model
{
    public struct ColorRange
    {
        public HsvColor Min { get; set; }

        public HsvColor Max { get; set; }

        public ColorRange(HsvColor min, HsvColor max)
        {
            Min = min;
            Max = max;
        }
    }
}
