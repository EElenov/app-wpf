using System;
using System.Collections.Generic;

namespace app_domain
{
    /// <summary>
    /// Design for type of work operations (loading, unloading, packing, etc.)
    /// </summary>
    public sealed class DummyDesign
    {
        public string Id;
        public string Name;
        public string Description;
        public string Tooltip;
        public DateTime CreationDate;
        public IEnumerable<DummyProgram> Programs;
    }
}
