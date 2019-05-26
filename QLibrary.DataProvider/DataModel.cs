using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using EntityData;

namespace DataLibrary
{
    public class DataModel
    {
        string _sqlConnStr = "SERVER=OSIT-L-0138;DATABASE=QLibraryDevelopment; uid=sa; Password=Sql@12345;";
        public List<Query> GetAllQueries()
        {
            var objQueryList = new List<Query>();
            using (var connection = new SqlConnection(_sqlConnStr))
            {
                connection.Open();
                var cmd = new SqlCommand("SP_GetAllQueries", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader drQuery = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (drQuery.HasRows)
                {    
                    while (drQuery.Read())
                    {
                        var objQuery = new Query();
                        if (drQuery["QueryId"] != null)
                        {
                            objQuery.QueryId = Convert.ToInt32(drQuery["QueryId"]);
                        }
                        if (drQuery["SectionId"] != null)
                        {
                            objQuery.SectionId = Convert.ToInt32(drQuery["SectionId"]);
                        }
                        if (drQuery["SectionName"] != null)
                        {
                            objQuery.SectionName = Convert.ToString(drQuery["SectionName"]);
                        }

                        
                        if (drQuery["QueryName"] != null)
                        {
                            objQuery.QueryName = Convert.ToString(drQuery["QueryName"]);
                        }
                        if (drQuery["Description"] != null)
                        {
                            objQuery.Description = Convert.ToString(drQuery["Description"]);
                        }
                        if (drQuery["SCreatedDate"] != null)
                        {
                            objQuery.SCreatedDate = Convert.ToDateTime(drQuery["SCreatedDate"]);
                        }
                        objQueryList.Add(objQuery);
                    }
                }
                connection.Close();
            }
            return objQueryList;
        }
        public List<Section> GetAllSections()
        {
            var objSectionList = new List<Section>();
            using (var connection = new SqlConnection(_sqlConnStr))
            {
                connection.Open();
                var cmd = new SqlCommand("SP_GetAllSections", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader drSection = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (drSection.HasRows)
                {  
                    while (drSection.Read())
                    {
                        var objSection = new Section();
                        if (drSection["SectionId"] != null)
                        {
                            objSection.SectionId = Convert.ToInt32(drSection["SectionId"]);
                        }
                        if (drSection["RequiredCount"] != null)
                        {
                            objSection.RequiredCount = Convert.ToInt32(drSection["RequiredCount"]);
                        }
                        if (drSection["TotalCount"] != null)
                        {
                            objSection.TotalCount = Convert.ToInt32(drSection["TotalCount"]);
                        }
                        if (drSection["SectionName"] != null) 
                        {
                            objSection.SectionName = Convert.ToString(drSection["SectionName"]);
                        }
                        if (drSection["CreatedDate"] != null)
                        {
                            objSection.CreatedDate = Convert.ToDateTime(drSection["CreatedDate"]);
                        }
                        objSectionList.Add(objSection);
                    }
                }
                connection.Close();
            }
            return objSectionList;
        }
        public Query GetQueryByQueryId(int QueryId)
        {
            var objQuery = new Query();
            using (var connection = new SqlConnection(_sqlConnStr))
            {
                connection.Open();
                var cmd = new SqlCommand("SP_GetQueryByQueryId", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@QueryId", QueryId));
                SqlDataReader drQuery = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (drQuery.HasRows)
                {
                    while (drQuery.Read())
                    {
                        if (drQuery["QueryId"] != null)
                        {
                            objQuery.QueryId = Convert.ToInt32(drQuery["QueryId"]);
                        }
                        if (drQuery["SectionId"] != null)
                        {
                            objQuery.SectionId = Convert.ToInt32(drQuery["SectionId"]);
                        }
                        if (drQuery["QueryName"] != null)
                        {
                            objQuery.QueryName = Convert.ToString(drQuery["QueryName"]);
                        }
                        if (drQuery["Description"] != null)
                        {
                            objQuery.Description = Convert.ToString(drQuery["Description"]);
                        }
                        if (drQuery["SCreatedDate"] != null)
                        {
                            objQuery.SCreatedDate = Convert.ToDateTime(drQuery["SCreatedDate"]);
                        }
                    }
                }
                connection.Close();
            }
            return objQuery;
        }
        public Section GetSectionBySectionId(int sectionId)
        {
            var objSection = new Section();
            using (var connection = new SqlConnection(_sqlConnStr))
            {
                connection.Open();
                var cmd = new SqlCommand("SP_GetSectionBySectionId", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@SectionId", sectionId));
                SqlDataReader drSection = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (drSection.HasRows)
                {
                    while (drSection.Read())
                    {
                        if (drSection["SectionId"] != null)
                        {
                            objSection.SectionId = Convert.ToInt32(drSection["SectionId"]);
                        }
                        if (drSection["RequiredCount"] != null)
                        {
                            objSection.RequiredCount = Convert.ToInt32(drSection["RequiredCount"]);
                        }
                        if (drSection["SectionName"] != null)
                        {
                            objSection.SectionName = Convert.ToString(drSection["SectionName"]);
                        }
                        if (drSection["CreatedDate"] != null)
                        {
                            objSection.CreatedDate = Convert.ToDateTime(drSection["CreatedDate"]);
                        }
                    }
                }
                connection.Close();
            }
            return objSection;
        }
        public int InsertQuery(Query qry)
        {
            int value = 0;
            using (var connection = new SqlConnection(_sqlConnStr))
            {
                connection.Open();
                var cmd = new SqlCommand("SP_InsertQuery", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@SectionId", qry.SectionId));
                cmd.Parameters.Add(new SqlParameter("@QueryName", qry.QueryName));
                cmd.Parameters.Add(new SqlParameter("@Description", qry.Description));
                value = cmd.ExecuteNonQuery();
                connection.Close();
            }
            return value;
        }
        public int InsertSection(Section sec)
        {
            int value = 0;
            using (var connection = new SqlConnection(_sqlConnStr))
            {
                connection.Open();
                var cmd = new SqlCommand("SP_InsertSection", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@SectionName", sec.SectionName));
                cmd.Parameters.Add(new SqlParameter("@RequiredCount", sec.RequiredCount));
                value = cmd.ExecuteNonQuery();
                connection.Close();
            }
            return value;
        }
        public int UpdateQueryByQueryId(Query qry)
        {
            int value = 0;
            using (var connection = new SqlConnection(_sqlConnStr))
            {
                connection.Open();
                var cmd = new SqlCommand("SP_UpdateQueryByQueryId", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@QueryId", qry.QueryId));
                cmd.Parameters.Add(new SqlParameter("@SectionId", qry.SectionId));
                cmd.Parameters.Add(new SqlParameter("@QueryName", qry.QueryName));
                cmd.Parameters.Add(new SqlParameter("@Description", qry.Description));
                value = cmd.ExecuteNonQuery();
                connection.Close();
            }
            return value;
        }
        public int UpdateSectionBySectionId(Section sec)
        {
            int value = 0;
            using (var connection = new SqlConnection(_sqlConnStr))
            {
                connection.Open();
                var cmd = new SqlCommand("SP_UpdateSectionBySectionId", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@SectionId", sec.SectionId));
                cmd.Parameters.Add(new SqlParameter("@SectionName", sec.SectionName));
                cmd.Parameters.Add(new SqlParameter("@RequiredCount", sec.RequiredCount));
                value = cmd.ExecuteNonQuery();
                connection.Close();
            }
            return value;
        }
    }
}
