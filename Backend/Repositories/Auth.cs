using System;
using System.Data.SqlClient;
using Library.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Library.Repositories
{

    public class AuthRepo : IAuthRepo
    {
        public string Constr { get; set; }
        public IConfiguration configuration;
        public SqlConnection? con;
        private readonly string key;
        public static string currentHash = null;
        public AuthRepo(IConfiguration _configuration)
        {
            configuration = _configuration;
            // Constr=configuration.GetConnectionString("DbConnection");
            Constr = configuration.GetConnectionString("DbConnection");
            Console.WriteLine(Constr);
        }
        public IUser login(string username, string password)
        {
            using (con = new SqlConnection(Constr))
            {
                IUser user = new IUser();
                con.Open();
                var cmd = new SqlCommand($"Select * from userTable where username='{username}' ", con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    user.username = Convert.ToString(rdr["username"]);
                    user.password = Convert.ToString(rdr["password"]);
                    // bool tempPass = user.password != password;
                    // string userPassword = Convert.ToString(rdr["password"]);
                    user.role = Convert.ToInt32(rdr["role"]);
                    // var tempSalt = Convert.ToString(rdr["userSalt"]);
                    // user.passwordSalt = Encoding.UTF8.GetBytes(tempSalt);
                    // for (int i = 0; i < tempSalt.Length; i++)
                    // {
                    // var tempStr = Convert.ToByte(tempSalt[i]);
                    // user.passwordSalt.Append(Convert.ToByte(tempSalt[i]));
                    // }
                    // var tempHash = Convert.ToString(rdr["userHash"]);
                    // user.passwordHash = Encoding.UTF8.GetBytes(tempHash);
                    // for (int i = 0; i < tempSalt.Length; i++)
                    // {
                    // user.passwodHash.Append(Convert.ToByte(tempHash[i]));
                    // }
                    // if (!verifyHash(user.password, user.passwordHash, user.passwordSalt))
                    // {
                    //     // var val = verifyHash(user.password, user.passwordHash, user.passwordSalt);
                    //     return tempHash + " " + tempSalt;
                    // }

                }
                string userPassword = user.password;
                if (user.username == null)
                {
                    throw new AppException("User not found!");
                }
                else if (userPassword != password)
                {
                    throw new AppException("Wrong Password");
                }
                else
                {
                    var token = CreateToken(user);
                    user.token = token;
                    return user;
                }
                throw new AppException("Invalid User" + user.password);
            }
            return null;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public IUser register(string username, string password, int role)
        {
            if (username == "")
                throw new AppException("Username Invalid!");
            if (password == "" || password.Length < 8)
                throw new AppException("Password Invalid");
            // CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            // string passwordHashString = Encoding.UTF8.GetString(passwordHash, 0, passwordHash.Length);
            // string passwordStaltString = Encoding.UTF8.GetString(passwordSalt, 0, passwordSalt.Length);
            // currentHash = passwordStaltString;
            using (con = new SqlConnection(Constr))
            {
                con.Open();
                var cmd = new SqlCommand($"INSERT INTO userTable (username, role, password)  VALUES  ('{username}','{role}','{password}')", con);
                SqlDataReader rdr = cmd.ExecuteReader();
            }
            return new IUser { username = username, password = password };
        }
        private bool verifyHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes("STATICHASH")))
            {
                var computedHashString = Encoding.UTF8.GetString(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
                var computedHash = Encoding.UTF8.GetBytes(computedHashString);
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateToken(IUser user)
        {
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimTypes.Name, user.username)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        private bool verifyToken(string token)
        {

            return false;
        }
    }

    public interface IAuthRepo
    {
        public IUser login(string username, string password);
        public IUser register(string username, string password, int role);
    }
}