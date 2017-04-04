using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VkNet;

namespace VkClient
{
    abstract class VmBase : INotifyPropertyChanged
    {
        protected static readonly VkApi api = new VkApi();

        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event

        protected  void Set<T>(ref T field, T value, [CallerMemberName] string propName = null)
        {
            if (field != null && !field.Equals(value) || value != null && !value.Equals(field))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }
    }

    public class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly Action<object> action;

        public CommandBase(Action<object> action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
                return true;
        }

        public void Execute(object parameter)
        {
            action(parameter);
        }
    }

    


}
