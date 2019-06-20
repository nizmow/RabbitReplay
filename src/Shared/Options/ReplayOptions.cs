using System;

namespace RabbitReplay.Shared.Options
{
    public class ReplayOptions : GlobalOptions
    {
        public ReplayOptions(Uri rabbitUri) : base(rabbitUri)
        {
        }
    }
}
