using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Labb2_EntityFramework;
using Labb2_EntityFramework.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Labb2_EntityFramework.Commands;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using Labb2_EntityFramework.Dialogs;
using System.Windows;

namespace Labb2_EntityFramework.ViewModel;

internal class MainWindowViewModel : ViewModelBase
{
    public DelegateCommand AddBooksCommand { get; }
    public DelegateCommand RemoveBooksCommand { get; }
    public ObservableCollection<Butiker> Stores { get; private set; }
    private Butiker _selectedStore;
    public Butiker SelectedStore
    {
        get => _selectedStore;
        set
        {
            _selectedStore = value;
            RaisePropertyChanged(nameof(SelectedStore));
            LoadInventory();
            RaisePropertyChanged(nameof(Inventories));
        }
    }
    public ObservableCollection<InventorySummary> Inventories { get; private set; }
    private InventorySummary _selectedBook;
    public InventorySummary SelectedBook
    {
        get => _selectedBook;
        set
        {
            _selectedBook = value;
            RaisePropertyChanged(nameof(SelectedBook));
            AddBooksCommand.RaiseCanExecuteChanged();
            RemoveBooksCommand.RaiseCanExecuteChanged();
        }
    }

    private string _unitsToChange;
    public string UnitsToChange
    {
        get => _unitsToChange;
        set
        {
            _unitsToChange = value;
            RemoveBooksCommand.RaiseCanExecuteChanged();
            AddBooksCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(UnitsToChange));
         }
    }

    public MainWindowViewModel()
    {
        LoadStores();
        AddBooksCommand = new DelegateCommand(DoAddBooks, CanAddBooks);
        RemoveBooksCommand = new DelegateCommand(DoRemoveBooks, CanRemoveBooks);
    }
    private bool CanAddBooks(object? obj) => 
        UnitsToChange != null 
        && UnitsToChange.Length > 0 
        && UnitsToChange.All(char.IsDigit) 
        && SelectedBook != null;
    private void DoAddBooks(object obj)
    {
        using var db = new BokhandelContext();

        int unitsParsed = Int32.Parse(UnitsToChange);
        var lagersaldo = db.Lagersaldos.First(i => i.ButikId == SelectedStore.Id && i.Isbn == SelectedBook.Isbn);
        lagersaldo.Antal += unitsParsed;
        if (lagersaldo.Antal >= 100)
        {
            lagersaldo.Antal = 100;
            InvalidUnitDialog popup = new InvalidUnitDialog();
            popup.ShowDialog();
        }
        db.SaveChanges();
        LoadInventory();
        RaisePropertyChanged(nameof(Inventories));
        AddBooksCommand.RaiseCanExecuteChanged();
    }
    private bool CanRemoveBooks(object? obj) => 
        UnitsToChange != null 
        && UnitsToChange.Length > 0 
        && UnitsToChange.All(char.IsDigit)
        && SelectedBook != null;
    private void DoRemoveBooks(object obj)
    {
        using var db = new BokhandelContext();

        int unitsParsed = Int32.Parse(UnitsToChange);
        var lagersaldo = db.Lagersaldos.FirstOrDefault(i => i.ButikId == SelectedStore.Id && i.Isbn == SelectedBook.Isbn);
        lagersaldo.Antal -= unitsParsed;
        if (lagersaldo.Antal <= 0)
        {
            lagersaldo.Antal = 0;
        }
        db.SaveChanges();
        LoadInventory();
        RaisePropertyChanged(nameof(Inventories));
        RemoveBooksCommand.RaiseCanExecuteChanged();
    }
    private void LoadStores()
    {
        using var db = new BokhandelContext();

        Stores = new ObservableCollection<Butiker>(
            db.Butikers
            .Distinct()
            .ToList());
        SelectedStore = Stores.FirstOrDefault();
    }
    public void LoadInventory()
    {
        using var db = new BokhandelContext();

        Inventories = new ObservableCollection<InventorySummary>(
            db.Lagersaldos
            .Where(o => o.Butik == SelectedStore)
            .Select(o => new InventorySummary()
            {
                Id = o.ButikId,
                Book = o.IsbnNavigation.Titel,
                Author = o.IsbnNavigation.Författare.Förnamn + " " + o.IsbnNavigation.Författare.Efternamn,
                Units = o.Antal,
                Isbn = o.IsbnNavigation.Isbn13
            }
            ).ToList()
        );
    }
}

public class InventorySummary
{
    public int Id { get; set; }
    public string Book { get; set; }
    public string Author { get; set; }
    public int Units { get; set; }
    public string Isbn { get; set; }
}
