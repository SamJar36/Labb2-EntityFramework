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

namespace Labb2_EntityFramework.ViewModel;

internal class MainWindowViewModel : ViewModelBase
{
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
    public ObservableCollection<Böcker> Books { get; private set; }
    private Böcker _selectedBook;
    public Böcker SelectedBook
    {
        get => _selectedBook;
        set
        {
            _selectedBook = value;
            RaisePropertyChanged(nameof(SelectedBook));
        }
    }
    public ObservableCollection<inventorySummary> Inventories { get; private set; }

    public MainWindowViewModel()
    {
        LoadStores();
        LoadBooks();
    }
    public void AddBooks()
    {

    }
    public void RemoveBooks()
    {

    }
    private void LoadBooks()
    {
        using var db = new BokhandelContext();

        Books = new ObservableCollection<Böcker>(
            db.Böckers
            .Distinct()
            .ToList());
        SelectedBook = Books.FirstOrDefault();
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

        Inventories = new ObservableCollection<inventorySummary>(
            db.Lagersaldos
            .Where(o => o.Butik == SelectedStore)
            .Select(o => new inventorySummary()
            {
                Book = o.IsbnNavigation.Titel,
                Author = o.IsbnNavigation.Författare.Förnamn,
                Units = o.Antal
            }
            ).ToList()
        );
    }
}

public class inventorySummary
{
    public string Book { get; set; }
    public string Author { get; set; }
    public int Units { get; set; }
}
