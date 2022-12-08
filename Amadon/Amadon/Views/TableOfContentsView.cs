using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using System.Reflection.PortableExecutable;
using System.Transactions;
using UbStandardObjects;
using UbStandardObjects.Objects;

namespace Amadon.Views;

public class TableOfContentsView : ContentView
{
    public TableOfContentsView()
    {
        Content = new VerticalStackLayout
        {
            Spacing = 12,

            Children =
            {
                new Label { Text = "English" },
                CreateMenu("Part I", 0, 31),
                CreateMenu("Part II", 32, 56),
                CreateMenu("Part III", 56, 119),
                CreateMenu("Part IV", 120, 196)
            }
        };
    }


    public class HyperlinkUI : Span
    {
        public static readonly BindableProperty LinkUrlProperty = BindableProperty.Create(nameof(LinkUrl), typeof(string), typeof(HyperlinkUI), null); 

        public string LinkUrl
        {
            get
            {
                return (string)GetValue(LinkUrlProperty);
            }
            set
            {
                SetValue(LinkUrlProperty, value);
            }
        }

        public HyperlinkUI()
        {
            ApplyHyperlinkAppearance();
        }

        private void ApplyHyperlinkAppearance()
        {
            this.TextColor = Color.FromArgb("#0000EE");
            this.TextDecorations = TextDecorations.Underline;
        }

        private void CreateNavgigationCommand()
        {
            //... Since Span inherits GestureElement, you can add Gesture Recognizer to navigate using LinkUrl
        }
    }

    private Label CreateLabel(TOC_Entry entry)
    {
        Label label = new()
        {
            FormattedText = entry.Ident,
            Margin = new Thickness(40, 10, 10, 10)
        };
        return label;
    }


    private Expander CreateMenu(string title, short startPaper, short endPaper)
    {
        VerticalStackLayout layout = new VerticalStackLayout();
        Expander expanderPaper = null;
        foreach (TOC_Entry entry in StaticObjects.Book.LeftTranslation.TableOfContents.FindAll(e => e.Paper >= startPaper && e.Paper <= endPaper))
        {
            if (entry.Section == 0 && entry.ParagraphNo == 0)
            {
                VerticalStackLayout layoutPaper = new VerticalStackLayout();
                foreach (TOC_Entry entryPaper in StaticObjects.Book.LeftTranslation.TableOfContents.FindAll(e => e.Paper == entry.Paper && e.ParagraphNo == 0))
                {
                    layoutPaper.Children.Add(CreateLabel(entryPaper));
                }
                expanderPaper = new Expander()
                {
                    Header = new Label { Text = entry.Ident },
                    Margin= new Thickness(40, 10, 10, 10),
                    Content = layoutPaper
                };
                layout.Children.Add(expanderPaper);
            }
        }

        Expander expander = new Expander
        {
            Header = new Label { Text = title },
            Margin = new Thickness(40, 10, 10, 10),
            Content = layout
        };

        return expander;
    }

}