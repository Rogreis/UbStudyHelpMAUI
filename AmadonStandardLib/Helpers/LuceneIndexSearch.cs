using AmadonStandardLib.Helpers;
using AmadonStandardLib.InterchangeData;
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

namespace AmadonStandardLib.Classes
{
    public class LuceneIndexSearch : LuceneBase
    {

        private const string IndexFieldName = "Entry";
        private const string IndexFieldData = "Data";

        public LuceneIndexSearch()
        {
        }

        public void Dispose()
        {
        }


        private bool CreateLuceneIndexForUBIndex(SearchIndexData searchIndexData)
        {
            try
            {

                IndexPath = Path.Combine(searchIndexData.IndexPathRoot, "IX");
                System.IO.Directory.CreateDirectory(IndexPath);
                if (System.IO.Directory.GetFiles(IndexPath, "*.*").Length > 4)
                {
                    LibraryEventsControl.FireSendMessage("UbIndex found.");
                    return true;
                }
                LibraryEventsControl.FireSendMessage($"Recreating index");

                luceneIndexDirectory = FSDirectory.Open(IndexPath);

                string pathFile = Path.Combine(StaticObjects.Parameters.TUB_Files_RepositoryFolder, "tubIndex_000.json");
                string json = File.ReadAllText(pathFile);
                List<TubIndex> Indexes = StaticObjects.DeserializeObject<List<TubIndex>>(json);


                Analyzer analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
                IndexWriterConfig config = new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer);
                IndexWriter writer = new IndexWriter(luceneIndexDirectory, config);

                foreach (TubIndex index in Indexes)
                {
                    Document doc = new Document();
                    // StringField indexes but doesn't tokenize
                    doc.Add(new StringField(IndexFieldName, index.Title, Field.Store.YES));
                    doc.Add(new TextField(IndexFieldData, index.Title, Field.Store.YES));
                    writer.AddDocument(doc);
                }
                writer.Flush(triggerMerge: false, applyAllDeletes: false);
                //writer.Flush(true, true);
                //writer.PrepareCommit();
                //writer.Commit();
                writer.Dispose();
                LibraryEventsControl.FireSendMessage("UbIndex created.");
                return true;
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.Error("Error creating Search Index for UB Index.", ex);
                LibraryEventsControl.FireSendMessage("Creating Search Index for UB Index", ex);
                return false;
            }
        }


        public bool Execute(SearchIndexData searchIndexData)
        {
            try
            {
                if (!CreateLuceneIndexForUBIndex(searchIndexData))
                {
                    string message = "Failure generating index for " + IndexPath;
                    StaticObjects.Logger.NonFatalError(message);
                    LibraryEventsControl.FireSendMessage(message);
                    return false;
                }

                Analyzer analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);

                // How to query
                // http://lucenenet.apache.org/
                // https://lucene.apache.org/core/2_9_4/queryparsersyntax.html

                var reader = DirectoryReader.Open(FSDirectory.Open(IndexPath));
                var parser = new QueryParser(LuceneVersion.LUCENE_48, IndexFieldData, analyzer);
                Query searchQuery = parser.Parse(searchIndexData.Query);

                


                IndexSearcher searcher = new IndexSearcher(reader);
                TopDocs hits = searcher.Search(searchQuery, 20);
                searchIndexData.ResultsList = new List<string>();

                // Nothing found? Try to expand (emulate starting with)
                if (hits.ScoreDocs.Length == 0)
                {
                    searchIndexData.Query = searchIndexData.Query.Trim() + "*";
                    searchQuery = parser.Parse(searchIndexData.Query);
                    hits = searcher.Search(searchQuery, 20);
                }

                int results = hits.ScoreDocs.Length;
                for (int i = 0; i < results; i++)
                {
                    Document doc = searcher.Doc(hits.ScoreDocs[i].Doc);
                    searchIndexData.ResultsList.Add(doc.GetField(IndexFieldData).GetStringValue());
                }
                return true;
            }
            catch (Exception ex)
            {
                StaticObjects.Logger.Error("Error executing Index Search.", ex);
                LibraryEventsControl.FireSendMessage("Executing Index Search", ex);
                searchIndexData.ErrorMessage = ex.Message;
                return false;
            }
        }

    }
}
