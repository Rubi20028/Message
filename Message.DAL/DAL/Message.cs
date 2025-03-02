using System.Data;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace Message.DAL.DAL;

public class Message
{
    string conString = "Host=host.docker.internal;Port=5433;Database=postgres;Username=postgres;Password=postgres";

    public List<Domain.Models.Message> GetAllMessages()
    {
        List<Domain.Models.Message> messagesList = new List<Domain.Models.Message>();
        
        string sql = "SELECT * FROM getall()";
        using (NpgsqlConnection connection = new NpgsqlConnection(conString))
        {
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Domain.Models.Message message = new Domain.Models.Message
                        {
                            MessageId = reader.GetInt32(0), 
                            Text = reader.GetString(1), 
                            TimeStamp = reader.GetDateTime(2), 
                            SequenceNumber = reader.GetInt32(3)
                        };

                        messagesList.Add(message);
                    }
                }
            }
            connection.Close();
        }
        return messagesList;
    }

    public bool InsertMessage(Domain.Models.Message message)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(conString))
        {
            string sql = "CALL InsertMessage(@p_text, @p_timestamp)";
            
            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("p_text", message.Text);
                command.Parameters.AddWithValue("p_timestamp", message.TimeStamp);
                
                connection.Open();
                
                int rowsAffected = command.ExecuteNonQuery();
                
                connection.Close();

                return rowsAffected > 0;
            }
        }
    }
}