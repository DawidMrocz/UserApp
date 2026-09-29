namespace Common.ApiModels
{
    public class SearchResponse<TItem> where TItem : SearchItem
    {
        public IEnumerable<TItem> Rows { get; set; }

        public int TotalRows { get; set; }

        public SearchResponse()
        {
        }

        public SearchResponse(IEnumerable<TItem> rows, int totalResult)
        {
            Rows = rows;
            TotalRows = totalResult;
        }

        public SearchResponse(IEnumerable<TItem> rows)
        {
            Rows = rows;
            TotalRows = (rows?.FirstOrDefault()?.TotalRows).GetValueOrDefault();
        }

        public SearchResponse<TOther> Map<TOther>(Func<TItem, TOther> mapFunc) where TOther : SearchItem
        {
            Func<TItem, TOther> mapFunc2 = mapFunc;
            return new SearchResponse<TOther>
            {
                Rows = Rows.Select((TItem r) => mapFunc2(r)),
                TotalRows = TotalRows
            };
        }
    }
}
