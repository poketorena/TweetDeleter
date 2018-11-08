using CsvHelper.Configuration;

namespace TweetDeleter
{
    public sealed class MyClassMap : ClassMap<MyStatus>
    {
        public MyClassMap()
        {
            Map(m => m.Id).Name("tweet_id");
            Map(m => m.Text).Name("text");
        }
    }
}
