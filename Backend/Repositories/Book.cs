using Library.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Library.Repositories{

 public class BookRepoSQL:IBookRepoSQL
 {
     public string Constr {get;set;}
     public IConfiguration configuration;
     public SqlConnection? con;

     public BookRepoSQL(IConfiguration _configuration)
     {
        configuration=_configuration;
        // Constr=configuration.GetConnectionString("DbConnection");
        Constr=configuration.GetConnectionString("DbConnection");
        Console.WriteLine(Constr);
     }
    
    public List<IBook> GetBookRecord()
    {
        List<IBook> books=new ();
        try{
                // string str="null";
            using(con=new SqlConnection(Constr))
            {
                con.Open();
                var cmd=new SqlCommand("GetBookRecords", con);
                // cmd.CommandType=System.Data.CommandType.StoredProcedure;
                SqlDataReader rdr=cmd.ExecuteReader();
                while(rdr.Read())
                {
                    //  return rdr["author"];
                    IBook bookObj=new IBook();
                    bookObj.id=Convert.ToInt32(rdr["bookId"]);
                    bookObj.name=Convert.ToString(rdr["name"]);
                    bookObj.author=Convert.ToString(rdr["author"]);
                    if((rdr["issues"] is DBNull))
                    bookObj.issues=0;
                    else
                    bookObj.issues=Convert.ToInt32(rdr["issues"]);

                    bookObj.description=Convert.ToString(rdr["description"]);
                    bookObj.coverImage=Convert.ToString(rdr["coverImage"]);
                    bookObj.totalQuantity=Convert.ToInt32(rdr["totalQuantity"]);
                    bookObj.activeIssues=Convert.ToInt32(rdr["activeIssues"]);
                    bookObj.isBookActive=Convert.ToInt32(rdr["isBookActive"]);
                    books.Add(bookObj);
                }
            }
            return books;
            // return books.ToList();
        }
        catch(Exception)
        {
            throw;
        }
    }

    
    public IBook addBook(IBook book)
    {
        
        try{
            IBook bookObj=new IBook();
            string query="INSERT INTO bookTable ( name, author, issues, description, coverImage, isBookActive, totalQuantity, activeIssues) VALUES ('"+book.name+"', '"+book.author+"', '"+book.issues+"', '"+book.description+"','"+book.coverImage+"', '1', '0', '0');";
            using(con=new SqlConnection(Constr))
            {
                con.Open();
                var cmd=new SqlCommand(query, con);
                // cmd.CommandType=System.Data.CommandType.StoredProcedure;
                SqlDataReader rdr=cmd.ExecuteReader();
                while(rdr.Read())
                {
                    //  return rdr["author"];
                    bookObj.id=Convert.ToInt32(rdr["bookId"]);
                    bookObj.name=Convert.ToString(rdr["name"]);
                    bookObj.author=Convert.ToString(rdr["author"]);
                    if((rdr["issues"] is DBNull))
                    bookObj.issues=0;
                    else
                    bookObj.issues=Convert.ToInt32(rdr["issues"]);
                    bookObj.description=Convert.ToString(rdr["description"]);
                }
            }
            // return books.ToList();
            return bookObj;
        }
        catch(Exception)
        {
            throw;
        }
    }
    public IBook getBook(int id)
    {
        try{
            IBook bookObj=new IBook();
            string query="SELECT * FROM bookTable where bookId = "+id+" ;";
            using(con=new SqlConnection(Constr))
            {
                con.Open();
                var cmd=new SqlCommand(query, con);
                // cmd.CommandType=System.Data.CommandType.StoredProcedure;
                SqlDataReader rdr=cmd.ExecuteReader();
                while(rdr.Read())
                {
                    //  return rdr["author"];
                    bookObj.id=Convert.ToInt32(rdr["bookId"]);
                    bookObj.name=Convert.ToString(rdr["name"]);
                    bookObj.author=Convert.ToString(rdr["author"]);
                    if((rdr["issues"] is DBNull))
                    bookObj.issues=0;
                    else
                    bookObj.issues=Convert.ToInt32(rdr["issues"]);
                    bookObj.description=Convert.ToString(rdr["description"]);
                    bookObj.totalQuantity=Convert.ToInt32(rdr["totalQuantity"]);
                    bookObj.isBookActive=Convert.ToInt32(rdr["isBookActive"]);
                    bookObj.activeIssues=Convert.ToInt32(rdr["activeIssues"]);
                }
            }
            // return books.ToList();
            return bookObj;
        }
        catch(Exception)
        {
            throw;
        }
    }
    public void updateBook(int bookId, IBook book)
    {
        
        try{
            IBook bookObj=new IBook();
            string query;
            if(book.coverImage!="unchanged")
            query=$"UPDATE bookTable SET name='{book.name}', author='{book.author}', description='{book.description}',coverImage='{book.coverImage}'   WHERE bookId='{bookId}'";
            else
            query=$"UPDATE bookTable SET name='{book.name}', author='{book.author}', description='{book.description}'   WHERE bookId='{bookId}'";
            using(con=new SqlConnection(Constr))
            {
                con.Open();
                var cmd=new SqlCommand(query, con);
                // cmd.CommandType=System.Data.CommandType.StoredProcedure;
                SqlDataReader rdr=cmd.ExecuteReader();
                while(rdr.Read())
                {
                    //  return rdr["author"];
                    bookObj.id=Convert.ToInt32(rdr["bookId"]);
                    bookObj.name=Convert.ToString(rdr["name"]);
                    bookObj.author=Convert.ToString(rdr["author"]);
                    if((rdr["issues"] is DBNull))
                    bookObj.issues=0;
                    else
                    bookObj.issues=Convert.ToInt32(rdr["issues"]);
                    bookObj.description=Convert.ToString(rdr["description"]);
                }
            }
            // return books.ToList();
        }
        catch(Exception)
        {
            throw;
        }
    }
    public void DeleteBook(int id)
    {
        try{
            IBook bookObj=new IBook();
            string query=$"UPDATE bookTable SET isBookActive=0 WHERE bookId={id}";
            using(con=new SqlConnection(Constr))
            {
                con.Open();
                var cmd=new SqlCommand(query, con);
                // cmd.CommandType=System.Data.CommandType.StoredProcedure;
                SqlDataReader rdr=cmd.ExecuteReader();
                
            }
            // return books.ToList();
        }
        catch(Exception)
        {
            throw;
        }
    }
}
    public interface IBookRepoSQL
    {
        public List<IBook> GetBookRecord();
        public IBook addBook(IBook book);
        public IBook getBook(int id);
        public void updateBook(int bookId,IBook book);
        public void DeleteBook(int bookId);
    }

}