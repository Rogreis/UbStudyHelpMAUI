using Amadon.Classes;
using UbStandardObjects;
using UbStandardObjects.Objects;
using static Amadon.Views.PapersView;

namespace Amadon;

public partial class TextPage : ContentPage
{
    private int NoRows = 0;  // Number of lines already inserted in the table

    private readonly int ColLeft = 0;

    private GetDataFilesMAUI GetDataFiles = null;  // Object to get data from disk (repositories)
    
    public TextPage()
	{
		InitializeComponent();
        EventsControl.TOCClicked += EventsControl_TOCClicked;
        GetDataFiles = new GetDataFilesMAUI((ParametersMAUI)StaticObjects.Parameters);
    }


    private int ColRight
    {
        get
        {
            switch (ColumnsType)
            {
                case PaperShowType.LeftRightCompare:
                case PaperShowType.LeftRight:
                    return 1;
                case PaperShowType.LeftMiddleRightCompare:
                case PaperShowType.LeftMiddleRight:
                    return 2;
            }
            return -1;
        }
    }

    private int ColMiddle
    {
        get
        {
            switch (ColumnsType)
            {
                case PaperShowType.LeftMiddleRightCompare:
                case PaperShowType.LeftMiddleRight:
                    return 1;
            }
            return -1;
        }
    }

    private int ColCompare
    {
        get
        {
            switch (ColumnsType)
            {
                case PaperShowType.LeftRightCompare:
                    return 2;
                case PaperShowType.LeftMiddleRightCompare:
                    return 3;
            }
            return -1;
        }
    }

    public PaperShowType ColumnsType { get; set; } = PaperShowType.LeftRight;



    private void InitializeGrid()
    {
        //TextGrid.Clear();
        TextGrid.RowDefinitions.Clear();
        TextGrid.ColumnDefinitions.Clear();
        NoRows = 0;
        TextGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

        switch (ColumnsType)
        {
            case PaperShowType.LeftOnly:
                break;
            case PaperShowType.LeftRight:
                TextGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                break;

            case PaperShowType.LeftMiddleRight:
            case PaperShowType.LeftRightCompare:
                TextGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                TextGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                break;
            case PaperShowType.LeftMiddleRightCompare:
                TextGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                TextGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                TextGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                break;
        }
    }


    private void CreateLabel(string html, int row, int col)
    {
        Label label = new Label();
        label.Padding = new Thickness(15);
        label.Text = html;
        TextGrid.Add(label, col, row);
    }

    private void FillRow(string htmlTextLeft, string htmlTextMiddle = null, string htmlTextRight = null, string htmlCompare = null)
    {
        TextGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        CreateLabel(htmlTextLeft, NoRows, ColLeft);
        if (htmlTextMiddle != null && ColMiddle >= 0) CreateLabel(htmlTextMiddle, NoRows, ColMiddle);
        if (htmlTextRight != null && ColRight >= 0) CreateLabel(htmlTextRight, NoRows, ColRight);
        if (htmlCompare != null && ColRight >= 0) CreateLabel(htmlCompare, NoRows, ColCompare);
        NoRows++;
    }

    private string FormatMAUILabelText(Paragraph p)
    {
        if (p == null) return null;
        return p.Text;
    }

    private void FillRow(Paragraph pLeft, Paragraph pMiddle = null, Paragraph pRight = null, bool showCompare = false)
    {
        FillRow(FormatMAUILabelText(pLeft), FormatMAUILabelText(pMiddle), FormatMAUILabelText(pRight), null);
    }


    private void ShowPaper(TOC_Entry entry, PaperShowType showType, bool showCompare = false)
    {
        ColumnsType = showType;
        InitializeGrid();

        Translation transLeft = GetDataFiles.GetTranslation(StaticObjects.Parameters.LanguageIDLeftTranslation);
        // Translation left must not be null
        if (transLeft == null)
        {
            EventsControl.FireSendMessage($"Non existing translation: {StaticObjects.Parameters.LanguageIDLeftTranslation}");
            return;
        }
        Paper paperLeft = transLeft.Paper(entry.Paper);
        if (transLeft == null)
        {
            EventsControl.FireSendMessage($"Non existing translation-paper: {StaticObjects.Parameters.LanguageIDLeftTranslation}-{entry.Paper}");
            return;
        }

        Translation transMiddle = StaticObjects.Parameters.LanguageIDMiddleTranslation < 0 ? null : GetDataFiles.GetTranslation(StaticObjects.Parameters.LanguageIDMiddleTranslation);
        Paper paperMiddle = transMiddle != null ? transMiddle.Paper(entry.Paper) : null;
        Translation transRight = StaticObjects.Parameters.LanguageIDRightTranslation < 0 ? null : GetDataFiles.GetTranslation(StaticObjects.Parameters.LanguageIDRightTranslation);
        Paper paperRight = transRight != null ? transRight.Paper(entry.Paper) : null;

        foreach (Paragraph pLeft in paperLeft.Paragraphs)
        {
            Paragraph pMiddle = paperMiddle != null ? paperMiddle.GetParagraph(pLeft.Entry) : null;
            Paragraph pRight = paperRight != null ? paperRight.GetParagraph(pLeft.Entry) : null;
            FillRow(pLeft, pMiddle, pRight, showCompare);
        }

    }


    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        ShowPaper(StaticObjects.Parameters.Entry, PaperShowType.LeftRight);
    }

    private void EventsControl_TOCClicked(TOC_Entry entry)
    {
        ShowPaper(entry, PaperShowType.LeftRight);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        ShowPaper(StaticObjects.Parameters.Entry, PaperShowType.LeftRight);
    }
}