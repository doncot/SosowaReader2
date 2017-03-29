using Prism.Commands;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SosowaReader.ViewModels
{
    public class SearchPageViewModel : ViewModelBase
    {
        private DelegateCommand searchCommand;
        public DelegateCommand SearchCommand
        {
            set { searchCommand = value; }
            get
            {
                return new DelegateCommand(async () => await SearchAsync());
            }
        }

        public async Task SearchAsync()
        {

        }
    }
}
