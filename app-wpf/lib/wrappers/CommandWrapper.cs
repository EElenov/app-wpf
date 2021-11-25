using System.Windows.Input;

namespace app_wpf
{
    public class CommandWrapper
    {
        public ICommand Command { get; set; }
        public string Label { get; set; }
        public string Tooltip { get; set; }
        public bool IsEnabled { get; set; }
    }
}
