namespace DataLayer.Exceptions
{
    public class DbDeleteException : Exception
    {
        public DbDeleteException(string message) : base(message) { }
    }
}
