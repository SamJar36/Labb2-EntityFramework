using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2_EntityFramework.ViewModel;

internal class ConfigurationViewModel
{
    private readonly MainWindowViewModel? mainWindowViewModel;

    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;
    }
}
