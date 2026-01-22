using LibraryManagement.Application.Services;
using LibraryManagement.Core.Entities;
using LibraryManagement.WinForms;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.WinForms.Forms.Books;

public partial class BookListForm : Form
{
    private readonly BookService _bookService;
    private DataGridView _gridBooks = null!;
    private TextBox _txtSearch = null!;
    private Button _btnEdit = null!;
    private Button _btnDelete = null!;

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

        var panelSearch = new Panel { Dock = DockStyle.Top, Height = 60 };
        _txtSearch = new TextBox { Location = new Point(20, 20), Width = 300, PlaceholderText = "Rechercher..." };
        var btnSearch = new Button { Text = "Rechercher", Location = new Point(330, 18) };
        btnSearch.Click += async (s, e) => await SearchBooks();
        
        var btnAdd = new Button { Text = "Nouveau Livre", Location = new Point(430, 18), Width = 120 };
        btnAdd.Click += (s, e) => OpenBookDetail(null);

        _btnEdit = new Button { Text = "Modifier", Location = new Point(560, 18), Width = 90 };
        _btnEdit.Click += (s, e) => EditSelectedBook();

        _btnDelete = new Button { Text = "Supprimer", Location = new Point(660, 18), Width = 90 };
        _btnDelete.Click += async (s, e) => await DeleteSelectedBook();

        panelSearch.Controls.Add(_txtSearch);
        panelSearch.Controls.Add(btnSearch);
        panelSearch.Controls.Add(btnAdd);
        panelSearch.Controls.Add(_btnEdit);
        panelSearch.Controls.Add(_btnDelete);

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
        var detailForm = Program.ServiceProvider.GetRequiredService<BookDetailForm>();
        detailForm.BookId = bookId;
        
        if (detailForm.ShowDialog() == DialogResult.OK)
        {
            LoadBooks();
        }
    }

    private int? GetSelectedBookId()
    {
        if (_gridBooks.SelectedRows.Count <= 0)
            return null;

        if (_gridBooks.SelectedRows[0].DataBoundItem is not Book book)
            return null;

        return book.BookId;
    }

    private void EditSelectedBook()
    {
        var id = GetSelectedBookId();
        if (!id.HasValue)
        {
            MessageBox.Show("Veuillez sélectionner un livre.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        OpenBookDetail(id.Value);
    }

    private async Task DeleteSelectedBook()
    {
        var id = GetSelectedBookId();
        if (!id.HasValue)
        {
            MessageBox.Show("Veuillez sélectionner un livre.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirm = MessageBox.Show("Confirmer la suppression du livre sélectionné ?", "Confirmation",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes)
            return;

        try
        {
            await _bookService.DeleteBookAsync(id.Value);
            MessageBox.Show("Livre supprimé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadBooks();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Suppression impossible", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
