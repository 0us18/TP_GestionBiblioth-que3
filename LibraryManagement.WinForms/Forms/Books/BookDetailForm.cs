using LibraryManagement.Application.Services;
using LibraryManagement.Core.Entities;

namespace LibraryManagement.WinForms.Forms.Books;

public partial class BookDetailForm : Form
{
    private readonly BookService _bookService;
    public int? BookId { get; set; }

    private TextBox _txtTitle;
    private TextBox _txtISBN;
    private NumericUpDown _numYear;
    private NumericUpDown _numCopies;

    public BookDetailForm(BookService bookService)
    {
        _bookService = bookService;
        InitializeComponent();
        this.Load += async (s, e) => await LoadBookData();
    }

    private void InitializeComponent()
    {
        this.Text = "Détail du Livre";
        this.Size = new Size(400, 350);
        this.StartPosition = FormStartPosition.CenterParent;

        var lblTitle = new Label { Text = "Titre:", Location = new Point(20, 20) };
        _txtTitle = new TextBox { Location = new Point(120, 20), Width = 200 };

        var lblISBN = new Label { Text = "ISBN:", Location = new Point(20, 60) };
        _txtISBN = new TextBox { Location = new Point(120, 60), Width = 200 };

        var lblYear = new Label { Text = "Année:", Location = new Point(20, 100) };
        _numYear = new NumericUpDown { Location = new Point(120, 100), Width = 100, Maximum = 3000, Minimum = 1000, Value = DateTime.Now.Year };

        var lblCopies = new Label { Text = "Copies:", Location = new Point(20, 140) };
        _numCopies = new NumericUpDown { Location = new Point(120, 140), Width = 100, Minimum = 0 };

        var btnSave = new Button { Text = "Enregistrer", Location = new Point(120, 200), DialogResult = DialogResult.None };
        btnSave.Click += async (s, e) => await SaveBook();

        var btnCancel = new Button { Text = "Annuler", Location = new Point(220, 200), DialogResult = DialogResult.Cancel };

        this.Controls.AddRange(new Control[] { lblTitle, _txtTitle, lblISBN, _txtISBN, lblYear, _numYear, lblCopies, _numCopies, btnSave, btnCancel });
    }

    private async Task LoadBookData()
    {
        if (BookId.HasValue)
        {
            var book = await _bookService.GetBookByIdAsync(BookId.Value);
            if (book != null)
            {
                _txtTitle.Text = book.Title;
                _txtISBN.Text = book.ISBN;
                _numYear.Value = book.PublicationYear;
                _numCopies.Value = book.AvailableCopies;
            }
        }
    }

    private async Task SaveBook()
    {
        try
        {
            var book = new Book
            {
                Title = _txtTitle.Text,
                ISBN = _txtISBN.Text,
                PublicationYear = (int)_numYear.Value,
                AvailableCopies = (int)_numCopies.Value
            };

            if (BookId.HasValue)
            {
                book.BookId = BookId.Value;
                await _bookService.UpdateBookAsync(book);
            }
            else
            {
                await _bookService.AddBookAsync(book);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
