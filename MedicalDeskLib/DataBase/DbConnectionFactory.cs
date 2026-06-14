using MySql.Data.MySqlClient;

namespace MedicalDeskLib.Database;

public static class DbConnectionFactory
{
    private const string ConnectionString =
        "server=localhost;" +
        "database=MedicalDesk;" +
        "uid=root;" +
        "pwd= vertrigo;" +
        "charset=utf8mb4;";

    public static MySqlConnection Create()
    {
        return new MySqlConnection(ConnectionString);
    }
}