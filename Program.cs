using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace BasicConnectivity;

public class Program
{
    // Data Autentikasi dan koneksi terhadap database yang akan digunakan
    static string connectionString = "Data Source=DESKTOP-HM2DN7T; Integrated Security=True;Database=db_hr_dts;Connect Timeout=30;";
    private static void Main()
    {
        // Memanggil fungsi-fungsi berikut untuk berinteraksi dengan database
        GetAllRegions();
        //GetRegionById(10);
        //InsertRegion("Wakanda Selatan");
        //DeleteRegion(12);
        //UpdateRegion(12, "Wkwkwkwland");
    }

    // GET ALL: Region
    public static void GetAllRegions()
    {

        using var connection = new SqlConnection(connectionString); // Instansiasi untuk connect ke database dengan argument data autentikasi yang sudah di define sebelumnya
        using var command = new SqlCommand(); // Instansiasi untuk menjalankan manipulation atau query database

        command.Connection = connection; // menghubungkan query dengan tabel database yg ada
        command.CommandText = "SELECT * FROM regions"; // melakukan query yaitu select semua baris dan kolom pada tabel regions

        try
        {
            connection.Open(); // membuka koneksi database

            using var reader = command.ExecuteReader(); // menjalankan method untuk membaca data pada tabel

            // Pengondisian apabila data pada databel tersedia tampilkan, apabila tidak data pada tabel kosong
            if (reader.HasRows)
                while (reader.Read())
                {
                    Console.WriteLine("Id: " + reader.GetInt32(0));
                    Console.WriteLine("Name: " + reader.GetString(1));
                }
            else
                Console.WriteLine("No rows found.");

            reader.Close(); // menutup sesi membaca data pada tabel
            connection.Close(); // menutup sesi koneksi ke database
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}"); // Pesan Error apabila koneksi ke database gagal
        }
    }

    // GET BY ID: RegionById
    public static void GetRegionById(int id) 
    {
        using var connection = new SqlConnection(connectionString); // Instansiasi untuk connect ke database dengan argument data autentikasi yang sudah di define sebelumnya
        using var command = new SqlCommand(); // Instansiasi untuk menjalankan manipulation atau query database

        command.Connection = connection; // menghubungkan query dengan tabel database yg ada
        command.CommandText = "SELECT * FROM regions WHERE id = @id"; // melakukan query yaitu select pada kolom dan baris berdasarkan id yang dipilih

        try
        {   
            //mendefine atau menentukan paramater masukan yaitu Id untuk menjadi argument pada query yang dilakukan
            var pId = new SqlParameter();
            pId.ParameterName = "@id";
            pId.Value = id;
            pId.SqlDbType = SqlDbType.Int;
            command.Parameters.Add(pId);

            connection.Open(); // membuka koneksi database

            using var reader = command.ExecuteReader(); // menjalankan method untuk membaca data pada tabel

            // Pengondisian apabila data pada databel tersedia tampilkan, apabila tidak data pada tabel kosong
            if (reader.HasRows)
                while (reader.Read())
                {
                    Console.WriteLine("Id: " + reader.GetInt32(0));
                    Console.WriteLine("Name: " + reader.GetString(1));
                }
            else
                Console.WriteLine("No rows found.");

            reader.Close(); // menutup sesi membaca data pada tabel
            connection.Close(); // menutup sesi koneksi ke database
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}"); // Pesan Error apabila koneksi ke database gagal
        }
    }

    // INSERT: Region
    public static void InsertRegion(string name)
    {
        using var connection = new SqlConnection(connectionString); // Instansiasi untuk connect ke database dengan argument data autentikasi yang sudah di define sebelumnya
        using var command = new SqlCommand(); // Instansiasi untuk menjalankan manipulation atau query database

        command.Connection = connection; // menghubungkan perintah manipulasi dengan tabel database yg ada
        command.CommandText = "INSERT INTO regions VALUES (@name);"; // melakukan manipulasi yaitu insert dengan menambahkan data region yang baru

        try
        {
            //mendefine atau menentukan paramater masukan yaitu Name untuk menjadi argument pada manipulasi yang dilakukan
            var pName = new SqlParameter();
            pName.ParameterName = "@name";
            pName.Value = name;
            pName.SqlDbType = SqlDbType.VarChar;
            command.Parameters.Add(pName);
             
            connection.Open(); // membuka koneksi database
            using var transaction = connection.BeginTransaction(); // menjalankan method transaksi, bertujuan untuk merecord manipulasi data yang dilakukan. 
            try
            {
                command.Transaction = transaction; // menghubungkan transaksi dengan perintah manipulasi data sebelumnya

                var result = command.ExecuteNonQuery(); // menjalankan method untuk mengeksekusi perintah manipulasi data

                transaction.Commit(); // menjalankan transaksi yang dilakukan
                connection.Close(); // menutup sesi koneksi ke database

                // pengondisian terhadap manipulasi data berhasil dan tidak
                switch (result)
                {
                    case >= 1:
                        Console.WriteLine("Insert Success");
                        break;
                    default:
                        Console.WriteLine("Insert Failed");
                        break;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback(); // Apabila manipulasi data gagal, maka keadaan tabel akan dikembalikan ke keadaan sebelumnya
                Console.WriteLine($"Error Transaction: {ex.Message}"); // Pesan Error apabila transaksi gagal
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}"); // Pesan Error apabila koneksi ke database gagal
        }
    }

    // UPDATE: Region
    public static void UpdateRegion(int id, string name) 
    {
        using var connection = new SqlConnection(connectionString); // Instansiasi untuk connect ke database dengan argument data autentikasi yang sudah di define sebelumnya
        using var command = new SqlCommand(); // Instansiasi untuk menjalankan manipulation atau query database

        command.Connection = connection; // menghubungkan query dengan tabel database yg ada
        command.CommandText = "UPDATE regions SET name = @name WHERE id = @id;"; // melakukan manipulasi yaitu update dengan memperbaharui data berdasarkan id dan nama yang dipilih

        try
        {
            //mendefine atau menentukan paramater masukan yaitu Name untuk menjadi argument pada manipulasi yang dilakukan
            var pId = new SqlParameter();
            pId.ParameterName = "@id";
            pId.Value = id;
            pId.SqlDbType = SqlDbType.Int;
            command.Parameters.Add(pId);

            var pName = new SqlParameter();
            pName.ParameterName = "@name";
            pName.Value = name;
            pName.SqlDbType = SqlDbType.VarChar;
            command.Parameters.Add(pName);

            connection.Open(); // membuka koneksi database
            using var transaction = connection.BeginTransaction(); // menjalankan method transaksi, bertujuan untuk merecord manipulasi data yang dilakukan. 
            try
            {
                command.Transaction = transaction; // menghubungkan transaksi dengan perintah manipulasi data sebelumnya

                var result = command.ExecuteNonQuery(); // menjalankan method untuk mengeksekusi perintah manipulasi data

                transaction.Commit(); // menjalankan transaksi yang dilakukan
                connection.Close(); // menutup sesi koneksi ke database

                // pengondisian terhadap manipulasi data berhasil dan tidak
                switch (result)
                {
                    case >= 1:
                        Console.WriteLine("Update Success");
                        break;
                    default:
                        Console.WriteLine("Update Failed");
                        break;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback(); // Apabila manipulasi data gagal, maka keadaan tabel akan dikembalikan ke keadaan sebelumnya
                Console.WriteLine($"Error Transaction: {ex.Message}"); // Pesan Error apabila transaksi gagal
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}"); // Pesan Error apabila koneksi ke database gagal
        }

    }

    // DELETE: Region
    public static void DeleteRegion(int id) 
    {
        using var connection = new SqlConnection(connectionString); // Instansiasi untuk connect ke database dengan argument data autentikasi yang sudah di define sebelumnya
        using var command = new SqlCommand(); // Instansiasi untuk menjalankan manipulation atau query database

        command.Connection = connection; // menghubungkan query dengan tabel database yg ada
        command.CommandText = "DELETE FROM regions WHERE id = @id;"; // melakukan manipulasi yaitu delete dengan menghapus data berdasarkan id  yang dipilih

        try 
        {
            //mendefine atau menentukan paramater masukan yaitu Name untuk menjadi argument pada manipulasi yang dilakukan
            var pId = new SqlParameter();
            pId.ParameterName = "@id";
            pId.Value = id;
            pId.SqlDbType = SqlDbType.Int;
            command.Parameters.Add(pId);

            connection.Open(); // membuka koneksi database
            using var transaction = connection.BeginTransaction(); // menjalankan method transaksi, bertujuan untuk merecord manipulasi data yang dilakukan. 
            try
            {
                command.Transaction = transaction; // menghubungkan transaksi dengan perintah manipulasi data sebelumnya

                var result = command.ExecuteNonQuery(); // menjalankan method untuk mengeksekusi perintah manipulasi data

                transaction.Commit(); // menjalankan transaksi yang dilakukan
                connection.Close(); // menutup sesi koneksi ke database

                // pengondisian terhadap manipulasi data berhasil dan tidak
                switch (result)
                {
                    case >= 1:
                        Console.WriteLine("Delete Success");
                        break;
                    default:
                        Console.WriteLine("Delete Failed");
                        break;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback(); // Apabila manipulasi data gagal, maka keadaan tabel akan dikembalikan ke keadaan sebelumnya
                Console.WriteLine($"Error Transaction: {ex.Message}"); // Pesan Error apabila transaksi gagal
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}"); // Pesan Error apabila koneksi ke database gagal
        }
    }
}