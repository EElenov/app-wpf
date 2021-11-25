using System;

namespace app_domain
{
    public enum ParameterType
    {
        IsInteger,
        IsString,
        IsBoolean,
        IsCollection
    }
    /// <summary>
    /// Parameter controlling an aspect of the workflow.
    /// </summary>
    public class DummyParameter
    {
        public string Name;
        public string Tooltip;
        public object Data;//avoiding dynamic type, because parameters could be in the thousands for some machines
        public ParameterType ParamInterpreter;
        public DummyParameter() : this(ParameterType.IsString) { }
        public DummyParameter (ParameterType ParamInterpreter)
        {
            this.ParamInterpreter = ParamInterpreter;
        }
    }
}
