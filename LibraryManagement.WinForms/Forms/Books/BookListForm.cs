using LibraryManagement.Application.Services;
using LibraryManagement.Core.Entities;
using LibraryManagement.WinForms;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.WinForms.Forms.Books;

public partial class BookListForm : Form
{
    private readonly BookService _bookService;
    private DataGridView _gridBooks;
    private TextBox _txtSearch;

    public BookListForm(BookService bookService)
    {
        _bookService = bookService;
        InitializeComponent();
        LoadBooks();
    }

    private void InitializeComponent()
    {
        this.Text = "Gestion des Livres";
        this.Size = new Size(800, 500);

        // Search Panel
        var panelSearch = new Panel { Dock = DockStyle.Top, Height = 60 };
        _txtSearch = new TextBox { Location = new Point(20, 20), Width = 300, PlaceholderText = "Rechercher..." };
        var btnSearch = new Button { Text = "Rechercher", Location = new Point(330, 18) };
        btnSearch.Click += async (s, e) => await SearchBooks();
        
        var btnAdd = new Button { Text = "Nouveau Livre", Location = new Point(650, 18), Width = 120 };
        btnAdd.Click += (s, e) => OpenBookDetail(null);

        panelSearch.Controls.Add(_txtSearch);
        panelSearch.Controls.Add(btnSearch);
        panelSearch.Controls.Add(btnAdd);

        // Grid
        _gridBooks = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true
        };
        _gridBooks.DoubleClick += (s, e) => 
        {
            if (_gridBooks.SelectedRows.Count > 0)
            {
                var book = (Book)_gridBooks.SelectedRows[0].DataBoundItem;
                OpenBookDetail(book.BookId);
            }
        };

        this.Controls.Add(_gridBooks);
        this.Controls.Add(panelSearch);
    }

    private async void LoadBooks()
    {
        var books = await _bookService.GetAllBooksAsync();
        _gridBooks.DataSource = books.ToList();
    }

    private async Task SearchBooks()
    {
        var books = await _bookService.SearchBooksAsync(_txtSearch.Text);
        _gridBooks.DataSource = books.ToList();
    }

    private void OpenBookDetail(int? bookId)
    {
        // Resolve BookDetailForm from DI container manually for this factory pattern
        // In real app, use a factory or DI provider
        var detailForm = Program.ServiceProvider.GetRequiredService<BookDetailForm>();
        detailForm.BookId = bookId;
        
        if (detailForm.ShowDialog() == DialogResult.OK)
        {
            LoadBooks();
        }
    }
}
