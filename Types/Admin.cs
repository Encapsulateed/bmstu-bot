
namespace bmstu_bot.Types
{
    internal class Admin
    {
        public Models.Admin admin { get; set; }



        public Task Add()
        {

            return Task.Run(async () =>
            {

                using (Models.BmstuBotContext db = new Models.BmstuBotContext())
                {
                    try
                    {
                        await db.Admins.AddAsync(this.admin);
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateException ex)
                    {
                        Console.WriteLine($"Exeption in Admin.cs\nFunction: Admin.Add()\n\n");
                    }
                }
            });
        }

        public Task Update()
        {
            return Task.Run(async () =>
            {
                using (Models.BmstuBotContext db = new Models.BmstuBotContext())
                {
                    try
                    {
                        var user = db.Admins.Where(usr => usr.ChatId == this.admin.ChatId).FirstOrDefault();
                        db.Update(user).CurrentValues.SetValues(this.admin);

                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateException ex)
                    {
                        Console.WriteLine($"Exeption in Admin.cs\nFunction: Admin.Update()\\n\n");
                    }
                }
            });
        }

        public Task Get()
        {
            return Task.Run(() =>
            {
                using (Models.BmstuBotContext db = new Models.BmstuBotContext())
                {
                    try
                    {
                        var admin = db.Admins.Where(admin => admin.ChatId == this.admin.ChatId).FirstOrDefault();
                        if (admin is not null)
                        {
                            this.admin = admin;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"No such user exeption in Admin.cs\nFunction: Admin.Get()\n\n {ex}\n\n");
                    }

                }
            });

        }

        public Task Remove()
        {
            return Task.Run(async () =>
            {

                using (Models.BmstuBotContext db = new Models.BmstuBotContext())
                {
                    try
                    {
                        db.Admins.Remove(this.admin);
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateException ex)
                    {
                        Console.WriteLine($"Exeption in Admin.cs\nFunction: Admin.Add()\n\n");
                    }
                }
            });
        }
    }
}
