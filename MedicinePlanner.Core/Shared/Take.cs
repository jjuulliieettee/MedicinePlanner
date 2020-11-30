using System;

namespace MedicinePlanner.Core.Shared
{
    public class Take
    {
        public DateTimeOffset TimeFrom { get; set; }

        public DateTimeOffset TimeTo { get; set; }

        public string Description { get; set; }
    }
}
