﻿using AmadonStandardLib.Helpers;
using AmadonStandardLib.InterchangeData;
using AmadonStandardLib.UbClasses;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AmadonStandardLib.Classes
{
    /// <summary>
    /// 
    /// Nuget from https://www.nuget.org/packages/Lucene.Net/4.8.0-beta00014
    /// Install-Package Lucene.Net -Version 4.8.0-beta00014
    /// https://www.byteblocks.com/Post/How-to-use-Lucene-In-Net-Core-project
    /// Packages used:
    /// https://www.nuget.org/packages/Lucene.Net.Analysis.Common/
    /// https://www.nuget.org/packages/Lucene.Net.QueryParser/
    /// https://www.byteblocks.com/Post/How-to-use-Lucene-In-Net-Core-project
    /// 
    /// 
    /// 
    /// 
    /// <see href="https://www.oreilly.com/library/view/windows-developer-power/0596527543/ch04s04.html"/>
    /// https://www.thebestcsharpprogrammerintheworld.com/2017/12/12/how-to-create-and-search-a-lucene-net-index-in-4-simple-steps-using-c-step-1/
    /// https://csharp.hotexamples.com/examples/Lucene.Net.Documents/Field/-/php-field-class-examples.html
    /// https://www.oreilly.com/library/view/windows-developer-power/0596527543/ch04s04.html
    /// Conceitos:
    /// https://www.codeguru.com/dotnet/introduction-to-lucene-net/
    /// Search exmple
    /// https://stackoverflow.com/questions/30519036/lucene-net-how-to-retrieve-a-single-document
    /// Good search example
    /// http://www.sebastianmihai.com/lucene-dot-net.html
    /// https://tonytruong.net/better-searches-with-lucene-net/
    /// https://developpaper.com/using-lucene-net-to-do-a-simple-search-engine-full-text-index/
    /// https://csharp.hotexamples.com/pt/examples/Lucene.Net.Search/IndexSearcher/Doc/php-indexsearcher-doc-method-examples.html
    /// 
    /// Exemplos de search string
    /// https://www.codeproject.com/Articles/609980/Small-Lucene-NET-Demo-App
    /// 
    /// Apache doc
    /// https://lucenenet.apache.org/docs/4.8.0-beta00014/api/demo/Lucene.Net.Demo.html
    /// 
    /// </summary>
    public class LuceneBookSearch : LuceneBase
    {

        // Fields for Lucene Index Searc
        private const string FieldId = "Id";
        private const string FieldPaper = "Paper";
        private const string FieldSection = "Section";
        private const string FieldParagraph = "ParagraphNo";
        private const string FieldText = "Text";

        public string ErrorMessage = "";


        public LuceneBookSearch()
        {
        }


        private bool CreateUBIndex(SearchData data)
        {
            IndexPath = Path.Combine(data.IndexPathRoot, $"T{data.Translation.LanguageID:000}");
            System.IO.Directory.CreateDirectory(IndexPath);
            if (System.IO.Directory.GetFiles(IndexPath, "*.*").Length > 4)
            {
                return true;
            }

            FireSendMessage($"Recreating search index from {data.Translation.Description}");

            try
            {
                if (data == null || data.Translation == null)
                {
                    StaticObjects.Logger.Error("Search error: Data or Translation null");
                    FireSendMessage("Search error: Data or Translation null");
                    return false;
                }
                if (!System.IO.Directory.Exists(IndexPath))
                {
                    System.IO.Directory.CreateDirectory(IndexPath);
                }
                luceneIndexDirectory = FSDirectory.Open(IndexPath);


                Analyzer analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
                IndexWriterConfig config = new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer);
                IndexWriter writer = new IndexWriter(luceneIndexDirectory, config);

                for (short paperNo = 0; paperNo <= 196; paperNo++)
                {
                    Paper paper = data.Translation.Paper(paperNo);
                    FireSendMessage($"Indexing paper {paperNo}");
                    foreach (Paragraph paragraph in paper.Paragraphs)
                    {
                        if (!paragraph.IsDivider)
                        {
                            Document doc = new Document();
                            doc.Add(new StringField(FieldId, paragraph.ID, Field.Store.YES));
                            doc.Add(new StringField(FieldPaper, paragraph.Paper.ToString(), Field.Store.YES));
                            doc.Add(new StringField(FieldSection, paragraph.Section.ToString(), Field.Store.YES));
                            doc.Add(new StringField(FieldParagraph, paragraph.ParagraphNo.ToString(), Field.Store.YES));
                            doc.Add(new TextField(FieldText, paragraph.Text, Field.Store.YES));
                            writer.AddDocument(doc);
                        }
                    }
                }
                writer.Flush(triggerMerge: false, applyAllDeletes: false);
                writer.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(IndexPath);
                directoryInfo.Delete(true);
                StaticObjects.Logger.Error("Creating Book Search Data for " + IndexPath, ex);
                FireSendMessage("Creating Book Search Data for ", ex);
                return false;
            }
        }



        public void Dispose()
        {
            luceneIndexDirectory.Dispose();
        }



        public void UpdateIndex(Paragraph paragraph)
        {
            // Open the index for writing
            Analyzer analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
            IndexWriterConfig config = new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer);
            IndexWriter indexWriter = new IndexWriter(luceneIndexDirectory, config);

            // Delete the old version of the document
            Term term = new Term(FieldParagraph, paragraph.ID);
            indexWriter.DeleteDocuments(term);

            Document doc = new Document();
            doc.Add(new StringField("id", paragraph.Paper.ToString(), Field.Store.YES));
            doc.Add(new StringField(FieldPaper, paragraph.Paper.ToString(), Field.Store.YES));
            doc.Add(new StringField(FieldSection, paragraph.Section.ToString(), Field.Store.YES));
            doc.Add(new StringField(FieldParagraph, paragraph.ParagraphNo.ToString(), Field.Store.YES));
            doc.Add(new TextField(FieldText, paragraph.Text, Field.Store.YES));

            // Add the new version of the document
            indexWriter.AddDocument(doc);

            // Commit the changes to the index
            indexWriter.Commit();

            // Close the index writer
            indexWriter.Dispose();
        }

        public bool Execute(SearchData searchData)
        {
            try
            {
                if (!CreateUBIndex(searchData))
                {
                    string message = "Book Index not created for " + IndexPath;
                    StaticObjects.Logger.NonFatalError(message);
                    FireSendMessage(message);
                }

                Analyzer analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);

                // How to query
                // http://www.lucenetutorial.com/lucene-query-syntax.html
                // https://lucene.apache.org/core/2_9_4/queryparsersyntax.html

                List<SearchResult> SearchResults = new List<SearchResult>();

                var parser = new QueryParser(LuceneVersion.LUCENE_48, FieldText, analyzer);
                Query searchQuery = parser.Parse(searchData.QueryString);

                // Store information to highlight searched words
                searchData.ExtractTerms(searchQuery);
                //searchData.SetSearchString(FieldText + ":", searchQuery.ToString());

                var reader = DirectoryReader.Open(FSDirectory.Open(IndexPath));
                IndexSearcher searcher = new IndexSearcher(reader);
                TopDocs hits = searcher.Search(searchQuery, 200);
                int results = hits.ScoreDocs.Length;
                for (int i = 0; i < results; i++)
                {
                    Document doc = searcher.Doc(hits.ScoreDocs[i].Doc);
                    short paper = Convert.ToInt16(doc.GetField(FieldPaper).GetStringValue());
                    if (searchData.CurrentPaperOnly)
                    {
                        if (paper != searchData.CurrentPaper)
                        {
                            continue;
                        }
                    }
                    else if (!searchData.IsPaperIncluded(paper))
                    {
                        continue;
                    }
                    string x1 = doc.GetField(FieldSection).GetStringValue();

                    short section = Convert.ToInt16(doc.GetField(FieldSection).GetStringValue());
                    short paragraphNo = Convert.ToInt16(doc.GetField(FieldParagraph).GetStringValue());
                    TOC_Entry entry = new TOC_Entry(searchData.Translation.LanguageID, paper, section, paragraphNo, 0, 0);
                    entry.Text = doc.GetField(FieldText).GetStringValue();
                    SearchResult searchResult = new SearchResult(entry);
                    searchResult.OriginalPosition = i;  // used to restore relevancy order after a sort by paragraph
                    searchData.SearchResults.Add(searchResult);
                }
                return true;
            }
            catch (Exception ex)
            {
                searchData.ErrorMessage = $"Error executing search: {ex.Message}";
                return false;
            }
        }


        //private void TestSearch(string query)
        //{
        //    SearchResults.Clear();
        //    try
        //    {
        //        Debug.WriteLine($"Testing query ({query})");
        //        if (!Execute(query))
        //        {
        //            Debug.WriteLine($"    ***** Error: {ErrorMessage}");
        //        }
        //        else
        //        {
        //            foreach(string word in SearchResults.Words)
        //            {
        //                Debug.WriteLine($"    {word}");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"    ***** Exception: {ex.Message}");
        //    }
        //    Debug.WriteLine("");
        //}


        ///// <summary>
        ///// Run some tests for searc
        ///// </summary>
        //public void SearchTest()
        //{
        //    // Simple searchs
        //    TestSearch("Jesus");
        //    TestSearch("\"Christ Michael\"");
        //    TestSearch("Jesus and God");
        //    TestSearch("\"My master delays his coming\" AND God");
        //    TestSearch("Adora???");
        //    TestSearch("Adora*");



        //    // Wildcards
        //    TestSearch("absolut?");
        //    TestSearch("absolut*");
        //    TestSearch("abso*e");


        //    // Similar Searches The default that is used if the parameter is not given is 0.5.*
        //    TestSearch("abso~");
        //    TestSearch("roam~0.8");

        //    // Proximity Searches
        //    TestSearch("\"Jesus God\"~10");
        //    TestSearch("\"Jesus God\"~2");

        //    // Boosting a Term By default, the boost factor is 1. Although the boost factor must be positive, it can be less than 1 (e.g. 0.2)

        //    TestSearch("Jesus^4 God");
        //    TestSearch("\"Jesus Christ\"^4 \"apostle Peter\"");





        //    // Boolean Operators
        //    //# OR
        //    TestSearch("\"Christ Michael\" OR Jesus");
        //    //# AND
        //    TestSearch("\"Christ Michael\" AND \"Jesus of Nazareth\"");

        //    //# +
        //    TestSearch("+Christ Nebadon");

        //    //# NOT
        //    TestSearch("\"Christ Michael\" NOT \"Jesus of Nazareth\"");

        //    //# -
        //    TestSearch("\"Christ Michael\" - \"Jesus of Nazareth\"");


        //    // Grouping
        //    TestSearch("(Christ OR Nebadon) AND Gabriel");


        //    // Field Grouping
        //    TestSearch("(+Buddha+\"Genghis Khan\")");

        //    // Escaping Special Characters + TestSearch("&& || ! ( ) { } [ ] ^ " ~ * ? : \");

        //    TestSearch("\\[Revealed");





        //}




    }
}
