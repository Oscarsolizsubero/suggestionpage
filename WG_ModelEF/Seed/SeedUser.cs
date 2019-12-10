using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WishGrid.Models.Seed
{
   public class SeedUser : SeedBaseTemplate<User>
    {
        private byte[] _PasswordHash;
        private byte[] _PasswordSalt;
        private Action<string> GeneratePassword;
        private DataContext _DataContext;

        public SeedUser(DataContext dataContext)
        {
            _DBContext = _DataContext = dataContext;
            _Models = dataContext.User;
        }

        private void Encrypt(string value, out byte[] valueHash, out byte[] valueSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                valueSalt = hmac.Key;
                valueHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
        }

        public override User GetRecord(CsvReader csvReader)
        {
            Encrypt(csvReader.GetField("Password"), out _PasswordHash, out _PasswordSalt);
            return new User()
            {
                UserName = csvReader.GetField("UserName"),
                PasswordHash = _PasswordHash,
                PasswordSalt = _PasswordSalt,
                RoleId = csvReader.GetField<int>("RoleId"),
                TenantId = csvReader.GetField<int>("TenantId"),
                Email = csvReader.GetField("Email"),
                LastName = csvReader.GetField("LastName"),
                Name = csvReader.GetField("Name"),
                Validation = csvReader.GetField<bool>("Validation"),
            };
        }
        public override bool Initialize(string fileName, bool hasHeaderRecord = true)
        {
                bool any = _DataContext.User.Any();
                if (!any)
                {
                    var csv = new CsvReader(new StreamReader($"{GetPath()}\\{fileName}"));
                    csv.Configuration.HasHeaderRecord = hasHeaderRecord;
                    using (csv)
                    {
                        csv.Read();
                        if (hasHeaderRecord)
                        {
                            csv.ReadHeader();
                        }
                        while (csv.Read())
                        {
                            _DataContext.User.Add(GetRecord(csv));
                            _DataContext.SaveChanges();
                        }
                    }
                }
                return any;
            }
    }
}
