using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace cat
{
    class Program
    {
        static void Main(string[] args)
        {
            string cs = @"server=localhost;userid=root;password=password;database=giraffe";
            using var con = new MySqlConnection(cs);
            con.Open();
            Console.WriteLine($"MySQL version : {con.ServerVersion}");
            Console.Write("Introduceti numele elevului: ");
            string numeElev = Console.ReadLine();
            bool elevExista = verificaElevExista(con, numeElev);
            if(elevExista == false)
            {
                elevInsert(con, numeElev);
            }
            elevNume(con, numeElev);
            elevNote(con, numeElev);
            elevAbsente(con, numeElev);

            
            var selectie = Selectie();
            int caseSwitch = selectie;
            while(caseSwitch != 0)
            {
                switch(caseSwitch)
                {
                    case 1:
                        Console.WriteLine("Introduceti nota: ");
                        string notaElev = Console.ReadLine();
                        string adaugaNota = "INSERT INTO Note VALUES ((SELECT student_key FROM Catalog WHERE student_name LIKE '" + numeElev +"'), " + notaElev + ");";
                        var cmd = new MySqlCommand(adaugaNota, con);
                        cmd.CommandText = adaugaNota;
                        cmd.ExecuteNonQuery();
                        //caseSwitch = Selectie();
                        break;
                    case 2:
                        Console.WriteLine("Adaugati absenta? Y / N");
                        string answer = Console.ReadLine().ToLower();
                        if (answer != "y" & answer != "n")
                        {
                            Console.WriteLine("Ati introdus un raspuns invalid.");
                        }
                        else
                        {
                            string adaugaAbsenta = "INSERT INTO Absente VALUES((SELECT student_key FROM Catalog WHERE student_name LIKE '" + numeElev + "'), CURRENT_TIMESTAMP());";
                            var cmd1 = new MySqlCommand(adaugaAbsenta, con);
                            cmd1.CommandText = adaugaAbsenta;
                            cmd1.ExecuteNonQuery();
                            //caseSwitch = Selectie();

                        }
                        break;
                    case 3:
                        elevNume(con, numeElev);
                        elevNote(con, numeElev);
                        elevAbsente(con, numeElev);
                        //caseSwitch = Selectie();
                        break;
                    case 4:
                        Console.Write("Introduceti numele elevului: ");
                        numeElev = Console.ReadLine();
                        bool elevExista1 = verificaElevExista(con, numeElev);
                        if(elevExista == false)
                        {
                            elevInsert(con, numeElev);
                        }
                        elevNume(con, numeElev);
                        elevNote(con, numeElev);
                        elevAbsente(con, numeElev);
                        //caseSwitch = Selectie();
                        break;
                    case 0:
                    break;
                    default:
                    //Console.WriteLine("Ati introdus o optiune invalida.");
                    //caseSwitch = Selectie();
                    break;
                    
          
                }
                caseSwitch = Selectie();
                if(caseSwitch == 0)
                {
                    Console.WriteLine("La revedere!");
                }
            }
        }

        static string elevAbsente(MySqlConnection con, string numeElev)
        {
            string sqlAbsente = "SELECT Absente.absenta FROM Absente WHERE Absente.student_key = (SELECT student_key FROM Catalog WHERE student_name LIKE '" + numeElev + "');";
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlAbsente, con);
            DataSet situatieAbsente = new DataSet();
            dataAdapter.Fill(situatieAbsente);
            foreach (DataTable table in situatieAbsente.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        object item = row[column];
                        string i = item.ToString();
                        Console.WriteLine(i);
                    }
                }
            } 
            return situatieAbsente.ToString();
        }

        static string elevNote(MySqlConnection con, string numeElev)
        {
            string sqlNote = "SELECT Note.note FROM Note WHERE Note.student_key = (SELECT student_key FROM Catalog WHERE student_name LIKE '" + numeElev + "');";
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlNote, con);
            DataSet situatieNote = new DataSet();
            dataAdapter.Fill(situatieNote);
            foreach (DataTable table in situatieNote.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        object item = row[column];
                        // read column and item
                        string i = item.ToString();
                        Console.WriteLine(i);
                    }
                }
            } 
            return situatieNote.ToString();
        }

        static string elevNume(MySqlConnection con, string numeElev)
        {
            string situatieNume = "SELECT * FROM Catalog WHERE student_name = '" + numeElev + "';";
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(situatieNume, con);
            DataSet situatiesql = new DataSet();
            dataAdapter.Fill(situatiesql);
            foreach (DataTable table in situatiesql.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        object item = row[column];
                        // read column and item
                        string i = item.ToString();
                        Console.WriteLine(i);
                    }
                }
            }
            return situatiesql.ToString();
        }

        static void elevInsert(MySqlConnection con, string numeElev)
        {
            string sqlinsert = "INSERT INTO Catalog(student_name) VALUES('" + numeElev +"')";
            using var cmd = new MySqlCommand(sqlinsert, con);
            cmd.CommandText = sqlinsert;
            cmd.ExecuteNonQuery();
        }

        static bool verificaElevExista(MySqlConnection con, string numeElev)
        {

            string sql = "SELECT * FROM Catalog WHERE student_name LIKE '" + numeElev + "'";
            using var cmd = new MySqlCommand(sql, con);
            using MySqlDataReader rdr = cmd.ExecuteReader();
            
            if (rdr.Read())
            {
                return true;
                //con.Close();
            }
            else
            {
                return false;
                //con.Close();
            }
        }

        static int Selectie()
        {
            System.Console.WriteLine("1. Adaugati note.");
            System.Console.WriteLine("2. Adaugati absente.");
            System.Console.WriteLine("3. Situatie elev.");
            System.Console.WriteLine("4. Alegeti alt elev");
            System.Console.WriteLine("0. Iesiti");
            var selectie = Console.ReadLine();
            int select;
            if(int.TryParse(selectie, out select))
            {
                Convert.ToInt32(selectie);
                while(select < 0 | select > 4)
                {
                    Console.WriteLine("Ati introdus o selectie invalida.");
                    Console.WriteLine("Introduceti selectia:");
                    select = Convert.ToInt32(Console.ReadLine());
                }
                return select;
            }
            else
            {
                Console.WriteLine("Ati introdus un parametru invalid.");
                Console.WriteLine("Introduceti selectia:");
                select = Convert.ToInt32(Console.ReadLine());
                return select;
            }
        }
        
    }
}
