using SQLite;

namespace Hangman
{
    public class Userscore
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public string Level { get; set; }
    }
}