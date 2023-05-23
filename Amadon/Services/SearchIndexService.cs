using AmadonStandardLib.Classes;
using AmadonStandardLib.Helpers;
using AmadonStandardLib.InterchangeData;
using System.Text;

namespace Amadon.Services
{
    public class SearchIndexService
    {
        private static LuceneIndexSearch searchIndex = new LuceneIndexSearch();

        //public async static SearchIndexData DoSearch(SearchIndexData searchIndexData)
        //{
        //    bool ret;
        //    ret = await InitializationService.InitSubjectIndex();
        //    if (!ret)
        //    {

        //    }

        //    LuceneIndexSearch searhIndex = new LuceneIndexSearch();
        //    // When an erro occurs, error message is set inside SearchIndexData
        //    searchIndexData.IndexPathRoot = StaticObjects.Parameters.IndexSearchFolders;
        //    searhIndex.Execute(searchIndexData);
        //    return Task.FromResult(searchIndexData);
        //}

        public async static Task<SearchIndexData> DoSearch(SearchIndexData searchIndexData)
        {
            bool ret;
            ret = await InitializationService.InitSubjectIndex();
            if (!ret)
            {
                return null;
            }

            // When an erro occurs, error message is set inside SearchIndexData
            searchIndexData.IndexPathRoot = StaticObjects.Parameters.IndexSearchFolders;
            ret= await searchIndex.Execute(searchIndexData);
            return searchIndexData;
        }



        public async static Task<TubIndex> GetSubjectItemsToShow(string startString)
        {
            TubIndex index = await LuceneIndexSearch.GetSubjectIndexEntry(startString);
            return index;
        }


    }
}
