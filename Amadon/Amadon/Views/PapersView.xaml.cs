using Amadon.Classes;
using UbStandardObjects;
using UbStandardObjects.Objects;

namespace Amadon.Views;

public partial class PapersView : ContentView
{
    public enum PaperShowType
    {
        LeftOnly = 0,
        LeftRight = 1,
        LeftMiddleRight = 2,
        LeftRightCompare = 3,
        LeftMiddleRightCompare = 4
    }

    // Possibilities: only left column, left and right, left - middle - right, left - middle - right - compare

    private int NoRows = 0;  // Number of lines already inserted in the table

    private readonly int ColLeft = 0;

    private GetDataFilesMAUI GetDataFiles = null;  // Object to get data from disk (repositories)

    private int ColRight
    {
        get
        {
            switch(ColumnsType)
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

    public PaperShowType ColumnsType { get; set;} = PaperShowType.LeftRight;

    
    public PapersView()
	{
		InitializeComponent();
        GetDataFiles = new GetDataFilesMAUI((ParametersMAUI)StaticObjects.Parameters);
    }

    private void InitializeGrid()
	{
        //GridText.Clear();
        GridText.RowDefinitions.Clear();
        GridText.ColumnDefinitions.Clear();
        NoRows = 0;
        GridText.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

        switch (ColumnsType)
        {
            case PaperShowType.LeftOnly:
                GridText.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                break;
            case PaperShowType.LeftRight:
                GridText.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                GridText.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                break;

            case PaperShowType.LeftMiddleRight:
            case PaperShowType.LeftRightCompare:
                GridText.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                GridText.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                GridText.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                break;
            case PaperShowType.LeftMiddleRightCompare:
                GridText.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                GridText.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                GridText.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                GridText.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                break;
        }
    }

    private void CreateLabel(string html, int row, int col)
    {
        Label label = new Label();
        label.WidthRequest = 200;
        label.HeightRequest = 40;
        label.Text = html;
        GridText.Add(label, row, col);
    }

    private void FillRow(string htmlTextLeft, string htmlTextMiddle= null, string htmlTextRight= null, string htmlCompare = null)
	{
        GridText.RowDefinitions.Add(new RowDefinition());
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


    public void ShowPaper(TOC_Entry entry, PaperShowType showType, bool showCompare= false)
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
            Paragraph pMiddle = paperMiddle != null ? paperMiddle.GetParagraph(entry) : null;
            Paragraph pRight = paperRight != null ? paperRight.GetParagraph(entry) : null;
            FillRow(pLeft, pMiddle, pRight, showCompare);
        }

    }

}