using System;
using System.Collections.Generic;

namespace app_domain
{
    /// <summary>
    /// Program for workflow configuration.
    /// </summary>
    public sealed class DummyProgram
    {
        public string Id;
        public string Name;
        public string Description;
        public string Tooltip;
        public DateTime CreationDate;
        public IEnumerable<DummyParameter> Parameters;
        //another layer of nesting for list of machies and its parameters here
    }
}
