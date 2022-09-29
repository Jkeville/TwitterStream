namespace Ingestion.API.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddCollection(this Dictionary<string, int> dic, string[]? strings)
        {
            if(strings == null || strings.Length == 0)
                return;

            foreach(string s in strings)
            {
                if(dic.TryGetValue(s, out int value))
                {
                    dic[s] = value + 1;
                }
                else
                {
                    dic.Add(s, 1);
                }
            }
        }
    }
}
