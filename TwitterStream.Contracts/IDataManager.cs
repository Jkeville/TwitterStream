using TweetStream.Models.Models;

namespace TwitterStream.Contracts
{
    public interface IDataManager
    {
        public Task<TopTenTotalsModel> GetData();
    }
}
