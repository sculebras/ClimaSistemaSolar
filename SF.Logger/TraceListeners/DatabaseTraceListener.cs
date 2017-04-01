using System;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;
using System.Data;

namespace Prudential.Logger.TraceListeners
{
    /// <summary>
    /// Summary description for DatabaseTraceListener.
    /// </summary>
    public class DatabaseTraceListener : TraceListener
    {
        private const string COLUMN_SEPARATOR = "|";
        private string m_strConnectionString;
        private int m_iMaximumRequests;
        private string m_strCommandText;
        private string m_strColumnsDefinition;
        private StringCollection m_objCollection;

        public DatabaseTraceListener()
        {
            InitializeListener();
        }

        public DatabaseTraceListener(string r_strListenerName) : base(r_strListenerName)
        {
            InitializeListener();
        }

        private void InitializeListener()
        {
            m_strConnectionString = ConfigurationManager.AppSettings["DatabaseTraceListener_ConnectionString"];
            m_iMaximumRequests = Convert.ToInt32(ConfigurationManager.AppSettings["DatabaseTraceListener_MaximumRequests"]);
            m_strCommandText = ConfigurationManager.AppSettings["DatabaseTraceListener_CommandText"];
            m_strColumnsDefinition = ConfigurationManager.AppSettings["DatabaseTraceListener_CommandText"];
            m_objCollection = new StringCollection();
        }

        private void SaveErrors()
        {
            SqlConnection objConnection = new SqlConnection(m_strConnectionString);
            SqlCommand objCommand = new SqlCommand();
            try
            {
                objCommand.Connection = objConnection;
                objCommand.CommandText = m_strCommandText;
                objCommand.CommandType = CommandType.Text;
                objConnection.Open();

                foreach (string m_strError in m_objCollection)
                {
                    CreateParameters(objCommand, m_strError);
                    objCommand.ExecuteNonQuery();
                }
                m_objCollection.Clear();
            }
            catch (Exception e)
            {

            }
            finally
            {
                if (objConnection != null)
                {
                    if (objConnection.State == ConnectionState.Open)
                        objConnection.Close();
                }
                objConnection = null;
                objCommand = null;
            }
        }

        private void AddToCollection(string r_strTraceDateTime,
            string r_strTraceCategory,
            string r_strTraceDescription,
            string r_strStackTrace,
            string r_strDetailedErrorDescription)
        {
            string strError = r_strTraceDateTime + COLUMN_SEPARATOR +
                r_strTraceCategory + COLUMN_SEPARATOR +
                r_strTraceDescription + COLUMN_SEPARATOR +
                r_strStackTrace + COLUMN_SEPARATOR +
                r_strDetailedErrorDescription;
            m_objCollection.Add(strError);
            if (m_objCollection.Count == m_iMaximumRequests)
            {
                SaveErrors();
            }
        }

        private void CreateParameters(SqlCommand r_objCommand, string r_strError)
        {
            if ((r_objCommand != null) && (!r_strError.Equals("")))
            {
                string[] strColumns;
                SqlParameterCollection objParameters = r_objCommand.Parameters;

                strColumns = r_strError.Split(COLUMN_SEPARATOR.ToCharArray());
                objParameters.Clear();

                objParameters.Add(new SqlParameter("@r_dtTraceDateTime",
                    SqlDbType.DateTime,
                    8));
                objParameters.Add(new SqlParameter("@r_vcTraceCategory",
                    SqlDbType.VarChar,
                    50));
                objParameters.Add(new SqlParameter("@r_vcTraceDescription",
                    SqlDbType.VarChar,
                    1024));
                objParameters.Add(new SqlParameter("@r_vcStackTrace",
                    SqlDbType.VarChar,
                    2048));
                objParameters.Add(new SqlParameter("@r_vcDetailedErrorDescription",
                    SqlDbType.VarChar,
                    2048));

                int iCount = strColumns.GetLength(0);
                for (int i = 0; i < iCount; i++)
                {
                    objParameters[i].IsNullable = true;
                    objParameters[i].Direction = ParameterDirection.Input;
                    objParameters[i].Value = strColumns.GetValue(i).ToString().Trim();
                }
            }
        }


        public override void Write(string message)
        {
            StackTrace objTrace = new StackTrace(true);
            AddToCollection(DateTime.Now.ToString(), "", message, objTrace.ToString(), "");
        }

        public override void Write(object o)
        {
            StackTrace objTrace = new StackTrace(true);
            AddToCollection(DateTime.Now.ToString(), "", o.ToString(), objTrace.ToString(), "");
        }

        public override void Write(string message, string category)
        {
            StackTrace objTrace = new StackTrace(true);
            AddToCollection(DateTime.Now.ToString(), category, message, objTrace.ToString(), "");
        }

        public override void Write(object o, string category)
        {
            StackTrace objTrace = new StackTrace(true);
            AddToCollection(DateTime.Now.ToString(), category, o.ToString(), objTrace.ToString(), "");
        }


        public override void WriteLine(string message)
        {
            Write(message + "\n");
        }

        public override void WriteLine(object o)
        {
            Write(o.ToString() + "\n");
        }

        public override void WriteLine(string message, string category)
        {
            Write((message + "\n"), category);
        }

        public override void WriteLine(object o, string category)
        {
            Write((o.ToString() + "\n"), category);
        }


        public override void Fail(string message)
        {
            StackTrace objTrace = new StackTrace(true);
            AddToCollection(DateTime.Now.ToString(), "Fail", message, objTrace.ToString(), "");
        }

        public override void Fail(string message, string detailMessage)
        {
            StackTrace objTrace = new StackTrace(true);
            AddToCollection(DateTime.Now.ToString(), "Fail", message, objTrace.ToString(), detailMessage);
        }

        public override void Close()
        {
            SaveErrors();
        }

        public override void Flush()
        {
            SaveErrors();
        }

    }
}
