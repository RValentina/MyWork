using AccountModel;
using System.Data;
using System.Data.SqlClient;

namespace AccountDataLayer
{
    public class AccountDA
    {
        /// <summary>
        /// Save the accounts to the database.
        /// </summary>
        /// <param name="account"></param>
        public void SaveAccounts(AccModel account)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ATTSCodeAssignment.Properties.Settings.AccountdbConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"insert into dbo.Account 
                                (Name,
                                Description,
                                CurrencyCode,
                                Value)
                                values
                                (@Name,
                                @Description,
                                @CurrencyCode,
                                @Value);";

            cmd.Parameters.Add("@Name", SqlDbType.VarChar);
            cmd.Parameters["@Name"].Value = account.Account;
            cmd.Parameters.Add("@Description", SqlDbType.VarChar);
            cmd.Parameters["@Description"].Value = account.Description;
            cmd.Parameters.Add("@CurrencyCode", SqlDbType.VarChar);
            cmd.Parameters["@CurrencyCode"].Value = account.CurrencyCode;
            cmd.Parameters.Add("@Value", SqlDbType.Int);
            cmd.Parameters["@Value"].Value = account.Value;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

            
        }
    }
}
