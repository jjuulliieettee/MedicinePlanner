using System;
using System.Collections.Generic;

namespace MedicinePlanner.Core.Shared
{
    public class Day
    {
        public DateTimeOffset Date { get; set; }

        public List<Take> Takes { get; set; }
    }
}
