using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labb2_EntityFramework;

namespace Labb2_EntityFramework.ViewModel;

internal class MainWindowViewModel
{
    public ConfigurationViewModel configurationViewModel { get; }

    public MainWindowViewModel()
    {
        configurationViewModel = new ConfigurationViewModel(this);
    }
}

