using Dapper;
using Dapper.FastCrud;
using Microsoft.Data.Sqlite;

namespace FakeNewsScraper
{
    internal class Dal
    {
        private static string DbFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\\FakeNewsScraper.db";

        public void InitializeDatabase()
        {
            File.Delete(DbFilePath);

            using (var connection = new SqliteConnection($"Data Source={DbFilePath};"))
            {
                connection.Open();
                connection.Execute("CREATE TABLE facebook_group (content TEXT, summary TEXT, title TEXT, link TEXT);");
                connection.Execute("CREATE TABLE facebook_group_post (groupLink TEXT, summary TEXT, text TEXT, link TEXT);");
            }
        }

        public void Save<T>(T item)
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={DbFilePath};"))
                {
                    connection.Open();
                    connection.Insert(item);
                }
            }
            catch (Exception ex)
            {
                LogError("Save to Database - " + ex.Message);
            }
        }

        protected static void LogError(string message)
        {
            Console.WriteLine(message);
        }
    }
}
