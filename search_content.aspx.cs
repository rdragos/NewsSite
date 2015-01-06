using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
public partial class search_content : System.Web.UI.Page
{
    public const int MAX_RESULTS = 5;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Params["query"] != null)
        {
            //must do the search
            queryDB(Request.Params["query"]);
        }
    }
    private void queryDB(string p)
    {
        List<string> allQ = p.Split(' ').ToList();
        var con = ConfigurationManager.ConnectionStrings["SqlServices"].ToString();
        using (SqlConnection myConnection = new SqlConnection(con))
        {
            string queryString =
                "SELECT Articles.Title, Articles.Content, Articles.CategoryName, " +
            " aspnet_Users.UserName, Articles.ArticleId " +
            "FROM Articles INNER JOIN aspnet_Users ON " +
            "Articles.CreatedBy = aspnet_Users.UserId " +
            "WHERE     Articles.PendingStatus=0";

            SqlCommand oCmd = new SqlCommand(queryString, myConnection);
            myConnection.Open();

            var gridDataTable = new DataTable();
            gridDataTable.Columns.Add("ArticleId");
            gridDataTable.Columns.Add("Title");
            gridDataTable.Columns.Add("Score");

            List<string> sList = new List<string>();
            try
            {
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {

                        string blob = "";

                        var newRow = gridDataTable.NewRow();
                        blob += " " + oReader.GetString(0);
                        blob += " " + oReader.GetString(1);
                        blob += " " + oReader.GetString(2);
                        blob += " " + oReader.GetString(3);

                        newRow["Title"] = oReader.GetString(0);
                        newRow["ArticleId"] = oReader.GetGuid(4).ToString();
                        newRow["Score"] = "100";

                        gridDataTable.Rows.Add(newRow);
                        sList.Add(blob);

                    }
                    Session["GridResults"] = filterResults(sList, gridDataTable, allQ);
                    gvResults.DataSource = Session["GridResults"];
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            gvResults.DataBind();
        }
    }
    private DataTable filterResults(List<string> sList, DataTable dt, List<string> toSearch)
    {
        int index = 0;
        List<Tuple<int, int>> scores = new List<Tuple<int, int>>();
        foreach (DataRow row in dt.Rows)
        {
            Dictionary<string, int> wordCount = new Dictionary<string, int>();
            foreach (string word in toSearch)
            {
                string curent_word = word.ToLower();
                int value;
                if (!wordCount.TryGetValue(curent_word, out value)) {
                    wordCount.Add(curent_word, 1);
                }
            }
            var splitted_string = sList[index].Split(" ".ToArray());
            int cnt = 0;
            foreach(string batch in splitted_string)
            {
                string word = batch.ToLower();
                if (wordCount.ContainsKey(word)) 
                {
                    cnt += 1;
                }
            }
            scores.Add(new Tuple<int, int>(cnt, index));
            index += 1;
        }
        scores.Sort();
        scores.Reverse();

        DataTable ret = new DataTable();
        ret.Columns.Add("ArticleId");
        ret.Columns.Add("Title");
        ret.Columns.Add("Score");

        index = 0;
        foreach (Tuple<int, int> t in scores)
        {
            var newRow = ret.NewRow();
            newRow["ArticleId"] = dt.Rows[t.Item2]["ArticleId"];
            newRow["Title"] = dt.Rows[t.Item2]["Title"];
            newRow["Score"] = t.Item1.ToString();
            ret.Rows.Add(newRow);
            if (index > MAX_RESULTS)
            {
                break;
            }
            index += 1;
        }
        return ret;
    }
    protected void searchAction(object sender, EventArgs e)
    {
        if (tb_search.Text != null)
        {
            Response.Redirect("search_content.aspx?query=" + tb_search.Text.Replace(" ", "%20"));
        }
    }
    
}