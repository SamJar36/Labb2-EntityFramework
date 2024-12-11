using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labb2_EntityFramework;
using Labb2_EntityFramework.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Labb2_EntityFramework.ViewModel;

internal class MainWindowViewModel : ViewModelBase
{
    private Butiker _selectedStore;
    public Butiker SelectedStore
    {
        get => _selectedStore;
        set
        {
            _selectedStore = value;
            RaisePropertyChanged();
        }
    }
    public ObservableCollection<Böcker> Books { get; set; }
    public ObservableCollection<Butiker> Stores { get; set; }
    public ObservableCollection<Författare> Authors { get; set; }
    public ObservableCollection<Lagersaldo>  Inventories { get; set; }
    public ObservableCollection<BonuspoängKonto> BonusPointAccounts { get; set; }

    public MainWindowViewModel()
    {
        using (var context = new BokhandelContext())
        {
            this.Books = new ObservableCollection<Böcker>(context.Böckers.ToList());
            this.Stores = new ObservableCollection<Butiker>(context.Butikers.ToList());
            this.Authors = new ObservableCollection<Författare>(context.Författares.ToList());
            this.Inventories = new ObservableCollection<Lagersaldo>(context.Lagersaldos.ToList());
            this.BonusPointAccounts = new ObservableCollection<BonuspoängKonto>(context.BonuspoängKontos.ToList());

            this.SelectedStore = Stores.FirstOrDefault();
        }
    }
    public void AddBooks()
    {

    }
    public void RemoveBooks()
    {

    }
}

