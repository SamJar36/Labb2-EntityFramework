﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2_EntityFramework.ViewModel;

internal class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? mainWindowViewModel;

    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;
    }
    public void AddBooks()
    {

    }
    public void RemoveBooks()
    {

    }
}
