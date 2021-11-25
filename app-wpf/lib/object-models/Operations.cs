using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using app_domain;

namespace app_wpf
{
    [Serializable]
    public class DummyDesignObject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tooltip { get; set; }
        public DateTime CreationDate { get; set; }
        [IgnoreDataMember]
        public bool IsChecked { get; set; }
    }
    [Serializable]
    public class DummyProgramObject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tooltip { get; set; }
        public DateTime CreationDate { get; set; }
        public IEnumerable<DummyParameterObject> Parameters { get; set; }
    }
    [Serializable]
    public class DummyParameterObject
    {
        public string Name { get; set; }
        public string Tooltip { get; set; }
        public object Data { get; set; }
        public ParameterType ParamInterpreter { get; set; }
    }
}
