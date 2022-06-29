using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TelegramBot
{
    public class Category
    {
        public int Id { get; set; }

        public string category_name_kz { get; set; }
        public string category_name { get; set; }

        public string category_name_ru { get; set; }

        public string category_name_en { get; set; }


        static string connectionString = @"Data Source=DEMOU;Initial Catalog=TelegramBot1;User Id = telegrambot; Password = F@%7%7lkd2dpnMx36nLNc9";

        public static List<Category> GetCategory(string str)
        {
            List<Category> categoryCon = new List<Category>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string number = "SELECT category_id," + str + "  FROM [TelegramBot1].[db_owner].[Category]";
                SqlCommand sqlCom = new SqlCommand(number, connection);
                SqlDataReader read = sqlCom.ExecuteReader();
                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        categoryCon.Add(new Category()
                        {
                            Id = Convert.ToInt32(read["category_id"].ToString()),
                            category_name = read[str] == DBNull.Value ? "" : read[str].ToString()
                        });
                    }
                }
            }
            return categoryCon;
        }
    }


    public class FAQ
    {
        public static int faq_id { get; set; }

        public string faq_name_kz { get; set; }
        public string faq_name { get; set; }
        public string faq_name_ru { get; set; }

        public string faq_name_en { get; set; }
        public string faq_answer_kz { get; set; }
        public static string faq_answer { get; set; }

        public string faq_answer_ru { get; set; }

        public string faq_answer_en { get; set; }

        public int faq_category_id { get; set; }
        static string connectionString = @"Data Source=DEMOU;Initial Catalog=TelegramBot1;User Id = telegrambot; Password = F@%7%7lkd2dpnMx36nLNc9";
        public static List<FAQ> GetFAQ(string str, int id)
        {
            List<FAQ> faqCon = new List<FAQ>();


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string number = "SELECT faq_name_en," + str + " FROM [TelegramBot1].[db_owner].[FAQ] where faq_category_id=" + id + "";
                SqlCommand sqlCom = new SqlCommand(number, connection);
                SqlDataReader read = sqlCom.ExecuteReader();
                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        faqCon.Add(new FAQ()
                        {
                            faq_name = read[str] == DBNull.Value ? "" : read[str].ToString(),
                            faq_name_en = read["faq_name_en"] == DBNull.Value ? "" : read["faq_name_en"].ToString(),

                        });
                    }
                }
            }
            return faqCon;
        }
        public static string GetAnswer(string str, string callback_answer)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string number = "SELECT " + str + " FROM [TelegramBot1].[db_owner].[FAQ] where faq_name_en='" + callback_answer + "'";
                SqlCommand sqlCom = new SqlCommand(number, connection);
                SqlDataReader read = sqlCom.ExecuteReader();
                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        faq_answer = read[str] == DBNull.Value ? "" : read[str].ToString();
                    }
                }
            }
            if (faq_answer != null)
            {

            }
            return faq_answer;
        }

        public static int GetFaqId(string str)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string number = "SELECT faq_id FROM [TelegramBot1].[db_owner].[FAQ] where faq_name_en='" + str + "'";
                SqlCommand sqlCom = new SqlCommand(number, connection);
                SqlDataReader read = sqlCom.ExecuteReader();
                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        faq_id = read["faq_id"] == DBNull.Value ? 0 : Convert.ToInt32(read["faq_id"]);
                    }
                }
            }
            return faq_id;
        }
        public static void Record_Count_Faq(string callback_answer)
        {
            string number = "update [db_owner].[FAQ] set count_search=(select count_search from FAQ where faq_name_en='" + callback_answer + "')+1 where faq_name_en='" + callback_answer + "'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCom = new SqlCommand(number, connection);
                sqlCom.ExecuteNonQuery();
            }
        }
        public static List<FAQ> GetAnswerBySearch(string callback_answer,string search)
        {
            List<FAQ> faqCon = new List<FAQ>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string number = "SELECT "+ callback_answer + ",faq_name_en FROM [TelegramBot1].[db_owner].[FAQ] where "+ callback_answer + " Like N'%"+ search + "%'";
                SqlCommand sqlCom = new SqlCommand(number, connection);
                SqlDataReader read = sqlCom.ExecuteReader();
                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        faqCon.Add(new FAQ()
                        {
                            faq_answer_kz = read[callback_answer] == DBNull.Value ? "" : read[callback_answer].ToString(),
                            faq_name_en = read["faq_name_en"] == DBNull.Value ? "" : read["faq_name_en"].ToString(),
                        });
                    }
                }
            }
            return faqCon;
        }
    }

    public class SubFAQ
    {
        public int SubFAQ_id { get; set; }

        public string SubFAQ_name_kz { get; set; }

        public string SubFAQ_name_ru { get; set; }

        public string SubFAQ_name_en { get; set; }
        public string SubFAQ_name { get; set; }
        public string SubFAQ_answer_kz { get; set; }

        public string SubFAQ_answer_ru { get; set; }

        public string SubFAQ_answer_en { get; set; }
        public static string SubFAQ_answer { get; set; }
        public int faq_id { get; set; }
        static string connectionString = @"Data Source=DEMOU;Initial Catalog=TelegramBot1;User Id = telegrambot; Password = F@%7%7lkd2dpnMx36nLNc9";
        public static List<SubFAQ> GetFAQ(string str, int id)
        {
            List<SubFAQ> faqCon = new List<SubFAQ>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string number = "SELECT SubFAQ_name_en," + str + " FROM [TelegramBot1].[db_owner].[SubFAQ] where faq_id=" + id + "";
                SqlCommand sqlCom = new SqlCommand(number, connection);
                SqlDataReader read = sqlCom.ExecuteReader();
                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        faqCon.Add(new SubFAQ()
                        {
                            SubFAQ_name = read[str] == DBNull.Value ? "" : read[str].ToString(),
                            SubFAQ_name_en = read["SubFAQ_name_en"] == DBNull.Value ? "" : read["SubFAQ_name_en"].ToString(),
                        });
                    }
                }
            }
            return faqCon;
        }
        public static string GetAnswer(string str, string callback_answer)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string number = "SELECT " + str + " FROM [TelegramBot1].[db_owner].[SubFAQ] where SubFAQ_name_en='" + callback_answer + "'";
                SqlCommand sqlCom = new SqlCommand(number, connection);
                SqlDataReader read = sqlCom.ExecuteReader();
                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        SubFAQ_answer = read[str] == DBNull.Value ? "" : read[str].ToString();
                    }
                }
            }
            return SubFAQ_answer;

        }

        public static List<SubFAQ> GetAnswerBySearch(string callback_answer, string search)
        {
            List<SubFAQ> faqCon = new List<SubFAQ>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string number = "SELECT " + callback_answer + ",SubFAQ_name_en FROM [TelegramBot1].[db_owner].[SubFAQ] where " + callback_answer + " Like N'%" + search + "%'";
                SqlCommand sqlCom = new SqlCommand(number, connection);
                SqlDataReader read = sqlCom.ExecuteReader();
                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        faqCon.Add(new SubFAQ()
                        {
                            SubFAQ_answer_kz = read[callback_answer] == DBNull.Value ? "" : read[callback_answer].ToString(),
                            SubFAQ_name_en = read["SubFAQ_name_en"] == DBNull.Value ? "" : read["SubFAQ_name_en"].ToString(),
                        });
                    }
                }
            }
            return faqCon;
        }
    }
        public class Submenu
    {
        public int Submenu_id { get; set; }
        public string Submenu_name_kz { get; set; }

        public string Submenu_name_ru { get; set; }

        public string Submenu_name_en { get; set; }
        public string Submenu_name { get; set; }
        public string Submenu_answer_kz { get; set; }
        public string Submenu_answer_ru { get; set; }
        public string Submenu_answer_en { get; set; }
        public static string Submenu_answer { get; set; }

        static string connectionString = @"Data Source=DEMOU;Initial Catalog=TelegramBot1;User Id = telegrambot; Password = F@%7%7lkd2dpnMx36nLNc9";
        public static List<Submenu> GetSubmenu(string str)
        {
            List<Submenu> categoryCon = new List<Submenu>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string number = "SELECT Submenu_id," + str + "  FROM [TelegramBot1].[db_owner].[Submenu]";
                SqlCommand sqlCom = new SqlCommand(number, connection);
                SqlDataReader read = sqlCom.ExecuteReader();
                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        categoryCon.Add(new Submenu()
                        {
                            Submenu_id = Convert.ToInt32(read["Submenu_id"].ToString()),
                            Submenu_name = read[str] == DBNull.Value ? "" : read[str].ToString()
                        });
                    }
                }
            }
            return categoryCon;
        }
        public static List<Submenu> GetSubmenu(string str, int id)
        {
            List<Submenu> faqCon = new List<Submenu>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string number = "SELECT Submenu_name_en," + str + " FROM [TelegramBot1].[db_owner].[Submenu] where Submenu_id=" + id + "";
                SqlCommand sqlCom = new SqlCommand(number, connection);
                SqlDataReader read = sqlCom.ExecuteReader();
                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        faqCon.Add(new Submenu()
                        {
                            Submenu_name = read[str] == DBNull.Value ? "" : read[str].ToString(),
                            Submenu_name_en = read["Submenu_name_en"] == DBNull.Value ? "" : read["Submenu_name_en"].ToString()
                        });
                    }
                }
            }
            return faqCon;
        }

        
        public static string GetAnswer(string str, string callback_answer)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string number = "SELECT " + str + " FROM [TelegramBot1].[db_owner].[Submenu] where Submenu_name_en='" + callback_answer + "'";
                SqlCommand sqlCom = new SqlCommand(number, connection);
                SqlDataReader read = sqlCom.ExecuteReader();
                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        Submenu_answer = read[str] == DBNull.Value ? "" : read[str].ToString();
                    }
                }
            }
            if (Submenu_answer != null)
            {

            }
            return Submenu_answer;

        }
    }
    public class Record_Question
    {
        static string connectionString = @"Data Source=DEMOU;Initial Catalog=TelegramBot1;User Id = telegrambot; Password = F@%7%7lkd2dpnMx36nLNc9";
        public static void Record_question(int id, string question, string fio, string phone_number)
        {
            string number = "insert into Visitor(user_id, user_fio,user_phone,user_question,user_question_data) values (" + id + ", N'" + fio + "',N'" + phone_number + "',N'" + question + "',SYSDATETIME())";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCom = new SqlCommand(number, connection);
                sqlCom.ExecuteNonQuery();
            }

        }

    }
}
    
