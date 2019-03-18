using System;

namespace RabbitReplay.App.Options
{
    public class ReplayOptions : GlobalOptions
    {
        public ReplayOptions(Uri rabbitUri) : base(rabbitUri)
        {
        }
    }
}
